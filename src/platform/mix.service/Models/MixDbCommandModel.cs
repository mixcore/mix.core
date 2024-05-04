using Newtonsoft.Json.Linq;

namespace Mix.Service.Models
{
    public class MixDbCommandModel
    {
        public int MixTenantId { get; set; }
        public string MixDbName { get; set; }
        public string ConnectionId { get; set; }
        public JObject Body { get; set; }
        public string? RequestedBy { get; set; }

        public MixDbCommandModel()
        {
        }
    }
}
