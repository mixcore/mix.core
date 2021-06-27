using Mix.Database.Entities.Cms.v2;
using Mix.Database.Services;
using Mix.Shared.Services;

namespace Mix.Database.Entities.v2
{
    public class PostgresqlMixCmsContext : MixCmsContext
    {
        public PostgresqlMixCmsContext(MixDatabaseService databaseService, MixAppSettingService appSettingService) : base(databaseService, appSettingService)
        {
        }
    }
}
