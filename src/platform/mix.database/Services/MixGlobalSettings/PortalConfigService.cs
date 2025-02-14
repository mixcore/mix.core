using Microsoft.Extensions.Configuration;
using Mix.Database.Entities.Settings;
using Mix.Shared.Services;

namespace Mix.Database.Services.MixGlobalSettings
{
    public class PortalConfigService : GlobalSettingServiceBase
    {
        public PortalConfigService(IConfiguration configuration, MixGlobalSetting settings) : base(configuration, settings)
        {
        }
    }
}
