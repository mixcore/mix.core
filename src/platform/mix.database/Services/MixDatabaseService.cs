using Microsoft.EntityFrameworkCore;
using Mix.Database.Entities.Cms.v2;
using Mix.Database.Entities.v2;
using Mix.Heart.Enums;
using Mix.Shared.Constants;
using Mix.Shared.Enums;
using Mix.Shared.Services;
using System;
using System.Linq;

namespace Mix.Database.Services
{
    public class MixDatabaseService : SingletonService<MixDatabaseService>
    {
        public MixAppSettingService _appSettingService;
        public MixDatabaseService()
        {
            _appSettingService = MixAppSettingService.Instance;
        }
        public MixDatabaseService(MixAppSettingService appSettingService)
        {
            _appSettingService = appSettingService;
        }

        public string GetConnectionString(string connectionName)
        {
            return _appSettingService.GetConfig<string>(MixAppSettingsSection.ConnectionStrings, connectionName);
        }
        
        public void SetConnectionString(string connectionName, string connection)
        {
            _appSettingService.SetConfig(MixAppSettingsSection.ConnectionStrings, connectionName, connection);
        }

        public MixCmsContext GetDbContext()
        {
            var provider = _appSettingService.GetEnumConfig<MixDatabaseProvider>(MixAppSettingsSection.GlobalSettings, MixConstants.CONST_SETTING_DATABASE_PROVIDER);
            return provider switch
            {
                MixDatabaseProvider.MSSQL => new MsSqlMixCmsContext(),
                MixDatabaseProvider.MySQL => new MySqlMixCmsContext(),
                MixDatabaseProvider.SQLITE => new SqliteMixCmsContext(),
                MixDatabaseProvider.PostgreSQL => new PostgresqlMixCmsContext(),
                _ => null,
            };
        }
        //public MixCmsAccountContext GetAccountDbContext()
        //{
        //    var provider = _appSettingService.GetEnumConfig<MixDatabaseProvider>(MixAppSettingsSection.GlobalSettings, MixConstants.CONST_SETTING_DATABASE_PROVIDER);
        //    return provider switch
        //    {
        //        MixDatabaseProvider.MSSQL or MixDatabaseProvider.MySQL or MixDatabaseProvider.SQLITE => new SQLAccountContext(),
        //        MixDatabaseProvider.PostgreSQL => new PostgresSQLAccountContext(),
        //        _ => null,
        //    };
        //}

        public void InitMixCmsContext(string connectionString,
            MixDatabaseProvider databaseProvider,
            string defaultCulture)
        {
            Instance.SetConnectionString(MixConstants.CONST_CMS_CONNECTION, connectionString);
            MixAppSettingService.Instance.SetConfig(
                MixAppSettingsSection.GlobalSettings, MixConstants.CONST_SETTING_DATABASE_PROVIDER, databaseProvider.ToString());
            MixAppSettingService.Instance.SetConfig(MixAppSettingsSection.GlobalSettings, MixConstants.CONST_SETTING_LANGUAGE, defaultCulture);
            //MixAppSettingService.Instance.SetConfig<string>(MixAppSettingsSection.MixConfigurations, WebConfiguration.MixCacheConnectionString, model.ConnectionString);
            //MixAppSettingService.Instance.SetConfig<string>(MixAppSettingsSection.GlobalSettings, WebConfiguration.MixCacheDbProvider, model.DatabaseProvider.ToString());
            MixAppSettingService.Instance.SaveSettings();
            MixAppSettingService.Instance.Reload();
        }

        public void InitMixCmsContext()
        {
            using (var ctx = GetDbContext())
            {
                ctx.Database.Migrate();
                var transaction = ctx.Database.BeginTransaction();
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
                var query = ctx.MixConfigurationContent.Where(c => c.MixConfigurationId == 1).ToQueryString();
                Console.WriteLine(query);
            }
        }

    }
}
