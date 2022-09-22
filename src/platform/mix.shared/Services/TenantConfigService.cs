

using Mix.Lib.Models.Configurations;
using Mix.Shared.Models;

namespace Mix.Shared.Services
{
    public class TenantConfigService : ConfigurationServiceBase<TenantConfigurationModel>
    {

        public TenantConfigService(string tenantName)
            : base($"{MixAppConfigFilePaths.AppConfigs}/{tenantName}/configurations")
        {
            AppSettings ??= new();
        }

        public new string AesKey
        {
            get { return AppSettings.ApiEncryptKey; }
            set { AppSettings.ApiEncryptKey = value; }
        }


        public bool IsEncryptApi => AppSettings.IsEncryptApi;
        public string DefaultCulture => AppSettings.DefaultCulture;
        public string Domain => AppSettings.Domain;
    }
}
