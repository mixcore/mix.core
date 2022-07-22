using Microsoft.EntityFrameworkCore;
using Mix.Constant.Enums;
using Mix.Database.Entities.Cms;
using Mix.Database.Services;
using Mix.Heart.Enums;
using Mix.Heart.UnitOfWork;
using Mix.RepoDb.Entities;
using Mix.RepoDb.Repositories;
using Mix.RepoDb.ViewModels;
using RepoDb.Interfaces;

namespace Mix.RepoDb.Services
{
    public class MixDbService
    {
        private MixRepoDbRepository _repository;
        private MixRepoDbRepository _backupRepository;

        #region Properties

        public ITrace Trace { get; }

        public ICache Cache { get; }
        private UnitOfWorkInfo<MixCmsContext> _uow;
        private DatabaseService _databaseService;
        #endregion

        public MixDbService(UnitOfWorkInfo<MixCmsContext> uow, DatabaseService databaseService, MixRepoDbRepository repository,
            ICache cache)
        {
            _uow = uow;
            _databaseService = databaseService;
            _repository = repository;
            _backupRepository = new(cache, databaseService, uow);
        }

        #region Methods

        // TODO: check why need to restart application to load new database schema for Repo Db Context !important
        public async Task<bool> MigrateDatabase(string name)
        {
            MixDatabaseViewModel database = await MixDatabaseViewModel.GetRepository(_uow).GetSingleAsync(m => m.SystemName == name);
            if (database != null && database.Columns.Count > 0)
            {
                await BackupToLocal(database);
                await Migrate(database, _databaseService.DatabaseProvider, _uow.DbContext);
                await RestoreFromLocal(database);
                return true;
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
                await Migrate(database, _backupRepository.DatabaseProvider, new BackupDbContext(_backupRepository.ConnectionString));
                var result = await _backupRepository.InsertManyAsync(data);
                return result > 0;
            }
            return false;
        }

        private void InitBackupRepository(string databaseName)
        {
            string cnn = $"Data Source=MixContent/backup/backup_{databaseName}.db";
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
                _repository.Init(database.SystemName);
                var result = await _repository.InsertManyAsync(data);
                return result >= 0;
            }
            return false;
        }

        private async Task<List<dynamic>?> GetCurrentData(string databaseName)
        {
            _repository.Init(databaseName);
            return await _repository.GetAllAsync();
        }
        private async Task<bool> Migrate(MixDatabaseViewModel database, MixDatabaseProvider databaseProvider, DbContext ctx)
        {
            List<string> colSqls = new List<string>();
            foreach (var col in database.Columns)
            {
                colSqls.Add(GenerateColumnSql(col));
            }

            var commandText = GetMigrateTableSql(database.SystemName, databaseProvider, colSqls);

            if (!string.IsNullOrEmpty(commandText))
            {
                var result = await ctx.Database.ExecuteSqlRawAsync(commandText);
                return result >= 0;
            }
            return false;
        }

        private string GetMigrateTableSql(string tableName, MixDatabaseProvider databaseProvider, List<string> colSqls)
        {
            return $"DROP TABLE IF EXISTS {tableName}; " +
                $"CREATE TABLE {tableName} " +
                $"(id {GetAutoIncreaseIdSyntax(databaseProvider)}, createdDateTime {GetColumnType(MixDataType.DateTime)}, " +
                $" {string.Join(",", colSqls.ToArray())})";
        }

        private string GetAutoIncreaseIdSyntax(MixDatabaseProvider databaseProvider)
        {
            return databaseProvider switch
            {
                MixDatabaseProvider.SQLSERVER => "int IDENTITY(1,1) PRIMARY KEY",
                MixDatabaseProvider.SQLITE => "INTEGER PRIMARY KEY AUTOINCREMENT",
                MixDatabaseProvider.PostgreSQL => "SERIAL PRIMARY KEY",
                MixDatabaseProvider.MySQL => "int NOT NULL AUTO_INCREMENT PRIMARY KEY",
                _ => string.Empty
            };
        }

        private string GenerateColumnSql(MixDatabaseColumnViewModel col)
        {

            string colType = GetColumnType(col.DataType, col.ColumnConfigurations.MaxLength);
            string nullable = col.ColumnConfigurations.IsRequire ? "NOT NUll" : "NULL";
            return $"{col.SystemName} {colType} {nullable}";
        }

        private string GetColumnType(MixDataType dataType, int? maxLength = null)
        {
            switch (dataType)
            {
                case MixDataType.DateTime:
                case MixDataType.Date:
                case MixDataType.Time:
                    return _databaseService.DatabaseProvider switch
                    {
                        MixDatabaseProvider.PostgreSQL => "timestamp without time zone",
                        _ => "datetime"
                    };
                case MixDataType.Double:
                    return "float";
                case MixDataType.Reference:
                case MixDataType.Integer:
                    return "int";
                case MixDataType.Html:
                    return _databaseService.DatabaseProvider switch
                    {
                        MixDatabaseProvider.SQLSERVER => "ntext",
                        _ => "text"
                    };
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
                case MixDataType.Boolean:
                case MixDataType.Icon:
                case MixDataType.VideoYoutube:
                case MixDataType.TuiEditor:
                case MixDataType.QRCode:
                default:
                    return $"varchar({maxLength ?? 250})";
            }
        }
        #endregion

    }
}
