using Microsoft.Extensions.Configuration;
using Mix.Database.Entities.Settings;
using Mix.Shared.Models.Configurations;
using Mix.Shared.Services;

namespace Mix.Database.Services.MixGlobalSettings
{
    public class AuthConfigService : GlobalSettingServiceBase
    {
        public MixAuthenticationConfigurations AppSettings { get; set; }
        public AuthConfigService(IConfiguration configuration, MixGlobalSetting settings) : base(configuration, settings)
        {
        }
        protected override void LoadAppSettings()
        {
            base.LoadAppSettings();
            AppSettings = RawSettings["Authentication"].ToObject<MixAuthenticationConfigurations>();
        }
    }
}
