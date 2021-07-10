using Microsoft.EntityFrameworkCore;
using Mix.Database.Entities.Account;
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
    public class MixDatabaseService
    {
        public MixAppSettingService _appSettingService;
        public MixDatabaseService()
        {
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
            return _appSettingService.DatabaseProvider switch
            {
                MixDatabaseProvider.MSSQL => new SqlServerMixCmsContext(this, _appSettingService),
                MixDatabaseProvider.MySQL => new MySqlMixCmsContext(this, _appSettingService),
                MixDatabaseProvider.SQLITE => new SqliteMixCmsContext(this, _appSettingService),
                MixDatabaseProvider.PostgreSQL => new PostgresqlMixCmsContext(this, _appSettingService),
                _ => null,
            };
        }
        public MixCmsAccountContext GetAccountDbContext()
        {
            return _appSettingService.DatabaseProvider switch
            {
                MixDatabaseProvider.MSSQL or MixDatabaseProvider.MySQL or MixDatabaseProvider.SQLITE => new SQLAccountContext(this, _appSettingService),
                MixDatabaseProvider.PostgreSQL => new PostgresSQLAccountContext(this, _appSettingService),
                _ => null,
            };
        }

        public void InitMixCmsContext(string connectionString,
            MixDatabaseProvider databaseProvider,
            string defaultCulture)
        {
            SetConnectionString(MixConstants.CONST_CMS_CONNECTION, connectionString);
            _appSettingService.SetConfig(
                MixAppSettingsSection.GlobalSettings, MixConstants.CONST_SETTING_DATABASE_PROVIDER, databaseProvider.ToString());
            _appSettingService.SetConfig(MixAppSettingsSection.GlobalSettings, MixConstants.CONST_SETTING_LANGUAGE, defaultCulture);
            //MixAppSettingService.Instance.SetConfig<string>(MixAppSettingsSection.MixConfigurations, WebConfiguration.MixCacheConnectionString, model.ConnectionString);
            //MixAppSettingService.Instance.SetConfig<string>(MixAppSettingsSection.GlobalSettings, WebConfiguration.MixCacheDbProvider, model.DatabaseProvider.ToString());
            _appSettingService.SaveSettings();
            _appSettingService.Reload();
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
                var query = ctx.MixConfigurationContent.Where(c => c.ParentId == 1).ToQueryString();
                Console.WriteLine(query);
            }
        }

    }
}
