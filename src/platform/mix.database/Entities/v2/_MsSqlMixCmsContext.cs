using Mix.Database.Entities.Cms.v2;
using Mix.Database.Services;
using Mix.Shared.Services;

namespace Mix.Database.Entities.v2
{
    public class MsSqlMixCmsContext : MixCmsContext
    {
        public MsSqlMixCmsContext(MixDatabaseService databaseService, MixAppSettingService appSettingService) : base(databaseService, appSettingService)
        {
        }
    }
}
