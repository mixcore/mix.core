using Mix.Shared.Constants;

namespace Mix.Shared.Services
{
    public class SmtpConfigService : JsonConfigurationServiceBase
    {
        public SmtpConfigService() : base(MixAppConfigFilePaths.Smtp)
        {
        }
    }
}
