using Microsoft.Extensions.Configuration;
using Mix.Database.Entities.Settings;
using Mix.Heart.Constants;
using Mix.Heart.Enums;
using Mix.Heart.Models;
using Mix.Heart.Services;

namespace Mix.Database.Services.MixGlobalSettings
{
    public class MixHeartConfigService : GlobalSettingServiceBase
    {
        public MixHeartConfigurationModel AppSettings { get; set; }
        public MixHeartConfigService(IConfiguration configuration, MixGlobalSetting settings) : base(configuration, settings)
        {
            AppSettings = RawSettings.ToObject<MixHeartConfigurationModel>();
        }


        public MixDatabaseProvider DatabaseProvider
        {
            get => AppSettings.DatabaseProvider;
        }

        public string GetConnectionString(string name)
        {
            switch (name)
            {
                case MixHeartConstants.CACHE_CONNECTION:
                    return AppSettings.CacheConnection;
                default:
                    return string.Empty;
            }
        }

        public void SetConnectionString(string name, string value)
        {
            switch (name)
            {
                case MixHeartConstants.CACHE_CONNECTION:
                    AppSettings.CacheConnection = value;
                    break;
                default:
                    break;
            }
            SaveSettings();
        }
    }
}
