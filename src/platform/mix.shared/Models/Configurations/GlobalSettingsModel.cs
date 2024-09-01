using Mix.Shared.Interfaces;

namespace Mix.Shared.Models.Configurations
{
    public class GlobalSettingsModel : IGlobalConfigurations
    {
        public bool IsInit { get; set; }
        public bool IsUpdateSystemDatabases { get; set; }
        public bool EnableAuditLog { get; set; }
        public bool IsLogStream { get; set; }
        public bool AllowAnyOrigin { get; set; }
        public bool IsHttps { get; set; }
        public int ResponseCache { get; set; }
        public string ApiEncryptKey { get; set; }
        public string AesKey { get; set; }
        public string DefaultDomain { get; set; }
        public string DefaultCulture { get; set; }
        public bool EnableOcelot { get; set; }
        public InitStep InitStatus { get; set; }
    }
}
