using Mix.Database.Entities.Account;
using Mix.Database.Entities.Cms.v2;
using Mix.Database.Entities.v2;
using Mix.Heart.Enums;
using Mix.Shared.Constants;
using Mix.Shared.Enums;
using Mix.Shared.Services;
using System;

namespace Mix.Database.Services
{
    public class MixDatabaseService
    {
        public static MixCmsContextV2 GetDbContext()
        {
            var provider = MixAppSettingService.GetEnumConfig<MixDatabaseProvider>(MixAppSettingsSection.GlobalSettings, MixConstants.CONST_SETTING_DATABASE_PROVIDER);
            return provider switch
            {
                MixDatabaseProvider.MSSQL => new MsSqlMixCmsContext(),
                MixDatabaseProvider.MySQL => new MySqlMixCmsContext(),
                MixDatabaseProvider.SQLITE => new SqliteMixCmsContext(),
                MixDatabaseProvider.PostgreSQL => new PostgresqlMixCmsContext(),
                _ => null,
            };
        }
        public static MixCmsAccountContext GetAccountDbContext()
        {
            var provider = MixAppSettingService.GetEnumConfig<MixDatabaseProvider>(MixAppSettingsSection.GlobalSettings, MixConstants.CONST_SETTING_DATABASE_PROVIDER);
            return provider switch
            {
                MixDatabaseProvider.MSSQL or MixDatabaseProvider.MySQL or MixDatabaseProvider.SQLITE => new SQLAccountContext(),
                MixDatabaseProvider.PostgreSQL => new PostgresSQLAccountContext(),
                _ => null,
            };
        }

        public static void InitMixCmsContext()
        {
            throw new NotImplementedException();
            //using (var ctx = MixDatabaseService.GetDbContext())
            //{
            //    ctx.Database.Migrate();
            //    var transaction = ctx.Database.BeginTransaction();
            //    var sysDatabasesFile = MixFileRepository.Instance.GetFile("sys_databases", MixFileExtensions.Json, $"{MixFolders.JsonDataFolder}");
            //    var sysDatabases = JObject.Parse(sysDatabasesFile.Content)["data"].ToObject<List<MixDatabaseViewModel>>();
            //    foreach (var db in sysDatabases)
            //    {
            //        if (!ctx.MixDatabase.Any(m => m.Name == db.Name))
            //        {
            //            db.SaveModel(true, ctx, transaction);
            //        }
            //    }
            //    transaction.Commit();
            //    transaction.Dispose();
            //}
        }
    }
}
