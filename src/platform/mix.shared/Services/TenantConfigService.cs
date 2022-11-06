

using Mix.Lib.Models.Configurations;

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
        public string Domain => $"//{AppSettings.Domain}";

        public override bool SaveSettings()
        {
            if (!string.IsNullOrEmpty(AppSettings.Domain) && !AppSettings.Domain.StartsWith("http"))
            {
                AppSettings.Domain = $"//{AppSettings.Domain}";
            }
            return base.SaveSettings();
        }
    }
}
