using Microsoft.Extensions.Configuration;
using Mix.Database.Entities.MixDb.EntityConfigurations;
using Mix.Database.Entities.Settings;
using Mix.Shared.Services;
using Newtonsoft.Json.Linq;

namespace Mix.Database.Services.MixGlobalSettings
{
    public class PortalConfigService : GlobalSettingServiceBase<JObject>
    {
        public PortalConfigService(IConfiguration configuration, MixGlobalSetting settings) : base(configuration, settings)
        {
        }
    }
}
