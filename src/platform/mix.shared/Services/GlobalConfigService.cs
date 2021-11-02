using Microsoft.Extensions.Configuration;
using Mix.Heart.Enums;
using Mix.Heart.Exceptions;
using Mix.Shared.Constants;
using Mix.Shared.Enums;
using Mix.Shared.Models;
using System;
using System.Linq;

namespace Mix.Shared.Services
{
    public class GlobalConfigService : AppSettingServiceBase<GlobalConfigurations>
    {
        public GlobalConfigService(IConfiguration configuration)
            : base(configuration, MixAppSettingsSection.GlobalSettings, MixAppConfigFilePaths.Global)
        {
        }
        
        public bool IsInit => AppSettings.IsInit;
        public string DefaultCulture => AppSettings.DefaultCulture;
        public string Domain => AppSettings.Domain;
        public InitStep InitStatus => AppSettings.InitStatus;
    }
}
