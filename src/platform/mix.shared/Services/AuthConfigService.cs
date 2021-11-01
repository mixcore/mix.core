using Microsoft.Extensions.Configuration;
using Mix.Heart.Exceptions;
using Mix.Shared.Constants;
using Mix.Shared.Enums;
using Mix.Shared.Models;
using System;

namespace Mix.Shared.Services
{
    public class AuthConfigService : AppSettingServiceBase<MixAuthenticationConfigurations>
    {
        public AuthConfigService(IConfiguration configuration)
            : base(configuration, MixAppSettingsSection.Authentication, MixAppConfigFilePaths.Authentication)
        {
        }

        protected override void BindAppSettings(IConfigurationSection settings)
        {
            try
            {
                base.AppSettings = new MixAuthenticationConfigurations();
                settings.Bind(base.AppSettings);
            }
            catch (Exception ex)
            {
                throw new MixException($"Cannot load config section {_sectionName}: {ex.Message}");
            }
        }
    }
}
