using Microsoft.Extensions.Configuration;
using Mix.Database.Entities.Settings;
using Mix.Shared.Models;
using Mix.Shared.Services;

namespace Mix.Database.Services.MixGlobalSettings
{
    public class SmtpConfigService : GlobalSettingServiceBase<SmtpConfiguration>
    {
        public SmtpConfigService(IConfiguration configuration, MixGlobalSetting settings) : base(configuration, settings)
        {
        }
    }
}
