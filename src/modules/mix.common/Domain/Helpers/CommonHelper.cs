using Mix.Service.Models;
using Mix.Shared.Models.Configurations;
using Mix.Shared.Services;

namespace Mix.Common.Domain.Helpers
{
    public class CommonHelper
    {
        internal static Common.Models.GlobalSettings GetAppSettings(MixAuthenticationConfigurations authConfigurations, MixTenantSystemModel currentTenant)
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
                    new JProperty("Facebook", authConfigurations.Facebook?.AppId),
                    new JProperty("Google", authConfigurations.Google?.AppId),
                    new JProperty("Twitter", authConfigurations.Twitter?.AppId),
                    new JProperty("Microsoft", authConfigurations.Microsoft?.AppId),
                }
            };
        }
    }
}
