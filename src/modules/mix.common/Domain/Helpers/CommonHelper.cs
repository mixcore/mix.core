using Mix.Common.Models;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Service.Models;
using Mix.Shared.Models.Configurations;

namespace Mix.Common.Domain.Helpers
{
    public class CommonHelper
    {
        internal static GlobalSettings GetAppSettings(
            string aesKey,
            PortalConfigService portalConfigSrv, 
            MixAuthenticationConfigurations authConfigurations, 
            MixTenantSystemModel currentTenant)
        {
            //var cultures = _cultureService.Cultures;
            //var culture = _cultureService.LoadCulture(lang);
            // Get Settings
            return new()
            {
                Domain = currentTenant?.Configurations.Domain,
                DefaultCulture = currentTenant?.Configurations.DefaultCulture,
                IsEncryptApi = false,
                LastUpdateConfiguration = currentTenant?.Configurations.LastUpdateConfiguration,
                PortalThemeSettings = portalConfigSrv.RawSettings,
                ApiEncryptKey = aesKey,
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
