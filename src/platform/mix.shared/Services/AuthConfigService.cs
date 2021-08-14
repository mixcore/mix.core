using Mix.Shared.Abstracts;
using Mix.Shared.Constants;
using Mix.Shared.Models;

namespace Mix.Shared.Services
{
    public class AuthConfigService : JsonConfigurationServiceBase
    {
        public readonly MixAuthenticationConfigurations AuthConfigurations;
        public AuthConfigService() : base(MixAppConfigFilePaths.Authentication)
        {
            AuthConfigurations = AppSettings.ToObject<MixAuthenticationConfigurations>();
        }
    }
}
