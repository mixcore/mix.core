using Microsoft.EntityFrameworkCore;
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
    public class MixDbService
    {
        private IDatabaseConstants _dbConstants;
        private MixRepoDbRepository _repository;
        private MixRepoDbRepository _backupRepository;

        #region Properties

        public ITrace Trace { get; }

        public ICache Cache { get; }
        private UnitOfWorkInfo<MixCmsContext> _uow;
        private DatabaseService _databaseService;

        static string[] _defaultProperties = { "Id", "CreatedDateTime", "LastModified", "MixTenantId", "CreatedBy", "ModifiedBy", "Priority", "Status", "IsDeleted" };
        #endregion

        public MixDbService(UnitOfWorkInfo<MixCmsContext> uow, DatabaseService databaseService, MixRepoDbRepository repository,
            ICache cache)
        {
            _uow = uow;
            _databaseService = databaseService;
            _repository = repository;
            _backupRepository = new(cache, databaseService);
            _dbConstants = _databaseService.DatabaseProvider switch
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

        public async Task<bool> BackupDatabase(string databaseName)
        {
            var database = await MixDatabaseViewModel.GetRepository(_uow).GetSingleAsync(m => m.SystemName == databaseName);
            if (database != null)
            {
                return await BackupToLocal(database);
            }
            return false;
        }

        #endregion

        #region Private

        private async Task<bool> BackupToLocal(MixDatabaseViewModel database)
        {
            var data = await GetCurrentData(database.SystemName);
            if (data != null && data.Count > 0)
            {
                InitBackupRepository(database.SystemName);
                await Migrate(database, _backupRepository.DatabaseProvider, _backupRepository);
                foreach (var item in data)
                {
                    GetMembers(item, database.Columns.Select(c => c.SystemName.ToTitleCase()));
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
                    GetMembers(item, database.Columns.Select(c => c.SystemName.ToTitleCase()));
                }
                _repository.InitTableName(database.SystemName);
                var result = await _repository.InsertManyAsync(data);
                return result >= 0;
            }
            return true;
        }

        private void GetMembers(ExpandoObject obj, IEnumerable<string> selectMembers)
        {
            var result = obj.ToList();
            foreach (KeyValuePair<string, object> kvp in result)
            {
                if (!_defaultProperties.Any(m => m == kvp.Key) && !selectMembers.Any(m => m == kvp.Key))
                {
                    obj!.Remove(kvp.Key, out _);
                }
            }
        }

        private async Task<List<dynamic>?> GetCurrentData(string databaseName)
        {
            _repository.InitTableName(databaseName);
            return await _repository.GetAllAsync();
        }

        private async Task<bool> Migrate(MixDatabaseViewModel database, MixDatabaseProvider databaseProvider, MixRepoDbRepository repo)
        {
            List<string> colSqls = new List<string>();
            string tableName = database.SystemName.ToLower();
            foreach (var col in database.Columns)
            {
                colSqls.Add(GenerateColumnSql(col));
            }

            var commandText = GetMigrateTableSql(tableName, databaseProvider, colSqls);
            if (!string.IsNullOrEmpty(commandText))
            {
                await repo.ExecuteCommand($"DROP TABLE IF EXISTS {tableName};");
                var result = await repo.ExecuteCommand(commandText);
                return result >= 0;
            }
            return false;
        }

        private string GetMigrateTableSql(string tableName, MixDatabaseProvider databaseProvider, List<string> colSqls)
        {
            return $"CREATE TABLE {tableName} " +
                $"(Id {GetAutoIncreaseIdSyntax(databaseProvider)}, " +
                $"CreatedDateTime {GetColumnType(MixDataType.DateTime)}, " +
                $"LastModified {GetColumnType(MixDataType.DateTime)} NULL, " +
                $"MixTenantId {GetColumnType(MixDataType.Integer)} NULL, " +
                $"CreatedBy {GetColumnType(MixDataType.Text)} NULL, " +
                $"ModifiedBy {GetColumnType(MixDataType.Text)} NULL, " +
                $"Priority {GetColumnType(MixDataType.Integer)} NOT NULL, " +
                $"Status {GetColumnType(MixDataType.Text)} NULL, " +
                $"IsDeleted {GetColumnType(MixDataType.Boolean)} NULL, " +
                $" {string.Join(",", colSqls.ToArray())})";
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
            return $"{_dbConstants.BacktickOpen}{col.SystemName.ToTitleCase()}{_dbConstants.BacktickClose} {colType} {nullable}";
        }

        private string GetColumnType(MixDataType dataType, int? maxLength = null)
        {
            switch (dataType)
            {
                case MixDataType.DateTime:
                case MixDataType.Date:
                case MixDataType.Time:
                    return _dbConstants.DateTime;
                case MixDataType.Double:
                    return "float";
                case MixDataType.Reference:
                case MixDataType.Integer:
                    return _dbConstants.Integer;
                case MixDataType.Guid:
                    return _dbConstants.Guid;
                case MixDataType.Html:
                    return _dbConstants.Text;
                case MixDataType.Boolean:
                    return _dbConstants.Boolean;
                case MixDataType.Json:
                    return $"{_dbConstants.NString}{_dbConstants.MaxLength}";
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
                    return $"{_dbConstants.NString}({maxLength ?? 250})";
            }
        }
        #endregion

    }
}
