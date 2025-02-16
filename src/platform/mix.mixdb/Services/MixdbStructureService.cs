using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
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
using Mix.Shared.Services;
using MySqlX.XDevAPI.Common;
using Newtonsoft.Json.Linq;
using RepoDb.Interfaces;
using System.Dynamic;

namespace Mix.Mixdb.Services
{
    public class MixdbStructureService : TenantServiceBase, IMixdbStructure
    {
        #region Properties
        IEnumerable<IMixdbStructureService> _dbStructureServices;
        protected IMixdbStructureService _dbStructureSrv { get; set; }
        protected IDatabaseConstants _databaseConstant;
        protected MixDatabaseProvider _databaseProvider;
        protected IMixMemoryCacheService _memoryCache;
        private readonly IConfiguration _configuration;
        protected readonly UnitOfWorkInfo<MixCmsContext> _cmsUow;
        protected readonly DatabaseService _databaseService;
        private readonly ICache _cache;
        private RepoDbRepository _repository;
        private AppSetting _settings;
        protected RuntimeDbContextService _runtimeDbContextService;

        #endregion

        #region Constructors

        public MixdbStructureService(
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            UnitOfWorkInfo<MixCmsContext> uow,
            DatabaseService databaseService,
            IMixMemoryCacheService memoryCache,
            MixCacheService cacheService,
            IMixTenantService mixTenantService,
            ICache cache,
            IEnumerable<IMixdbStructureService> dbStructureServices) : base(httpContextAccessor, cacheService, mixTenantService)
        {
            _configuration = configuration;
            _cmsUow = uow;
            _databaseService = databaseService;
            _databaseProvider = _databaseService.DatabaseProvider;
            _databaseConstant = _databaseProvider switch
            {
                MixDatabaseProvider.SQLSERVER => new SqlServerDatabaseConstants(),
                MixDatabaseProvider.MySQL => new MySqlDatabaseConstants(),
                MixDatabaseProvider.PostgreSQL => new PostgresDatabaseConstants(),
                MixDatabaseProvider.SQLITE => new SqliteDatabaseConstants(),
                MixDatabaseProvider.SCYLLADB => new CassandraDatabaseConstants(),
                _ => throw new NotImplementedException()
            };
            _memoryCache = memoryCache;
            _runtimeDbContextService = new RuntimeDbContextService(httpContextAccessor, databaseService);
            _cache = cache;
            _settings = new AppSetting()
            {
                CacheItemExpiration = 10,
                CommandTimeout = 1000
            };
            _repository = new(
               databaseService.GetConnectionString(MixConstants.CONST_MIXDB_CONNECTION),
               databaseService.DatabaseProvider,
               _settings);
            _dbStructureServices = dbStructureServices;
        }

        #endregion


