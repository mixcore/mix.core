using Mix.Constant.Enums;
using Mix.Database.Base;
using Mix.Database.Entities.Cms;
using Mix.Database.Services;
using Mix.Heart.Enums;
using Mix.Heart.Extensions;
using Mix.Heart.UnitOfWork;
using Mix.RepoDb.Entities;
using Mix.RepoDb.Repositories;
using Mix.RepoDb.ViewModels;
using RepoDb.Interfaces;
using System.Dynamic;

namespace Mix.RepoDb.Services
{
    public class MixDbService : IDisposable
    {
        private readonly IDatabaseConstants _databaseConstant;
        private readonly MixRepoDbRepository _repository;
        private readonly MixRepoDbRepository _backupRepository;

        #region Properties

        private readonly UnitOfWorkInfo<MixCmsContext> _uow;
        private readonly DatabaseService _databaseService;

        private static readonly string[] DefaultProperties =
        {
            "Id",
            "CreatedDateTime",
            "LastModified",
            "MixTenantId",
            "CreatedBy",
            "ModifiedBy",
            "Priority",
            "Status",
            "IsDeleted"
        };
        #endregion

        public MixDbService(
            UnitOfWorkInfo<MixCmsContext> uow,
            DatabaseService databaseService,
            MixRepoDbRepository repository,
            ICache cache)
        {
            _uow = uow;
            _databaseService = databaseService;
            _repository = repository;
            _backupRepository = new MixRepoDbRepository(cache, databaseService, uow);
            _databaseConstant = _databaseService.DatabaseProvider switch
            {
                MixDatabaseProvider.SQLSERVER => new SqlServerDatabaseConstants(),
                MixDatabaseProvider.MySQL => new MySqlDatabaseConstants(),
                MixDatabaseProvider.PostgreSQL => new PostgresDatabaseConstants(),
                MixDatabaseProvider.SQLITE => new SqliteDatabaseConstants(),
                _ => throw new NotImplementedException()
            };
        }

        #region Methods

        // TODO: check why need to restart application to load new database schema for Repo Db Context !important
        public async Task<bool> MigrateDatabase(string name)
        {
            MixDatabaseViewModel database = await MixDatabaseViewModel.GetRepository(_uow).GetSingleAsync(m => m.SystemName == name);
            if (database != null && database.Columns.Count > 0)
            {
                //await BackupDatabase(database.SystemName);
                await Migrate(database, _databaseService.DatabaseProvider, _repository);
                //await RestoreFromLocal(database);
                return true;
            }
            return false;
        }

        // TODO: check why need to restart application to load new database schema for Repo Db Context !important
        public async Task<bool> RestoreFromLocal(string name)
        {
            MixDatabaseViewModel database = await MixDatabaseViewModel.GetRepository(_uow).GetSingleAsync(m => m.SystemName == name);
            if (database != null && database.Columns.Count > 0)
            {
                return await RestoreFromLocal(database);
            }
            return false;
        }

        public async Task<bool> BackupDatabase(string databaseName, CancellationToken cancellationToken = default)
        {
            var database = await MixDatabaseViewModel.GetRepository(_uow).GetSingleAsync(m => m.SystemName == databaseName, cancellationToken);
            if (database != null)
            {
                return await BackupToLocal(database, cancellationToken);
            }
            return false;
        }

        #endregion

        #region Private

        private async Task<bool> BackupToLocal(MixDatabaseViewModel database, CancellationToken cancellationToken = default)
        {
            var data = await GetCurrentData(database.SystemName, cancellationToken);
            if (data != null && data.Count > 0)
            {
                InitBackupRepository(database.SystemName);
                await Migrate(database, _backupRepository.DatabaseProvider, _backupRepository);
                foreach (var item in data)
                {
                    GetMembers(item, database.Columns.Select(c => c.SystemName.ToTitleCase()).ToList());
                }
                var result = await _backupRepository.InsertManyAsync(data);
                return result > 0;
            }
            return true;
        }

        private void InitBackupRepository(string databaseName)
        {
            string cnn = $"Data Source=MixContent/Backup/backup_{databaseName}.db";
            using var ctx = new BackupDbContext(cnn);
            ctx.Database.EnsureCreated();
            ctx.Dispose();
            _backupRepository.Init(databaseName, MixDatabaseProvider.SQLITE, cnn);

        }

        private async Task<bool> RestoreFromLocal(MixDatabaseViewModel database)
        {
            InitBackupRepository(database.SystemName);
            var data = await _backupRepository.GetAllAsync();
            if (data != null && data.Count > 0)
            {
                foreach (var item in data)
                {
                    GetMembers(item, database.Columns.Select(c => c.SystemName.ToTitleCase()).ToList());
                }
                _repository.InitTableName(database.SystemName);
                var result = await _repository.InsertManyAsync(data);
                return result >= 0;
            }
            return true;
        }

        private void GetMembers(ExpandoObject obj, List<string> selectMembers)
        {
            var result = obj.ToList();
            foreach (KeyValuePair<string, object> kvp in result)
            {
                if (DefaultProperties.All(m => m != kvp.Key) && selectMembers.All(m => m != kvp.Key))
                {
                    obj!.Remove(kvp.Key, out _);
                }
            }
        }

