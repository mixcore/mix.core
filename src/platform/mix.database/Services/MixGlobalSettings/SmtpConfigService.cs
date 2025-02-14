using Microsoft.Extensions.Configuration;
using Mix.Database.Entities.Settings;
using Mix.Shared.Services;

namespace Mix.Database.Services.MixGlobalSettings
{
    public class SmtpConfigService : GlobalSettingServiceBase
    {
        public SmtpConfigService(IConfiguration configuration, MixGlobalSetting settings) : base(configuration, settings)
        {
        }
    }
}
