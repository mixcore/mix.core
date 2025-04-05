using Microsoft.Extensions.Configuration;
using Mix.Database.Entities.MixDb.EntityConfigurations;
using Mix.Database.Entities.Settings;
using Mix.Heart.Helpers;
using Mix.Shared.Models.Configurations;
using Mix.Shared.Services;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Mix.Database.Services.MixGlobalSettings
{
    public class PortalConfigService : GlobalSettingServiceBase<PortalConfigurationModel>
    {
        public PortalConfigService(IConfiguration configuration, MixGlobalSetting settings) : base(configuration, settings)
        {
        }
    }
}
