using Mix.Heart.Enums;
using Mix.Shared.Abstracts;
using Mix.Shared.Constants;
using Newtonsoft.Json.Linq;

namespace Mix.Shared.Services
{
    public class IPSecurityConfigService : JsonConfigurationServiceBase
    {
        public IPSecurityConfigService() : base(MixAppConfigFilePaths.IPSecurity)
        {
        }
    }
}
