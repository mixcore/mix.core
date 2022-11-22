using Mix.Common.Models;
using Mix.Lib.Models;
using Mix.Shared.Models.Configurations;
using Mix.Shared.Services;

namespace Mix.Common.Domain.Helpers
{
    public class CommonHelper
    {
        internal static GlobalSettings GetAppSettings(MixAuthenticationConfigurations _authConfigurations, MixTenantSystemModel currentTenant)
        {
            //var cultures = _cultureService.Cultures;
            //var culture = _cultureService.LoadCulture(lang);
            // Get Settings
            return new()
            {
                Domain = currentTenant?.Configurations.Domain,
                DefaultCulture = currentTenant?.Configurations.DefaultCulture,
                IsEncryptApi = currentTenant?.Configurations.IsEncryptApi ?? false,
                LastUpdateConfiguration = currentTenant?.Configurations.LastUpdateConfiguration,
                PortalThemeSettings = PortalConfigService.Instance.AppSettings,
                ApiEncryptKey = GlobalConfigService.Instance.AppSettings.ApiEncryptKey,
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
                }
            };
        }
    }
}
