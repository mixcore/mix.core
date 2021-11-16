using Microsoft.Extensions.Configuration;
using Mix.Shared.Constants;
using Mix.Shared.Enums;
using Mix.Shared.Models;

namespace Mix.Shared.Services
{
    public class GlobalConfigService : ConfigurationServiceBase<GlobalConfigurations>
    {
        public GlobalConfigService()
            : base(MixAppConfigFilePaths.Global)
        {
        }
        
        public bool IsInit => AppSettings.IsInit;
        public string DefaultCulture => AppSettings.DefaultCulture;
        public string Domain => AppSettings.Domain;
        public InitStep InitStatus => AppSettings.InitStatus;
    }
}
