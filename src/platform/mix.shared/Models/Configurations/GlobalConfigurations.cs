using Newtonsoft.Json.Linq;

namespace Mix.Shared.Models.Configurations
{
    public class GlobalConfigurations
    {
        public bool IsInit { get; set; }
        public bool IsHttps { get; set; }
        public int ResponseCache { get; set; }
        public string ApiEncryptKey { get; set; }
        public bool EnableOcelot { get; set; }
        public InitStep InitStatus { get; set; }
    }
}