        private async Task<List<dynamic>?> GetCurrentData(string databaseName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            _repository.InitTableName(databaseName);
            return await _repository.GetAllAsync(cancellationToken);
        }

        private async Task<bool> Migrate(MixDatabaseViewModel database, MixDatabaseProvider databaseProvider, MixRepoDbRepository repo)
        {
            var colsSql = new List<string>();
            var tableName = database.SystemName.ToLower();

            foreach (var col in database.Columns)
            {
                colsSql.Add(GenerateColumnSql(col));
            }

            var commandText = GetMigrateTableSql(tableName, databaseProvider, colsSql);
            if (!string.IsNullOrEmpty(commandText))
            {
                await repo.ExecuteCommand($"DROP TABLE IF EXISTS {_databaseConstant.BacktickOpen}{tableName}{_databaseConstant.BacktickClose};");
                var result = await repo.ExecuteCommand(commandText);
                return result >= 0;
            }

            return false;
        }

        private string GetMigrateTableSql(string tableName, MixDatabaseProvider databaseProvider, List<string> colsSql)
        {
            return $"CREATE TABLE {_databaseConstant.BacktickOpen}{tableName}{_databaseConstant.BacktickClose} " +
                $"(Id {GetAutoIncreaseIdSyntax(databaseProvider)}, " +
                $"CreatedDateTime {GetColumnType(MixDataType.DateTime)}, " +
                $"LastModified {GetColumnType(MixDataType.DateTime)} NULL, " +
                $"MixTenantId {GetColumnType(MixDataType.Integer)} NULL, " +
                $"CreatedBy {GetColumnType(MixDataType.Text)} NULL, " +
                $"ModifiedBy {GetColumnType(MixDataType.Text)} NULL, " +
                $"Priority {GetColumnType(MixDataType.Integer)} NOT NULL, " +
                $"Status {GetColumnType(MixDataType.Text)} NULL, " +
                $"IsDeleted {GetColumnType(MixDataType.Boolean)} NOT NULL, " +
                $" {string.Join(",", colsSql.ToArray())})";
        }

        private string GetAutoIncreaseIdSyntax(MixDatabaseProvider databaseProvider)
        {
            return databaseProvider switch
            {
                MixDatabaseProvider.SQLSERVER => $"{GetColumnType(MixDataType.Integer)} IDENTITY(1,1) PRIMARY KEY",
                MixDatabaseProvider.SQLITE => $"integer PRIMARY KEY AUTOINCREMENT",
                MixDatabaseProvider.PostgreSQL => "SERIAL PRIMARY KEY",
                MixDatabaseProvider.MySQL => $"{GetColumnType(MixDataType.Integer)} NOT NULL AUTO_INCREMENT PRIMARY KEY",
                _ => string.Empty
            };
        }

        private string GenerateColumnSql(MixDatabaseColumnViewModel col)
        {
            string colType = GetColumnType(col.DataType, col.ColumnConfigurations.MaxLength);
            string nullable = col.ColumnConfigurations.IsRequire ? "NOT NUll" : "NULL";
            string unique = col.ColumnConfigurations.IsUnique ? "Unique" : "";
            return $"{_databaseConstant.BacktickOpen}{col.SystemName.ToTitleCase()}{_databaseConstant.BacktickClose} {colType} {nullable} {unique}";
        }

        private string GetColumnType(MixDataType dataType, int? maxLength = null)
        {
            switch (dataType)
            {
                case MixDataType.DateTime:
                case MixDataType.Date:
                case MixDataType.Time:
                    return _databaseConstant.DateTime;
                case MixDataType.Double:
                    return "float";
                case MixDataType.Reference:
                case MixDataType.Integer:
                    return _databaseConstant.Integer;
                case MixDataType.Guid:
                    return _databaseConstant.Guid;
                case MixDataType.Html:
                    return _databaseConstant.Text;
                case MixDataType.Boolean:
                    return _databaseConstant.Boolean;
                case MixDataType.Json:
                case MixDataType.Array:
                    return $"{_databaseConstant.NString}{_databaseConstant.MaxLength}";
                case MixDataType.Duration:
                case MixDataType.Custom:
                case MixDataType.PhoneNumber:
                case MixDataType.Text:
                case MixDataType.MultilineText:
                case MixDataType.EmailAddress:
                case MixDataType.Password:
                case MixDataType.Url:
                case MixDataType.ImageUrl:
                case MixDataType.CreditCard:
                case MixDataType.PostalCode:
                case MixDataType.Upload:
                case MixDataType.Color:
                case MixDataType.Icon:
                case MixDataType.VideoYoutube:
                case MixDataType.TuiEditor:
                case MixDataType.QRCode:
                default:
                    return $"{_databaseConstant.NString}({maxLength ?? 250})";
            }
        }

        public void Dispose()
        {
            _repository.Dispose();
            _backupRepository.Dispose();
            _uow.Dispose();
        }
        #endregion
    }
}
