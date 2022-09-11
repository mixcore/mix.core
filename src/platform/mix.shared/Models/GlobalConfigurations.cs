
using Newtonsoft.Json.Linq;

namespace Mix.Shared.Models
{
    public class GlobalConfigurations
    {
        public ConnectionStrings ConnectionStrings { get; set; }
        public bool IsInit { get; set; }
        public bool IsEncryptApi { get; set; }

        public bool EnableOcelot { get; set; }
        public JObject PortalThemeSettings { get; set; }

        public bool IsMaintenance { get; set; }
        public bool IsHttps { get; set; }
        public int? MaxPageSize { get; set; } = 100;
        public InitStep InitStatus { get; set; }
        public string DefaultCulture { get; set; }
        public string Domain { get; set; }
        public int ResponseCache { get; set; }
        public string ApiEncryptKey { get; set; }

        public SmtpConfiguration Smtp { get; set; }
        public DateTime? LastUpdateConfiguration { get; set; }
    }
}
