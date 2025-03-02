using Mix.Heart.Enums;
using Mix.Shared.Interfaces;

namespace Mix.Shared.Models.Configurations
{
    public class GlobalSettingsModel : IGlobalConfigurations
    {
        public bool IsUpdateSystemDatabases { get; set; }
        public int? DefaultPageSize { get; set; }
        public int ResponseCache { get; set; }
        public string DefaultDomain { get; set; }
        public string DefaultCulture { get; set; }
        public bool EnableOcelot { get; set; }
    }
}
