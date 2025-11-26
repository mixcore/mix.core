using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Mix.Constant.Constants;
using Mix.Constant.Enums;
using Mix.Database.Base;
using Mix.Database.Entities.Cms;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Heart.Constants;
using Mix.Heart.Enums;
using Mix.Heart.Exceptions;
using Mix.Heart.Extensions;
using Mix.Heart.Helpers;
using Mix.Heart.Services;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Extensions;
using Mix.Lib.Interfaces;
using Mix.Mixdb.Interfaces;
using Mix.Mixdb.ViewModels;
using Mix.RepoDb.Models;
using Mix.RepoDb.Repositories;
using Mix.Service.Interfaces;
using Mix.Service.Services;
using Newtonsoft.Json.Linq;
using RepoDb.Interfaces;
using System.Dynamic;

namespace Mix.Mixdb.Services
{
    /// <summary>
    /// Service for managing database structure operations
    /// </summary>
    public class MixdbStructureService : TenantServiceBase, IMixdbStructure
    {
        private readonly ILogger<MixdbStructureService> _logger;
        private readonly IEnumerable<IMixdbStructureService> _dbStructureServices;
        private readonly IDatabaseConstants _databaseConstant;
        private readonly MixDatabaseProvider _databaseProvider;
        private readonly IMixMemoryCacheService _memoryCache;
        private readonly IConfiguration _configuration;
        private readonly UnitOfWorkInfo<MixCmsContext> _cmsUow;
        private readonly DatabaseService _databaseService;
        private readonly ICache _cache;
        private readonly RepoDbRepository _repository;
        private readonly AppSetting _settings;
        private readonly RuntimeDbContextService _runtimeDbContextService;
        private IMixdbStructureService _dbStructureSrv;

        public MixdbStructureService(
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            UnitOfWorkInfo<MixCmsContext> uow,
            DatabaseService databaseService,
            IMixMemoryCacheService memoryCache,
            MixCacheService cacheService,
            IMixTenantService mixTenantService,
            ICache cache,
            IEnumerable<IMixdbStructureService> dbStructureServices,
            ILogger<MixdbStructureService> logger) : base(httpContextAccessor, cacheService, mixTenantService)
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
            _runtimeDbContextService = new RuntimeDbContextService(httpContextAccessor, databaseService);
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _settings = new AppSetting()
            {
                CacheItemExpiration = 10,
                CommandTimeout = 1000
            };
            _repository = new(
               databaseService.GetConnectionString(MixConstants.CONST_MIXDB_CONNECTION),
               databaseService.DatabaseProvider,
               _settings);
            _dbStructureServices = dbStructureServices ?? throw new ArgumentNullException(nameof(dbStructureServices));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Migrates and initializes new database context
        /// </summary>
        public async Task MigrateInitNewDbContextDatabases(MixDbDatabase dbContext, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Starting migration for new database context: {ContextName}", dbContext.SystemName);

                cancellationToken.ThrowIfCancellationRequested();
                ValidateDatabaseContext(dbContext);

                var cnn = GetDecryptedConnectionString(dbContext.ConnectionString);
                InitDbStructureService(cnn, dbContext.DatabaseProvider);

                if (dbContext.DatabaseProvider == MixDatabaseProvider.PostgreSQL)
                {
                    await _dbStructureSrv.ExecuteCommand("CREATE EXTENSION IF NOT EXISTS \"unaccent\";");
                }

                var strMixDbs = MixFileHelper.GetFile(
                    "init-new-mixdb-database-tables", MixFileExtensions.Json, MixFolders.JsonDataFolder);
                var obj = JObject.Parse(strMixDbs.Content);
                var databases = obj.Value<JArray>("tables")?.ToObject<List<MixDbTable>>();
                var columns = obj.Value<JArray>("columns")?.ToObject<List<MixDbColumn>>();

                if (databases != null)
                {
                    await ProcessDatabasesAsync(databases, columns, dbContext, cancellationToken);
                }

                _logger.LogInformation("Successfully completed migration for database context: {ContextName}", dbContext.SystemName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error migrating database context: {ContextName}", dbContext.SystemName);
                throw new MixException(MixErrorStatus.Badrequest, ex);
            }
        }

        /// <summary>
        /// Migrates system databases
        /// </summary>
        public async Task MigrateSystemDatabases(CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Starting system databases migration");

                cancellationToken.ThrowIfCancellationRequested();

                var strMixDbs = MixFileHelper.GetFile(
                    "system-tables", MixFileExtensions.Json, MixFolders.JsonDataFolder);
                var obj = JObject.Parse(strMixDbs.Content);
                var databases = obj.Value<JArray>("tables")?.ToObject<List<MixDbTable>>();
                var columns = obj.Value<JArray>("columns")?.ToObject<List<MixDbColumn>>();

                var masterDbContext = await GetOrCreateMasterDbContextAsync();

                if (databases != null)
                {
                    await ProcessSystemDatabasesAsync(databases, columns, masterDbContext, cancellationToken);
                }

                _logger.LogInformation("Successfully completed system databases migration");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error migrating system databases");
                throw;
            }
        }

