using Microsoft.AspNetCore.Http;
using Mix.Database.Entities.Account;
using Mix.Database.Entities.MixDb;
using Mix.Database.Entities.Quartz;
using Mix.Heart.Constants;
using Mix.Heart.Entities.Cache;
using Mix.Heart.Services;
using Mix.Shared.Models.Configurations;
using Mix.Shared.Services;
using System.Threading.Tasks;

namespace Mix.Database.Services
{
    public class DatabaseService : ConfigurationServiceBase<DatabaseConfigurations>
    {
        public MixDatabaseProvider DatabaseProvider => MixDatabaseProvider.SQLITE; //AppSettings.DatabaseProvider;
        protected IHttpContextAccessor HttpContextAccessor;
        public DatabaseService(IHttpContextAccessor httpContextAccessor) : base(MixAppConfigFilePaths.Database, true)
        {
            HttpContextAccessor = httpContextAccessor;
            AesKey = GlobalConfigService.Instance.AppSettings.ApiEncryptKey;
        }

        protected override void LoadAppSettings()
        {
            AesKey = GlobalConfigService.Instance.AppSettings.ApiEncryptKey;
            base.LoadAppSettings();
        }

        public string GetConnectionString(string name)
        {
            return "Data Source=MixContent\\mix-cms.sqlite";
            switch (name)
            {
                case MixConstants.CONST_CMS_CONNECTION:
                    return AppSettings.ConnectionStrings?.MixCmsConnection;
                case MixConstants.CONST_ACCOUNT_CONNECTION:
                    return AppSettings.ConnectionStrings?.MixAccountConnection;
                case MixConstants.CONST_MIXDB_CONNECTION:
                    return AppSettings.ConnectionStrings?.MixDbConnection;
                case MixConstants.CONST_QUARTZ_CONNECTION:
                    return AppSettings.ConnectionStrings?.MixQuartzConnection;
                default:
                    return string.Empty;
            }
        }

        public void SetConnectionString(string name, string value)
        {
            switch (name)
            {
                case MixConstants.CONST_QUARTZ_CONNECTION:
                    AppSettings.ConnectionStrings.MixQuartzConnection = value;
                    break;
                case MixConstants.CONST_CMS_CONNECTION:
                    AppSettings.ConnectionStrings.MixCmsConnection = value;
                    break;
                case MixConstants.CONST_ACCOUNT_CONNECTION:
                    AppSettings.ConnectionStrings.MixAccountConnection = value;
                    break;
                case MixConstants.CONST_MIXDB_CONNECTION:
                    AppSettings.ConnectionStrings.MixDbConnection = value;
                    break;
                default:
                    break;
            }
            SaveSettings();
        }



        public MixCmsContext GetDbContext()
        {
            return DatabaseProvider switch
            {
                MixDatabaseProvider.SQLSERVER => new SqlServerMixCmsContext(this),
                MixDatabaseProvider.MySQL => new MySqlMixCmsContext(this),
                MixDatabaseProvider.SQLITE => new SqliteMixCmsContext(this),
                MixDatabaseProvider.PostgreSQL => new PostgresqlMixCmsContext(this),
                _ => null,
            };
        }

        public MixDbDbContext GetMixDbDbContext()
        {
            return DatabaseProvider switch
            {
                MixDatabaseProvider.SQLSERVER => new SqlServerMixDbDbContext(this),
                MixDatabaseProvider.MySQL => new MySqlMixDbDbContext(this),
                MixDatabaseProvider.SQLITE => new SqliteMixDbDbContext(this),
                MixDatabaseProvider.PostgreSQL => new PostgresSqlMixDbDbContext(this),
                _ => null,
            };
        }

        public MixCmsContext GetMixcmsContext()
        {
            return DatabaseProvider switch
            {
                MixDatabaseProvider.SQLSERVER => new(this),
                MixDatabaseProvider.MySQL => new MySqlMixCmsContext(this),
                MixDatabaseProvider.SQLITE => new SqliteMixCmsContext(this),
                MixDatabaseProvider.PostgreSQL => new PostgresqlMixCmsContext(this),
                _ => null,
            };
        }

