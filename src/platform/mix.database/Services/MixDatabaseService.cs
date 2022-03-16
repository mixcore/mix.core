using Microsoft.EntityFrameworkCore;
using Mix.Database.Entities;
using Mix.Database.Entities.Account;
using Mix.Database.Entities.Cms;
using Mix.Heart.Services;
using Mix.Shared.Constants;
using Mix.Shared.Models;
using Mix.Shared.Services;

namespace Mix.Database.Services
{
    public class MixDatabaseService : ConfigurationServiceBase<DatabaseConfigurations>
    {
        public MixDatabaseProvider DatabaseProvider => AppSettings.DatabaseProvider;

        public MixDatabaseService()
            : base(MixAppConfigFilePaths.Database, false)
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

        public void InitMixCmsContext(string connectionString,
            MixDatabaseProvider databaseProvider,
            string defaultCulture)
        {
            SetConnectionString(MixConstants.CONST_CMS_CONNECTION, connectionString);
            // TODO: Seperate Account db. Current Store account to same database
            SetConnectionString(MixConstants.CONST_ACCOUNT_CONNECTION, connectionString);
            AppSettings.DatabaseProvider = databaseProvider;
            //MixAppSettingService.Instance.SetConfig<string>(MixAppSettingsSection.MixConfigurations, WebConfiguration.MixCacheConnectionString, model.ConnectionString);
            //MixAppSettingService.Instance.SetConfig<string>(MixAppSettingsSection.GlobalSettings, WebConfiguration.MixCacheDbProvider, model.DatabaseProvider.ToString());
            SaveSettings();
        }

        public void InitMixCmsContext()
        {
            using var ctx = GetDbContext();
            ctx.Database.Migrate();
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
    }
}