        #region Private Methods

        private void ValidateDatabaseContext(MixDbDatabase dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            if (string.IsNullOrEmpty(dbContext.ConnectionString))
            {
                throw new ArgumentException("Connection string cannot be null or empty", nameof(dbContext));
            }

            if (string.IsNullOrEmpty(dbContext.SystemName))
            {
                throw new ArgumentException("System name cannot be null or empty", nameof(dbContext));
            }
        }

        private string GetDecryptedConnectionString(string connectionString)
        {
            if (AesEncryptionHelper.IsEncrypted(connectionString, _configuration.AesKey()))
            {
                return AesEncryptionHelper.DecryptString(connectionString, _configuration.AesKey());
            }
            return connectionString;
        }

        private async Task ProcessDatabasesAsync(
            List<MixDbTable> databases,
            List<MixDbColumn>? columns,
            MixDbDatabase dbContext,
            CancellationToken cancellationToken)
        {
            foreach (var database in databases)
            {
                string newDbName = GetNewDatabaseName(dbContext, database);
                var currentDb = await GetOrCreateDatabaseAsync(database, newDbName, dbContext, columns, cancellationToken);

                if (currentDb is { Columns.Count: > 0 })
                {
                    await _dbStructureSrv.Migrate(currentDb, dbContext.DatabaseProvider, cancellationToken);
                }
            }
        }

        private string GetNewDatabaseName(MixDbDatabase dbContext, MixDbTable database)
        {
            return dbContext.NamingConvention == MixDatabaseNamingConvention.SnakeCase
                ? $"{dbContext.SystemName}_{database.DisplayName.ToColumnName(false)}"
                : $"{dbContext.SystemName.ToTitleCase()}{database.DisplayName.ToColumnName(true)}";
        }

        private async Task<MixDbTableViewModel> GetOrCreateDatabaseAsync(
            MixDbTable database,
            string newDbName,
            MixDbDatabase dbContext,
            List<MixDbColumn>? columns,
            CancellationToken cancellationToken)
        {
            var currentDb = await MixDbTableViewModel.GetRepository(_cmsUow, CacheService)
                .GetSingleAsync(m => m.SystemName == newDbName);

            if (currentDb == null)
            {
                currentDb = new(database, _cmsUow)
                {
                    Id = 0,
                    NamingConvention = dbContext.NamingConvention,
                    SystemName = newDbName,
                    TenantId = CurrentTenant?.Id ?? 1,
                    MixDbDatabaseId = dbContext.Id,
                    DatabaseProvider = dbContext.DatabaseProvider,
                    CreatedDateTime = DateTime.UtcNow,
                    Columns = new()
                };

                if (columns is not null)
                {
                    currentDb.AddDefaultColumns();
                    await AddColumnsAsync(currentDb, columns, database.SystemName, dbContext.NamingConvention);
                }

                await currentDb.SaveAsync(cancellationToken);
            }

            return currentDb;
        }

        private async Task AddColumnsAsync(
            MixDbTableViewModel currentDb,
            List<MixDbColumn> columns,
            string databaseSystemName,
            MixDatabaseNamingConvention namingConvention)
        {
            var cols = columns.Where(c => c.MixDbTableName == databaseSystemName).ToList();
            foreach (var col in cols)
            {
                string newColName = namingConvention == MixDatabaseNamingConvention.SnakeCase
                    ? col.DisplayName.ToColumnName(false)
                    : col.DisplayName.ToColumnName(true);
                col.Id = 0;
                col.SystemName = newColName;
                col.MixDbTableName = currentDb.SystemName;
                currentDb.Columns.Add(new(col, _cmsUow));
            }
        }

        private async Task<MixDbDatabase> GetOrCreateMasterDbContextAsync()
        {
            var masterDbContext = _cmsUow.DbContext.MixDbDatabase.FirstOrDefault(
                m => m.SystemName == "master");

            if (masterDbContext is null)
            {
                masterDbContext = new MixDbDatabase()
                {
                    SystemName = "master",
                    Schema = "mix",
                    ConnectionString = _databaseService.GetConnectionString(MixConstants.CONST_CMS_CONNECTION).Encrypt(_configuration.AesKey()),
                    DatabaseProvider = _databaseService.DatabaseProvider,
                    AesKey = _configuration.AesKey(),
                    CreatedDateTime = DateTime.UtcNow,
                    NamingConvention = MixDatabaseNamingConvention.SnakeCase,
                    TenantId = CurrentTenant?.Id ?? 1,
                    DisplayName = "Master"
                };
                await _cmsUow.DbContext.AddAsync(masterDbContext);
                await _cmsUow.DbContext.SaveChangesAsync();
            }

            return masterDbContext;
        }

