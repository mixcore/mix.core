using Mix.Shared.Services;

namespace Mix.Service.Services
{
    public class MixConfigurationService : JsonConfigurationServiceBase
    {
        public MixConfigurationService() : base(MixAppConfigFilePaths.Configration)
        {
        }
    }
}
