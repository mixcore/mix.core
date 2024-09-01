using Microsoft.Extensions.Configuration;
using Mix.Shared.Models.Configurations;

namespace Mix.Shared.Services
{
    public class AuthConfigService : AppSettingServiceBase<MixAuthenticationConfigurations>
    {
        public AuthConfigService(IConfiguration configuration)
            : base(configuration, MixAppSettingsSection.Authentication, MixAppConfigFilePaths.Authentication, true)
        {
        }
    }
}