        private async Task ProcessSystemDatabasesAsync(
            List<MixDbTable> databases,
            List<MixDbColumn>? columns,
            MixDbDatabase masterDbContext,
            CancellationToken cancellationToken)
        {
            foreach (var database in databases)
            {
                if (!_cmsUow.DbContext.MixDbTable.Any(m => m.SystemName == database.SystemName))
                {
                    var currentDb = new MixDbTableViewModel(database, _cmsUow)
                    {
                        Id = 0,
                        MixDbDatabaseId = masterDbContext.Id,
                        TenantId = CurrentTenant?.Id ?? 1,
                        CreatedDateTime = DateTime.UtcNow,
                        Columns = new()
                    };

                    if (columns is not null)
                    {
                        await AddColumnsAsync(currentDb, columns, database.SystemName, masterDbContext.NamingConvention);
                    }

                    await currentDb.SaveAsync(cancellationToken);

                    if (currentDb is { Columns.Count: > 0 })
                    {
                        await _dbStructureSrv.Migrate(currentDb, _databaseService.DatabaseProvider, cancellationToken);
                    }
                }
            }
        }

        #endregion

        #region Public Methods

        public async Task<MixDbTableViewModel> GetMixDbTable(string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentException("Table name cannot be null or empty", nameof(tableName));
            }

            string name = $"{typeof(MixDbTableViewModel).FullName}_{tableName}";
            var result = await _memoryCache.TryGetValueAsync(
                name,
                cache =>
                {
                    cache.SlidingExpiration = TimeSpan.FromSeconds(20);
                    return MixDbTableViewModel.GetRepository(_cmsUow, CacheService)
                        .GetSingleAsync(m => m.SystemName == tableName);
                });

            if (result == null)
            {
                throw new MixException(MixErrorStatus.Badrequest, $"Invalid table name: {tableName}");
            }

            string cnn = result.MixDbDatabase?.ConnectionString.Decrypt(_configuration.AesKey())
                ?? _databaseService.GetConnectionString(MixConstants.CONST_CMS_CONNECTION);
            InitDbStructureService(cnn, result.DatabaseProvider);
            return result;
        }

        public void InitDbStructureService(string cnn, MixDatabaseProvider databaseProvider)
        {
            if (string.IsNullOrEmpty(cnn))
            {
                throw new ArgumentException("Connection string cannot be null or empty", nameof(cnn));
            }

            _dbStructureSrv = databaseProvider switch
            {
                MixDatabaseProvider.SCYLLADB => _dbStructureServices.First(m => m.DbProvider == MixDatabaseProvider.SCYLLADB),
                _ => _dbStructureServices.First(m => m.DbProvider != MixDatabaseProvider.SCYLLADB)
            };
            _dbStructureSrv.Init(cnn, databaseProvider);
        }

        public async Task ExecuteCommand(string commandText, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(commandText))
            {
                throw new ArgumentException("Command text cannot be null or empty", nameof(commandText));
            }

            cancellationToken.ThrowIfCancellationRequested();
            await _dbStructureSrv.ExecuteCommand(commandText);
        }

        public async Task MigrateDatabase(string databaseName, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(databaseName))
            {
                throw new ArgumentException("Database name cannot be null or empty", nameof(databaseName));
            }

            cancellationToken.ThrowIfCancellationRequested();
            var db = await GetMixDbTable(databaseName);
            if (db is null)
            {
                throw new NullReferenceException($"Database not found: {databaseName}");
            }
            await _dbStructureSrv.Migrate(db, db.DatabaseProvider, cancellationToken);
        }

        public async Task AlterColumn(MixdbColumnViewModel col, bool isDrop, CancellationToken cancellationToken = default)
        {
            if (col == null)
            {
                throw new ArgumentNullException(nameof(col));
            }

            cancellationToken.ThrowIfCancellationRequested();
            var db = await GetMixDbTable(col.MixDbTableName);
            if (db is null)
            {
                throw new NullReferenceException($"Database not found: {col.MixDbTableName}");
            }
            await _dbStructureSrv.AlterColumn(db, col, isDrop, cancellationToken);
        }

        public async Task AddColumn(MixdbColumnViewModel repoCol, CancellationToken cancellationToken = default)
        {
            if (repoCol == null)
            {
                throw new ArgumentNullException(nameof(repoCol));
            }

            cancellationToken.ThrowIfCancellationRequested();
            var db = await GetMixDbTable(repoCol.MixDbTableName);
            if (db is null)
            {
                throw new NullReferenceException($"Database not found: {repoCol.MixDbTableName}");
            }
            await _dbStructureSrv.AddColumn(db, repoCol, cancellationToken);
        }

        public async Task DropColumn(MixdbColumnViewModel repoCol, CancellationToken cancellationToken = default)
        {
            if (repoCol == null)
            {
                throw new ArgumentNullException(nameof(repoCol));
            }

            cancellationToken.ThrowIfCancellationRequested();
            var db = await GetMixDbTable(repoCol.MixDbTableName);
            if (db is null)
            {
                throw new NullReferenceException($"Database not found: {repoCol.MixDbTableName}");
            }
            await _dbStructureSrv.DropColumn(db, repoCol, cancellationToken);
        }

        #endregion
    }
}
