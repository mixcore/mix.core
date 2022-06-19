using Mix.Database.Entities;
using Mix.Database.Entities.Account;
using Mix.Database.Entities.Quartz;
using Mix.Heart.Constants;
using Mix.Heart.Entities.Cache;
using Mix.Heart.Services;
using Mix.Shared.Models;
using Mix.Shared.Services;
using System.Threading.Tasks;

namespace Mix.Database.Services
{
    public class MixDatabaseService : ConfigurationServiceBase<DatabaseConfigurations>
    {
        public MixDatabaseProvider DatabaseProvider => AppSettings.DatabaseProvider;

        public MixDatabaseService()
            : base(MixAppConfigFilePaths.Database, true)
        {
            AesKey = GlobalConfigService.Instance.AppSettings.ApiEncryptKey;
        }

        protected override void LoadAppSettings()
        {
            AesKey = GlobalConfigService.Instance.AppSettings.ApiEncryptKey;
            base.LoadAppSettings();
        }

        public string GetConnectionString(string name)
        {
            switch (name)
            {
                case MixConstants.CONST_CMS_CONNECTION:
                    return AppSettings.ConnectionStrings?.MixCmsConnection;
                case MixConstants.CONST_ACCOUNT_CONNECTION:
                    return AppSettings.ConnectionStrings?.MixAccountConnection;
                default:
                    return string.Empty;
            }
        }

        public void SetConnectionString(string name, string value)
        {
            switch (name)
            {
                case MixConstants.CONST_CMS_CONNECTION:
                    AppSettings.ConnectionStrings.MixCmsConnection = value;
                    break;
                case MixConstants.CONST_ACCOUNT_CONNECTION:
                    AppSettings.ConnectionStrings.MixAccountConnection = value;
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
                MixDatabaseProvider.SQLSERVER => new SQLServerQuartzDbContext(this),
                MixDatabaseProvider.MySQL => new MySQLQuartzDbContext(this),
                MixDatabaseProvider.SQLITE => new SQLiteQuartzDbContext(this),
                MixDatabaseProvider.PostgreSQL => new PostgresSQLQuartzDbContext(this),
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
            MixDatabaseProvider databaseProvider,
            string defaultCulture)
        {
            SetConnectionString(MixConstants.CONST_CMS_CONNECTION, connectionString);
            // TODO: Seperate Account db. Current Store account to same database
            if (databaseProvider == MixDatabaseProvider.SQLITE)
            {
                SetConnectionString(MixConstants.CONST_ACCOUNT_CONNECTION, connectionString.Replace(".db", "") + "-account.db");
            }
            else
            {
                SetConnectionString(MixConstants.CONST_ACCOUNT_CONNECTION, connectionString);
            }

            AppSettings.DatabaseProvider = databaseProvider;
            MixHeartConfigService.Instance.AppSettings.DatabaseProvider = databaseProvider;
            MixHeartConfigService.Instance.SetConnectionString(MixHeartConstants.CACHE_CONNECTION, connectionString);
            //MixAppSettingService.Instance.SetConfig<string>(MixAppSettingsSection.MixConfigurations, WebConfiguration.MixCacheConnectionString, model.ConnectionString);
            //MixAppSettingService.Instance.SetConfig<string>(MixAppSettingsSection.GlobalSettings, WebConfiguration.MixCacheDbProvider, model.DatabaseProvider.ToString());
            SaveSettings();
        }

        public async Task UpdateMixCmsContextAsync()
        {
            using var ctx = GetDbContext();
            await ctx.Database.MigrateAsync();
            using var cacheCtx = GetCacheDbContext();
            await cacheCtx.Database.MigrateAsync();
            using var accCtx = GetAccountDbContext();
            await accCtx.Database.MigrateAsync();
            //var transaction = ctx.Database.BeginTransaction();
            //var sysDatabasesFile = MixFileRepository.Instance.GetFile("sys_databases", MixFileExtensions.Json, $"{MixFolders.JsonDataFolder}");
            //var sysDatabases = JObject.Parse(sysDatabasesFile.Content)["data"].ToObject<List<MixDatabaseViewModel>>();
            //foreach (var db in sysDatabases)
            //{
            //    if (!ctx.MixDatabase.Any(m => m.Name == db.Name))
            //    {
            //        db.SaveModel(true, ctx, transaction);
            //    }
            //}
            //transaction.Commit();
            //transaction.Dispose();
            //var query = ctx.MixConfigurationContent.Where(c => c.ParentId == 1).ToQueryString();
            //Console.WriteLine(query);
        }

        public async Task InitQuartzContextAsync()
        {
            using var ctx = GetQuartzDbContext();
            await ctx.Database.MigrateAsync();
        }
    }
}
