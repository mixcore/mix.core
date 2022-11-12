using Mix.Shared.Models;
using Newtonsoft.Json.Linq;

namespace Mix.Lib.Models
{
    public class TenantConfigurationModel
    {
        public JObject PortalThemeSettings { get; set; }
        public bool IsMaintenance { get; set; }
        public bool IsEncryptApi { get; set; }
        public int? MaxPageSize { get; set; } = 100;

        public string DefaultCulture { get; set; }
        public string Domain { get; set; }
        public string ApiEncryptKey { get; set; }

        public SmtpConfiguration Smtp { get; set; }

        public DateTime? LastUpdateConfiguration { get; set; }
    }
}
