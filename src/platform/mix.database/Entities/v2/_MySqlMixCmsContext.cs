using Mix.Database.Entities.Cms.v2;
using Mix.Database.Services;
using Mix.Shared.Services;

namespace Mix.Database.Entities.v2
{
    public class MySqlMixCmsContext : MixCmsContext
    {
        public MySqlMixCmsContext(MixDatabaseService databaseService, MixAppSettingService appSettingService) : base(databaseService, appSettingService)
        {
        }
    }
}
