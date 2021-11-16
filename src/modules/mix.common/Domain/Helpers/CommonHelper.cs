using Mix.Common.Models;
using Mix.Heart.Enums;
using Mix.Heart.Helpers;
using Mix.Lib.Services;
using Mix.Shared.Constants;
using Mix.Shared.Enums;
using Mix.Shared.Models;
using Mix.Shared.Services;
using Newtonsoft.Json.Linq;
using System;

namespace Mix.Common.Domain.Helpers
{
    public class CommonHelper
    {
        internal static GlobalSettings GetAppSettings(string lang,
            MixAuthenticationConfigurations _authConfigurations,
            CultureService _cultureService)
        {
            lang ??= GlobalConfigService.Instance.AppSettings.DefaultCulture;
            var cultures = _cultureService.Cultures;
            var culture = _cultureService.LoadCulture(lang);
            // Get Settings
            return new()
            {
                Domain = GlobalConfigService.Instance.AppSettings.Domain,
                Lang = lang,
                PortalThemeSettings = GlobalConfigService.Instance.AppSettings.PortalThemeSettings,
                ApiEncryptKey = GlobalConfigService.Instance.AppSettings.ApiEncryptKey,
                IsEncryptApi = GlobalConfigService.Instance.AppSettings.IsEncryptApi,
                Cultures = cultures,
                PageTypes = Enum.GetNames(typeof(MixPageType)),
                ModuleTypes = Enum.GetNames(typeof(MixModuleType)),
                MixDatabaseTypes = Enum.GetNames(typeof(MixDatabaseType)),
                DataTypes = Enum.GetNames(typeof(MixDataType)),
                Statuses = Enum.GetNames(typeof(MixContentStatus)),
                RSAKeys = RSAEncryptionHelper.GenerateKeys(),
                ExternalLoginProviders = new JObject()
                {
                    new JProperty("Facebook", _authConfigurations.Facebook?.AppId),
                    new JProperty("Google", _authConfigurations.Google?.AppId),
                    new JProperty("Twitter", _authConfigurations.Twitter?.AppId),
                    new JProperty("Microsoft", _authConfigurations.Microsoft?.AppId),
                },
                LastUpdateConfiguration = GlobalConfigService.Instance.AppSettings.LastUpdateConfiguration

            };
        }
    }
}
