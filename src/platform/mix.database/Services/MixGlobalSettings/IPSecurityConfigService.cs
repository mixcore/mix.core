using Microsoft.Extensions.Configuration;
using Mix.Database.Entities.Settings;
using Mix.Shared.Services;
using Newtonsoft.Json.Linq;

namespace Mix.Database.Services.MixGlobalSettings
{
    public class IPSecurityConfigService : GlobalSettingServiceBase<JObject>
    {
        public IPSecurityConfigService(IConfiguration configuration, MixGlobalSetting settings) : base(configuration, settings)
        {
        }
    }
}
