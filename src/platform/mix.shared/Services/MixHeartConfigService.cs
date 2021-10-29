using Microsoft.Extensions.Configuration;
using Mix.Heart.Constants;
using Mix.Heart.Enums;
using Mix.Heart.Exceptions;
using Mix.Heart.Model;
using Mix.Shared.Constants;
using Mix.Shared.Enums;
using System;

namespace Mix.Shared.Services
{
    public class MixHeartConfigService : AppSettingServiceBase<MixHeartConfigurationModel>
    {
        public MixHeartConfigService(IConfiguration configuration)
            : base(configuration, MixAppSettingsSection.MixHeart, MixAppConfigFilePaths.MixHeart)
        {
        }

        public MixCacheDbProvider DatabaseProvider
        {
            get => AppSettings.DatabaseProvider;
        }

        public string GetConnectionString(string name)
        {
            switch (name)
            {
                case MixHeartConstants.CACHE_CONNECTION:
                    return AppSettings.ConnectionStrings.CacheConnection;
                case MixHeartConstants.AUDIT_LOG_CONNECTION:
                    return AppSettings.ConnectionStrings.AuditLogConnection;
                default:
                    return string.Empty;
            }
        }

        public void SetConnectionString(string name, string value)
        {
            switch (name)
            {
                case MixHeartConstants.CACHE_CONNECTION:
                    AppSettings.ConnectionStrings.CacheConnection = value;
                    break;
                case MixHeartConstants.AUDIT_LOG_CONNECTION:
                    AppSettings.ConnectionStrings.AuditLogConnection = value;
                    break;
                default:
                    break;
            }
            SaveSettings();
        }

        protected override void BindAppSettings(IConfigurationSection settings)
        {
            try
            {
                AppSettings = new();
                settings.Bind(AppSettings);
            }
            catch (Exception ex)
            {
                throw new MixException($"Cannot load config section {_sectionName}: {ex.Message}");
            }
        }
    }
}