        public async Task MigrateInitNewDbContextDatabases(MixDatabaseContext dbContext, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                var cnn = dbContext.ConnectionString;
                if (AesEncryptionHelper.IsEncrypted(dbContext.ConnectionString, _configuration.AesKey()))
                {
                    cnn = AesEncryptionHelper.DecryptString(dbContext.ConnectionString, _configuration.AesKey());
                }
                InitDbStructureService(cnn, dbContext.DatabaseProvider);
                if (dbContext.DatabaseProvider == MixDatabaseProvider.PostgreSQL)
                {
                    await _dbStructureSrv.ExecuteCommand("CREATE EXTENSION IF NOT EXISTS \"unaccent\";");
                }
                var strMixDbs = MixFileHelper.GetFile(
                    "init-new-dbcontext-databases", MixFileExtensions.Json, MixFolders.JsonDataFolder);
                var obj = JObject.Parse(strMixDbs.Content);
                var databases = obj.Value<JArray>("databases")?.ToObject<List<MixDatabase>>();
                var columns = obj.Value<JArray>("columns")?.ToObject<List<MixDatabaseColumn>>();
                if (databases != null)
                {
                    foreach (var database in databases)
                    {
                        string newDbName = dbContext.NamingConvention == MixDatabaseNamingConvention.SnakeCase
                            ? $"{dbContext.SystemName}_{database.DisplayName.ToColumnName(false)}"
                            : $"{dbContext.SystemName.ToTitleCase()}{database.DisplayName.ToColumnName(true)}";
                        var currentDb = await MixDbDatabaseViewModel.GetRepository(_cmsUow, CacheService)
                            .GetSingleAsync(m => m.SystemName == newDbName);
                        if (currentDb == null)
                        {
                            currentDb = new(database, _cmsUow);
                            currentDb.Id = 0;
                            currentDb.NamingConvention = dbContext.NamingConvention;
                            currentDb.SystemName = newDbName;
                            currentDb.TenantId = CurrentTenant?.Id ?? 1;
                            currentDb.MixDatabaseContextId = dbContext.Id;
                            currentDb.DatabaseProvider = dbContext.DatabaseProvider;
                            currentDb.CreatedDateTime = DateTime.UtcNow;
                            currentDb.Columns = new();

                            if (columns is not null)
                            {
                                currentDb.AddDefaultColumns();
                                var cols = columns.Where(c => c.MixDatabaseName == database.SystemName).ToList();
                                foreach (var col in cols)
                                {
                                    string newColName =
                                        dbContext.NamingConvention == MixDatabaseNamingConvention.SnakeCase
                                            ? col.DisplayName.ToColumnName(false)
                                            : col.DisplayName.ToColumnName(true);
                                    col.Id = 0;
                                    col.SystemName = newColName;
                                    col.MixDatabaseName = newDbName;
                                    currentDb.Columns.Add(new(col, _cmsUow));
                                }
                            }

                            await currentDb.SaveAsync(cancellationToken);
                        }

                        if (currentDb is { Columns.Count: > 0 })
                        {
                            await _dbStructureSrv.Migrate(currentDb, dbContext.DatabaseProvider);
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.Badrequest, ex);
            }
        }

        public async Task MigrateSystemDatabases(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var strMixDbs = MixFileHelper.GetFile(
                "system-databases", MixFileExtensions.Json, MixFolders.JsonDataFolder);
            var obj = JObject.Parse(strMixDbs.Content);
            var databases = obj.Value<JArray>("databases")?.ToObject<List<MixDatabase>>();
            var columns = obj.Value<JArray>("columns")?.ToObject<List<MixDatabaseColumn>>();
            var masterDbContext = _cmsUow.DbContext.MixDatabaseContext.FirstOrDefault(
                m => m.SystemName == "master");
            if (masterDbContext is null)
            {
                masterDbContext = new MixDatabaseContext()
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
            }
            if (databases != null)
            {
                foreach (var database in databases)
                {
                    if (!_cmsUow.DbContext.MixDatabase.Any(m => m.SystemName == database.SystemName))
                    {
                        MixDbDatabaseViewModel currentDb = new(database, _cmsUow);
                        currentDb.Id = 0;
                        currentDb.MixDatabaseContextId = masterDbContext.Id;
                        currentDb.TenantId = CurrentTenant?.Id ?? 1;
                        currentDb.CreatedDateTime = DateTime.UtcNow;
                        currentDb.Columns = new();

                        if (columns is not null)
                        {
                            var cols = columns.Where(c => c.MixDatabaseName == database.SystemName).ToList();
                            foreach (var col in cols)
                            {
                                col.Id = 0;
                                currentDb.Columns.Add(new(col, _cmsUow));
                            }
                        }

                        await currentDb.SaveAsync(cancellationToken);

                        if (currentDb is { Columns.Count: > 0 })
                        {
                            await _dbStructureSrv.Migrate(currentDb, _databaseService.DatabaseProvider);
                        }
                    }
                }

            }
        }


        #region Helpers
        protected async Task<MixDbDatabaseViewModel> GetMixDatabase(string tableName)
        {
            var result = await _memoryCache.TryGetValueAsync(
                tableName,
                async cache =>
                {
                    cache.SlidingExpiration = TimeSpan.FromSeconds(20);
                    var db = await MixDbDatabaseViewModel.GetRepository(_cmsUow, CacheService)
                        .GetSingleAsync(m => m.SystemName == tableName);
                    if (db != null)
                    {
                        db.DatabaseProvider = db.MixDatabaseContext != null
                        ? db.MixDatabaseContext.DatabaseProvider
                        : _databaseService.DatabaseProvider;
                    }
                    return db;
                }
            );
            if (result == null)
            {
                throw new MixException(MixErrorStatus.Badrequest, "Invalid table name");
            }
            string cnn = result.MixDatabaseContext?.ConnectionString.Decrypt(_configuration.AesKey()) ?? _databaseService.GetConnectionString(MixConstants.CONST_CMS_CONNECTION);
            InitDbStructureService(cnn, result.DatabaseProvider);
            return result;
        }

        public void InitDbStructureService(string cnn, MixDatabaseProvider databaseProvider)
        {
            switch (databaseProvider)
            {
                case MixDatabaseProvider.SCYLLADB:
                    _dbStructureSrv = _dbStructureServices.First(m => m.DbProvider == MixDatabaseProvider.SCYLLADB);
                    break;
                case MixDatabaseProvider.SQLSERVER:
                case MixDatabaseProvider.MySQL:
                case MixDatabaseProvider.PostgreSQL:
                case MixDatabaseProvider.SQLITE:
                default:
                    _dbStructureSrv = _dbStructureServices.First(m => m.DbProvider != MixDatabaseProvider.SCYLLADB);
                    break;
            }
            _dbStructureSrv.Init(cnn, databaseProvider);
        }

        protected string GetRelationshipDbName(MixDbDatabaseViewModel mixDb)
        {
            string relName = mixDb.NamingConvention == MixDatabaseNamingConvention.SnakeCase
                ? MixDatabaseNames.DATA_RELATIONSHIP_SNAKE_CASE
                : MixDatabaseNames.DATA_RELATIONSHIP_TITLE_CASE;
            return mixDb.MixDatabaseContextId.HasValue
                ? $"{mixDb.MixDatabaseContext.SystemName}_{relName}"
                : MixDatabaseNames.SYSTEM_DATA_RELATIONSHIP;
        }

        protected void GetMembers(ExpandoObject obj, List<string> selectMembers, FieldNameService fieldNameService)
        {
            var result = obj.ToList();
            foreach (KeyValuePair<string, object?> kvp in result)
            {
                if (fieldNameService.GetAllFieldName().All(m => m != kvp.Key) && selectMembers.All(m => m != kvp.Key))
                {
                    obj!.Remove(kvp.Key, out _);
                }
            }
        }





        #endregion

        #region Methods
        public Task ExecuteCommand(string commandText, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                return _dbStructureSrv.ExecuteCommand(commandText);
            }
            throw new TaskCanceledException();
        }
        public async Task MigrateDatabase(string databaseName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var db = await GetMixDatabase(databaseName);
            if (db is null)
            {
                throw new NullReferenceException(databaseName);
            }
            await _dbStructureSrv.Migrate(db, db.DatabaseProvider);
        }
        public Task<bool> BackupDatabase(string databaseName, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RestoreFromLocal(string databaseName, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task AlterColumn(MixdbDatabaseColumnViewModel col, bool isDrop, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var db = await GetMixDatabase(col.MixDatabaseName);
            if (db is null)
            {
                throw new NullReferenceException(col.MixDatabaseName);
            }
            await _dbStructureSrv.AlterColumn(db, col, isDrop, cancellationToken);
        }

        public async Task AddColumn(MixdbDatabaseColumnViewModel repoCol, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var db = await GetMixDatabase(repoCol.MixDatabaseName);
            if (db is null)
            {
                throw new NullReferenceException(repoCol.MixDatabaseName);
            }
            await _dbStructureSrv.AddColumn(db, repoCol, cancellationToken);
        }

        public async Task DropColumn(MixdbDatabaseColumnViewModel repoCol, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var db = await GetMixDatabase(repoCol.MixDatabaseName);
            if (db is null)
            {
                throw new NullReferenceException(repoCol.MixDatabaseName);
            }
            await _dbStructureSrv.DropColumn(db, repoCol, cancellationToken);
        }


        #endregion
    }
}
