using Mix.Shared.Models;
using Newtonsoft.Json.Linq;

namespace Mix.Service.Models
{
    public class TenantConfigurationModel
    {
        public JObject PortalThemeSettings { get; set; }
        public bool IsMaintenance { get; set; }
        public string DefaultCulture { get; set; }
        public string? Domain { get; set; }
        public int MaxPageSize { get; set; } = 1000;
        public DateTime? LastUpdateConfiguration { get; set; }
    }
}
