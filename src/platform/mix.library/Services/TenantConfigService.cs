namespace Mix.Lib.Services
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
            get => AppSettings.ApiEncryptKey;
            set => AppSettings.ApiEncryptKey = value;
        }

        public bool IsEncryptApi => AppSettings.IsEncryptApi;
        public string DefaultCulture => AppSettings.DefaultCulture;
        public string Domain => $"https://{AppSettings.Domain}";

        public override bool SaveSettings()
        {
            if (!string.IsNullOrEmpty(AppSettings.Domain) && !AppSettings.Domain.StartsWith("http"))
            {
                AppSettings.Domain = $"https://{AppSettings.Domain}";
            }
            return base.SaveSettings();
        }
    }
}
