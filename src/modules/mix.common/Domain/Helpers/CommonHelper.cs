using Mix.Common.Models;
using Mix.Heart.Helpers;
using Mix.Lib.Services;
using Mix.Shared.Models;
using Mix.Shared.Services;

namespace Mix.Common.Domain.Helpers
{
    public class CommonHelper
    {
        internal static GlobalSettings GetAppSettings(MixAuthenticationConfigurations _authConfigurations)
        {
            //var cultures = _cultureService.Cultures;
            //var culture = _cultureService.LoadCulture(lang);
            // Get Settings
            return new()
            {
                Domain = GlobalConfigService.Instance.AppSettings.Domain,
                PortalThemeSettings = PortalConfigService.Instance.AppSettings,
                ApiEncryptKey = GlobalConfigService.Instance.AppSettings.ApiEncryptKey,
                IsEncryptApi = GlobalConfigService.Instance.AppSettings.IsEncryptApi,
                //Cultures = cultures,
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