        public MixCmsAccountContext GetAccountDbContext()
        {
            return DatabaseProvider switch
            {
                MixDatabaseProvider.SQLSERVER => new SqlServerAccountContext(this),
                MixDatabaseProvider.MySQL => new MySqlAccountContext(this),
                MixDatabaseProvider.SQLITE => new SqliteAccountContext(this),
                MixDatabaseProvider.PostgreSQL => new PostgresSQLAccountContext(this),
                _ => null,
            };
        }

        public QuartzDbContext GetQuartzDbContext()
        {
            return DatabaseProvider switch
            {
                MixDatabaseProvider.SQLSERVER => new SQLServerQuartzDbContext(HttpContextAccessor, this),
                MixDatabaseProvider.MySQL => new MySQLQuartzDbContext(HttpContextAccessor, this),
                MixDatabaseProvider.SQLITE => new SQLiteQuartzDbContext(HttpContextAccessor, this),
                MixDatabaseProvider.PostgreSQL => new PostgresSQLQuartzDbContext(HttpContextAccessor, this),
                _ => null,
            };
        }

        public MixCacheDbContext GetCacheDbContext()
        {
            return DatabaseProvider switch
            {
                MixDatabaseProvider.SQLSERVER => new MsSqlCacheDbContext(),
                MixDatabaseProvider.MySQL => new MySqlCacheDbContext(),
                MixDatabaseProvider.SQLITE => new SqliteCacheDbContext(),
                MixDatabaseProvider.PostgreSQL => new PostgresCacheDbContext(),
                _ => null,
            };
        }

        public void InitConnectionStrings(string connectionString,
            MixDatabaseProvider databaseProvider)
        {
            SetConnectionString(MixConstants.CONST_CMS_CONNECTION, connectionString);
            // TODO: Seperate Account db. Current Store account to same database
            if (databaseProvider == MixDatabaseProvider.SQLITE)
            {
                SetConnectionString(MixConstants.CONST_QUARTZ_CONNECTION, connectionString.Replace(".sqlite", "") + "-quartz.sqlite");
                SetConnectionString(MixConstants.CONST_ACCOUNT_CONNECTION, connectionString.Replace(".sqlite", "") + "-account.sqlite");
                SetConnectionString(MixConstants.CONST_MIXDB_CONNECTION, connectionString.Replace(".sqlite", "") + "-mixdb.sqlite");
            }
            else
            {
                SetConnectionString(MixConstants.CONST_QUARTZ_CONNECTION, connectionString);
                SetConnectionString(MixConstants.CONST_ACCOUNT_CONNECTION, connectionString);
                SetConnectionString(MixConstants.CONST_MIXDB_CONNECTION, connectionString);
            }

            AppSettings.DatabaseProvider = databaseProvider;
            MixHeartConfigService.Instance.AppSettings.DatabaseProvider = databaseProvider;
            MixHeartConfigService.Instance.SetConnectionString(MixHeartConstants.CACHE_CONNECTION, connectionString);
            SaveSettings();
        }

        public void ResetConnectionStrings()
        {
            SetConnectionString(MixConstants.CONST_CMS_CONNECTION, null);
            SetConnectionString(MixConstants.CONST_QUARTZ_CONNECTION, null);
            SetConnectionString(MixConstants.CONST_ACCOUNT_CONNECTION, null);
            SetConnectionString(MixConstants.CONST_MIXDB_CONNECTION, null);
            MixHeartConfigService.Instance.SetConnectionString(MixHeartConstants.CACHE_CONNECTION, null);
            SaveSettings();
        }
        public void UpdateMixCmsContext()
        {
            using var ctx = GetDbContext();
            ctx.Database.Migrate();
            using var cacheCtx = GetCacheDbContext();
            cacheCtx.Database.Migrate();
            using var accCtx = GetAccountDbContext();
            accCtx.Database.Migrate();
            using var mixdbCtx = GetMixDbDbContext();
            mixdbCtx.Database.Migrate();
        }

        public async Task InitQuartzContextAsync()
        {
            using var ctx = GetQuartzDbContext();
            await ctx.Database.MigrateAsync();
        }
    }
}
