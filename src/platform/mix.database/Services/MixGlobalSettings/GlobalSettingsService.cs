using Microsoft.Extensions.Configuration;
using Mix.Database.Entities.Settings;
using Mix.Heart.Helpers;
using Mix.Shared.Models.Configurations;
using System.Linq;

namespace Mix.Database.Services.MixGlobalSettings
{
    public class GlobalSettingsService : GlobalSettingServiceBase
    {
        public GlobalSettingsModel AppSettings { get; set; }
        public GlobalSettingsService(IConfiguration configuration, MixGlobalSetting settings) : base(configuration, settings)
        {
            _sectionName = MixAppSettingsSection.GlobalSettings;
            AppSettings = RawSettings[MixAppSettingsSection.GlobalSettings].ToObject<GlobalSettingsModel>();
        }

        #region Properties

        public int ResponseCache => AppSettings.ResponseCache;
        public string DefaultDomain => AppSettings.DefaultDomain;
        public string DefaultCulture => AppSettings.DefaultCulture;

        #endregion

        //#region Instance
        ///// <summary>
        ///// The synchronize root
        ///// </summary>
        //protected static readonly object SyncRoot = new object();

        ///// <summary>
        ///// The instance
        ///// </summary>
        //private static GlobalSettingsService _instance;

        //public static GlobalSettingsService Instance
        //{
        //    get
        //    {
        //        if (_instance == null)
        //        {
        //            lock (SyncRoot)
        //            {
        //                if (_instance == null)
        //                {
        //                    using var dbContext = new GlobalSettingContext();
        //                    var settings = dbContext.MixGlobalSetting.First(m => m.SystemName == "global");
        //                    _instance = new(settings);
        //                }
        //            }
        //        }

        //        return _instance;
        //    }
        //}
        //#endregion

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
