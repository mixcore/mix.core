using Microsoft.Extensions.Configuration;
using Mix.Database.Entities.Settings;
using Mix.Heart.Helpers;
using Mix.Shared.Models.Configurations;
using System.Linq;

namespace Mix.Database.Services.MixGlobalSettings
{
    public class GlobalSettingsService : GlobalSettingServiceBase<GlobalSettingsModel>
    {
        public GlobalSettingsService(IConfiguration configuration, MixGlobalSetting settings) : base(configuration, settings)
        {
        }

        #region Properties

        public int ResponseCache => AppSettings.ResponseCache;
        public string DefaultDomain => AppSettings.DefaultDomain;
        public string DefaultCulture => AppSettings.DefaultCulture;

        #endregion

        public override void SetConfig<TValue>(string name, TValue value, bool isSave = false)
        {
            base.SetConfig(name, value, isSave);
            AppSettings = RawSettings[MixAppSettingsSection.GlobalSettings].ToObject<GlobalSettingsModel>();
        }

        public override void SaveSettings()
        {
            base.SaveSettings();
            AppSettings = RawSettings[MixAppSettingsSection.GlobalSettings].ToObject<GlobalSettingsModel>();
        }
    }
}
