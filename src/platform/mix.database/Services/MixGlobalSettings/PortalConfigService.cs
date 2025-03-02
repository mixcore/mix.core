using Microsoft.Extensions.Configuration;
using Mix.Database.Entities.MixDb.EntityConfigurations;
using Mix.Database.Entities.Settings;
using Mix.Heart.Helpers;
using Mix.Shared.Services;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Mix.Database.Services.MixGlobalSettings
{
    public class PortalConfigService : GlobalSettingServiceBase<JObject>
    {
        public PortalConfigService(IConfiguration configuration, MixGlobalSetting settings) : base(configuration, settings)
        {
        }
        protected override void LoadAppSettings()
        {
            var content = _settings.IsEncrypt ? AesEncryptionHelper.DecryptString(_settings.Settings, _aesKey)
            : _settings.Settings;
            RawSettings = JObject.Parse(content);
            AppSettings = RawSettings[_sectionName].ToObject<JObject>();
        }
    }
}
