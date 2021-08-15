using Microsoft.EntityFrameworkCore;
using Mix.Database.Entities.Account;
using Mix.Database.Entities.Cms;
using Mix.Database.Entities.v2;
using Mix.Heart.Enums;
using Mix.Shared.Constants;
using Mix.Shared.Enums;
using Mix.Shared.Services;
using System;
using System.Linq;

namespace Mix.Database.Services
{
    public class MixDatabaseService
    {
        public GlobalConfigService _globalConfigService;
        public MixDatabaseService(GlobalConfigService globalConfigService)
        {
            _globalConfigService = globalConfigService;
        }

        public string GetConnectionString(string connectionName)
        {
            return _globalConfigService.GetConnectionString(connectionName);
        }
        
        public void SetConnectionString(string connectionName, string connection)
        {
            _globalConfigService.SetConnectionString(connectionName, connection);
        }

        public MixCmsContext GetDbContext()
        {
            return _globalConfigService.DatabaseProvider switch
            {
                MixDatabaseProvider.SQLSERVER => new SqlServerMixCmsContext(this, _globalConfigService),
                MixDatabaseProvider.MySQL => new MySqlMixCmsContext(this, _globalConfigService),
                MixDatabaseProvider.SQLITE => new SqliteMixCmsContext(this, _globalConfigService),
                MixDatabaseProvider.PostgreSQL => new PostgresqlMixCmsContext(this, _globalConfigService),
                _ => null,
            };
        }
        public MixCmsAccountContext GetAccountDbContext()
        {
            return _globalConfigService.DatabaseProvider switch
            {
                MixDatabaseProvider.SQLSERVER or MixDatabaseProvider.MySQL or MixDatabaseProvider.SQLITE => new SQLAccountContext(this, _globalConfigService),
                MixDatabaseProvider.PostgreSQL => new PostgresSQLAccountContext(this, _globalConfigService),
                _ => null,
            };
        }

        public void InitMixCmsContext(string connectionString,
            MixDatabaseProvider databaseProvider,
            string defaultCulture)
        {
            SetConnectionString(MixConstants.CONST_CMS_CONNECTION, connectionString);
            _globalConfigService.SetConfig(MixConstants.CONST_SETTING_DATABASE_PROVIDER, databaseProvider.ToString());
            _globalConfigService.SetConfig(MixConstants.CONST_SETTING_LANGUAGE, defaultCulture);
            //MixAppSettingService.Instance.SetConfig<string>(MixAppSettingsSection.MixConfigurations, WebConfiguration.MixCacheConnectionString, model.ConnectionString);
            //MixAppSettingService.Instance.SetConfig<string>(MixAppSettingsSection.GlobalSettings, WebConfiguration.MixCacheDbProvider, model.DatabaseProvider.ToString());
            _globalConfigService.SaveSettings();
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
 