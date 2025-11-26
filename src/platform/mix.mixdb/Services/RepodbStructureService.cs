using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Mix.Constant.Constants;
using Mix.Constant.Enums;
using Mix.Database.Base;
using Mix.Database.Entities.Cms;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Heart.Enums;
using Mix.Heart.Exceptions;
using Mix.Heart.Extensions;
using Mix.Heart.Helpers;
using Mix.Heart.Services;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Extensions;
using Mix.Lib.Interfaces;
using Mix.Mixdb.Entities;
using Mix.Mixdb.Helpers;
using Mix.Mixdb.Interfaces;
using Mix.Mixdb.ViewModels;
using Mix.RepoDb.Models;
using Mix.RepoDb.Repositories;
using Mix.RepoDb.ViewModels;
using Mix.Service.Interfaces;
using Mix.Service.Services;
using Mix.Shared.Dtos;
using RepoDb;
using RepoDb.Enumerations;
using RepoDb.Interfaces;
using System.Data;
using System.Dynamic;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace Mix.Mixdb.Services
{
    /// <summary>
    /// Service for managing database structure operations using RepoDb
    /// </summary>
    public class RepodbStructureService : IMixdbStructureService, IDisposable
    {
        private readonly ILogger<RepodbStructureService> _logger;
        private readonly IDatabaseConstants _databaseConstant;
        private readonly MixDatabaseProvider _databaseProvider;
        private readonly UnitOfWorkInfo<MixCmsContext> _cmsUow;
        private readonly IConfiguration _configuration;
        private readonly DatabaseService _databaseService;
        private readonly ICache _cache;
        private readonly IMixMemoryCacheService _memoryCache;
        private readonly AppSetting _settings;
        private RepoDbRepository _repository;
        private RepoDbRepository _backupRepository;
        private bool _disposed;

        public MixDatabaseProvider DbProvider { get; }

        public RepodbStructureService(
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            UnitOfWorkInfo<MixCmsContext> uow,
            DatabaseService databaseService,
            IMixMemoryCacheService memoryCache,
            MixCacheService cacheService,
            IMixTenantService mixTenantService,
            ICache cache,
            ILogger<RepodbStructureService> logger)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _cmsUow = uow ?? throw new ArgumentNullException(nameof(uow));
            _databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));
            _databaseProvider = _databaseService.DatabaseProvider;
            _databaseConstant = _databaseProvider switch
            {
                MixDatabaseProvider.SQLSERVER => new SqlServerDatabaseConstants(),
                MixDatabaseProvider.MySQL => new MySqlDatabaseConstants(),
                MixDatabaseProvider.PostgreSQL => new PostgresDatabaseConstants(),
                MixDatabaseProvider.SQLITE => new SqliteDatabaseConstants(),
                MixDatabaseProvider.SCYLLADB => new CassandraDatabaseConstants(),
                _ => throw new NotImplementedException($"Database provider {_databaseProvider} is not supported")
            };
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _settings = new AppSetting()
            {
                CacheItemExpiration = 10,
                CommandTimeout = 1000
            };
        }

        #region Public Methods

        /// <summary>
        /// Initializes the database connection
        /// </summary>
        public void Init(string connectionString, MixDatabaseProvider dbProvider)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException("Connection string cannot be null or empty", nameof(connectionString));
            }

            try
            {
                _logger.LogInformation("Initializing database connection for provider: {Provider}", dbProvider);
                _repository = new RepoDbRepository(connectionString, dbProvider, _settings);
                _repository.CreateConnection(connectionString, true, true);
                _logger.LogInformation("Database connection initialized successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initializing database connection");
                throw;
            }
        }

        /// <summary>
        /// Executes a SQL command
        /// </summary>
        public async Task<int> ExecuteCommand(string commandText)
        {
            if (string.IsNullOrEmpty(commandText))
            {
                throw new ArgumentException("Command text cannot be null or empty", nameof(commandText));
            }

            try
            {
                _logger.LogDebug("Executing command: {Command}", commandText);
                var result = await _repository.ExecuteCommand(commandText);
                _repository.CompleteTransaction();
                _logger.LogDebug("Command executed successfully, affected rows: {Rows}", result);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing command: {Command}", commandText);
                throw;
            }
        }

        /// <summary>
        /// Adds a new column to the database
        /// </summary>
        public async Task AddColumn(MixDbTableViewModel database, MixdbColumnViewModel col, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Adding column {ColumnName} to table {TableName}", col.SystemName, col.MixDbTableName);

                cancellationToken.ThrowIfCancellationRequested();
                ValidateDatabaseAndColumn(database, col);

                var fieldNameService = new FieldNameService(database.NamingConvention);
                var commandText = GenerateAddColumnSql(col, database.DatabaseProvider, database.Type, fieldNameService);

                await ExecuteCommand(commandText);
                _logger.LogInformation("Column added successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding column {ColumnName} to table {TableName}", col.SystemName, col.MixDbTableName);
                throw;
            }
        }

        /// <summary>
        /// Alters an existing column in the database
        /// </summary>
        public async Task AlterColumn(MixDbTableViewModel database, MixdbColumnViewModel col, bool isDrop, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Altering column {ColumnName} in table {TableName}", col.SystemName, col.MixDbTableName);

                cancellationToken.ThrowIfCancellationRequested();
                ValidateDatabaseAndColumn(database, col);

                if (database.MixDbDatabase != null && database.MixDbDatabaseId != 1)
                {
                    _repository.InitializeRepoDb(
                        database.MixDbDatabase.ConnectionString.Decrypt(_configuration.AesKey()),
                        database.MixDbDatabase.DatabaseProvider);
                }

                var fieldNameService = new FieldNameService(database.NamingConvention);
                var alterCommandText = isDrop
                    ? $"{GenerateDropColumnSql(col)} {GenerateAddColumnSql(col, database.DatabaseProvider, database.Type, fieldNameService)}"
                    : GenerateAlterColumnSql(col);

                await ExecuteCommand(alterCommandText);
                _logger.LogInformation("Column altered successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error altering column {ColumnName} in table {TableName}", col.SystemName, col.MixDbTableName);
                throw;
            }
        }

        /// <summary>
        /// Drops a column from the database
        /// </summary>
        public async Task DropColumn(MixDbTableViewModel database, MixdbColumnViewModel col, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Dropping column {ColumnName} from table {TableName}", col.SystemName, col.MixDbTableName);

                cancellationToken.ThrowIfCancellationRequested();
                ValidateDatabaseAndColumn(database, col);

                if (database.MixDbDatabase != null)
                {
                    _repository.InitializeRepoDb(
                        database.MixDbDatabase.ConnectionString.Decrypt(_configuration.AesKey()),
                        database.MixDbDatabase.DatabaseProvider);
                }

                var alterCommandText = GenerateDropColumnSql(col);
                await ExecuteCommand(alterCommandText);
                _logger.LogInformation("Column dropped successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error dropping column {ColumnName} from table {TableName}", col.SystemName, col.MixDbTableName);
                throw;
            }
        }

        /// <summary>
        /// Migrates the database structure
        /// </summary>
        public async Task Migrate(
            MixDbTableViewModel database,
            MixDatabaseProvider databaseProvider,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Starting migration for database: {DatabaseName}", database.SystemName);

                cancellationToken.ThrowIfCancellationRequested();
                ValidateDatabaseForMigration(database);

                var fieldNameService = new FieldNameService(database.NamingConvention);
                var colsSql = new List<string>();
                var tableName = database.SystemName;
                var databaseConstant = MixDbHelper.GetDatabaseConstant(databaseProvider);

                foreach (var col in database.Columns)
                {
                    colsSql.Add(GenerateColumnSql(col, databaseProvider, database.Type, fieldNameService));
                }

                var commandText = GetMigrateTableSql(tableName, colsSql);
                if (!string.IsNullOrEmpty(commandText))
                {
                    string dropSql = $"DROP TABLE IF EXISTS {_databaseConstant.BacktickOpen}{tableName}{_databaseConstant.BacktickClose};";
                    await _cmsUow.DbContext.Database.ExecuteSqlRawAsync(dropSql);
                    await _cmsUow.DbContext.Database.ExecuteSqlRawAsync(commandText);
                }

                _logger.LogInformation("Migration completed successfully for database: {DatabaseName}", database.SystemName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error migrating database: {DatabaseName}", database.SystemName);
                throw;
            }
        }

        /// <summary>
        /// Executes a SQL query with parameters
        /// </summary>
        public async Task ExecuteSqlAsync(string sql, params object[] parameters)
        {
            if (string.IsNullOrEmpty(sql))
            {
                throw new ArgumentException("SQL query cannot be null or empty", nameof(sql));
            }

            try
            {
                _logger.LogDebug("Executing SQL query: {Sql}", sql);
                await _cmsUow.DbContext.Database.ExecuteSqlRawAsync(sql, parameters);
                _logger.LogDebug("SQL query executed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing SQL query: {Sql}", sql);
                throw;
            }
        }

        #endregion

        #region Private Methods

        private void ValidateDatabaseAndColumn(MixDbTableViewModel database, MixdbColumnViewModel col)
        {
            if (database == null)
            {
                throw new MixException(MixErrorStatus.Badrequest, $"Invalid table {col.MixDbTableName}");
            }

            if (col == null)
            {
                throw new ArgumentNullException(nameof(col));
            }

            if (string.IsNullOrEmpty(col.SystemName))
            {
                throw new ArgumentException("Column name cannot be null or empty", nameof(col));
            }
        }

        private void ValidateDatabaseForMigration(MixDbTableViewModel database)
        {
            if (database == null)
            {
                throw new ArgumentNullException(nameof(database));
            }

            if (string.IsNullOrEmpty(database.SystemName))
            {
                throw new ArgumentException("Database name cannot be null or empty", nameof(database));
            }

            if (database.Columns == null || !database.Columns.Any())
            {
                throw new ArgumentException("Database must have at least one column", nameof(database));
            }
        }

        private string GetMigrateTableSql(string tableName, List<string> colsSql)
        {
            return $"CREATE TABLE {_databaseConstant.BacktickOpen}{tableName}{_databaseConstant.BacktickClose} (" +
                   $" {string.Join(",", colsSql.ToArray())})";
        }

        private string GetIdSyntax(MixDatabaseProvider databaseProvider, MixDbTableType dbType)
        {
            if (dbType == MixDbTableType.GuidService)
            {
                return $"{_databaseConstant.BacktickOpen}{_databaseConstant.Guid}{_databaseConstant.BacktickClose} PRIMARY KEY";
            }

            return databaseProvider switch
            {
                MixDatabaseProvider.SQLSERVER => $"{GetColumnType(MixDataType.Integer)} IDENTITY(1,1) PRIMARY KEY",
                MixDatabaseProvider.SQLITE => $"integer PRIMARY KEY AUTOINCREMENT",
                MixDatabaseProvider.PostgreSQL => "SERIAL PRIMARY KEY",
                MixDatabaseProvider.MySQL => $"{GetColumnType(MixDataType.Integer)} NOT NULL AUTO_INCREMENT PRIMARY KEY",
                _ => string.Empty
            };
        }

        private string GenerateColumnSql(MixdbColumnViewModel col, MixDatabaseProvider databaseProvider,
            MixDbTableType dbType, FieldNameService fieldNameService)
        {
            if (string.Equals(col.SystemName, "id", StringComparison.OrdinalIgnoreCase))
            {
                return $"{_databaseConstant.BacktickOpen}id{_databaseConstant.BacktickClose} {GetIdSyntax(databaseProvider, dbType)}";
            }

            string colType = GetColumnType(col.DataType, col.ColumnConfigurations.MaxLength);
            string nullable = col.ColumnConfigurations.IsRequire ? "NOT NUll" : "NULL";
            string unique = col.ColumnConfigurations.IsUnique ? "Unique" : "";
            string defaultValue = !IsLongTextColumn(col) && !string.IsNullOrEmpty(col.DefaultValue)
                ? $"DEFAULT '{@col.DefaultValue}'"
                : string.Empty;

            return $"{_databaseConstant.BacktickOpen}{col.SystemName}{_databaseConstant.BacktickClose} {colType} {nullable} {unique} {defaultValue}";
        }

        private string GenerateAddColumnSql(MixdbColumnViewModel col, MixDatabaseProvider databaseProvider,
            MixDbTableType type, FieldNameService fieldNameService)
        {
            return $"ALTER TABLE {_databaseConstant.BacktickOpen}{col.MixDbTableName}{_databaseConstant.BacktickClose}" +
                   $" ADD {GenerateColumnSql(col, databaseProvider, type, fieldNameService)};";
        }

        private string GenerateDropColumnSql(MixdbColumnViewModel col)
        {
            return $"ALTER TABLE {_databaseConstant.BacktickOpen}{col.MixDbTableName}{_databaseConstant.BacktickClose}" +
                   $" DROP COLUMN IF EXISTS {_databaseConstant.BacktickOpen}{col.SystemName}{_databaseConstant.BacktickClose};";
        }

        private string GenerateAlterColumnSql(MixdbColumnViewModel col)
        {
            string colType = GetColumnType(col.DataType, col.ColumnConfigurations.MaxLength);
            string colName = $"{_databaseConstant.BacktickOpen}{col.SystemName}{_databaseConstant.BacktickClose}";
            string tableName = $"{_databaseConstant.BacktickOpen}{col.MixDbTableName}{_databaseConstant.BacktickClose}";
            string uniqueKey = $"{_databaseConstant.BacktickOpen}{col.MixDbTableName}_{col.SystemName}_key{_databaseConstant.BacktickClose}";
            string defaultValue = !IsLongTextColumn(col) && !string.IsNullOrEmpty(col.DefaultValue)
                ? $"DEFAULT '{@col.DefaultValue}'"
                : string.Empty;
            string alterTable = $"ALTER TABLE {tableName} ";

            var result = alterTable +
                        $" ALTER COLUMN {colName} TYPE {colType} USING {colName}::{colType};"
                        + alterTable
                        + $" ALTER COLUMN {colName} DROP NOT NULL;";

            if (col.ColumnConfigurations.IsRequire)
            {
                result += alterTable + $" ALTER COLUMN {colName} SET NOT NULL;";
            }

            if (!string.IsNullOrEmpty(defaultValue))
            {
                result += alterTable + $" ALTER COLUMN {colName} SET {defaultValue};";
            }

            result += alterTable + $" DROP CONSTRAINT IF EXISTS {uniqueKey};";
            if (col.ColumnConfigurations.IsUnique)
            {
                result += alterTable + $" ADD CONSTRAINT {uniqueKey} UNIQUE ({colName});";
            }

            return result;
        }

        private bool IsLongTextColumn(MixdbColumnViewModel col)
        {
            return col.DataType == MixDataType.Text
                   | col.DataType == MixDataType.Array
                   | col.DataType == MixDataType.ArrayMedia
                   | col.DataType == MixDataType.ArrayRadio
                   | col.DataType == MixDataType.Html
                   | col.DataType == MixDataType.Json
                   | col.DataType == MixDataType.TuiEditor;
        }

        private string GetColumnType(MixDataType dataType, int? maxLength = null)
        {
            return dataType switch
            {
                MixDataType.Date => _databaseConstant.Date,
                MixDataType.DateTime or MixDataType.Time => _databaseConstant.DateTime,
                MixDataType.Double => "float",
                MixDataType.Reference or MixDataType.Integer => _databaseConstant.Integer,
                MixDataType.Long => _databaseConstant.Long,
                MixDataType.Guid => _databaseConstant.Guid,
                MixDataType.Html => _databaseConstant.Text,
                MixDataType.Boolean => _databaseConstant.Boolean,
                MixDataType.Json or MixDataType.Array or MixDataType.ArrayMedia or
                MixDataType.ArrayRadio or MixDataType.Text => _databaseConstant.Text,
                _ => $"{_databaseConstant.NString}({maxLength ?? 250})"
            };
        }

        #endregion

        #region IDisposable Implementation

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _repository?.Dispose();
                _backupRepository?.Dispose();
                _cmsUow?.Dispose();
            }

            _disposed = true;
        }

        #endregion
    }
}
