using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Mix.Database.Entities.Account;
using Mix.Database.Entities.Cms;
using Mix.Database.Entities.v2;
using Mix.Heart.Enums;
using Mix.Heart.Exceptions;
using Mix.Shared.Constants;
using Mix.Shared.Enums;
using Mix.Shared.Models;
using Mix.Shared.Services;
using System;

namespace Mix.Database.Services
{
    public class MixDatabaseService: AppSettingServiceBase<DatabaseConfigurations>
    {
        public MixDatabaseProvider DatabaseProvider => AppSettings.DatabaseProvider;

        public MixDatabaseService(IConfiguration configuration) 
            : base(configuration, MixAppSettingsSection.Database, MixAppConfigFilePaths.Database)
        {
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
                MixDatabaseProvider.SQLSERVER or MixDatabaseProvider.MySQL or MixDatabaseProvider.SQLITE => new SQLAccountContext(this),
                MixDatabaseProvider.PostgreSQL => new PostgresSQLAccountContext(this),
                _ => null,
            };
        }

        public void InitMixCmsContext(string connectionString,
            MixDatabaseProvider databaseProvider,
            string defaultCulture)
        {
            SetConnectionString(MixConstants.CONST_CMS_CONNECTION, connectionString);
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

        protected override void BindAppSettings(IConfigurationSection settings)
        {
            try
            {
                AppSettings = new DatabaseConfigurations();
                settings.Bind(AppSettings);
            }
            catch (Exception ex)
            {
                throw new MixException($"Cannot load config section {_sectionName}: {ex.Message}");
            }
        }
    }
}
 