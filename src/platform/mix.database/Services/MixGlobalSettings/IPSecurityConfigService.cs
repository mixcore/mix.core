using Microsoft.Extensions.Configuration;
using Mix.Database.Entities.Settings;
using Mix.Shared.Services;

namespace Mix.Database.Services.MixGlobalSettings
{
    public class IPSecurityConfigService : GlobalSettingServiceBase
    {
        public IPSecurityConfigService(IConfiguration configuration, MixGlobalSetting settings) : base(configuration, settings)
        {
        }
    }
}
