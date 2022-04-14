

namespace Mix.Shared.Services
{
    public class IPSecurityConfigService : JsonConfigurationServiceBase
    {
        public IPSecurityConfigService() : base(MixAppConfigFilePaths.IPSecurity)
        {
        }
    }
}
