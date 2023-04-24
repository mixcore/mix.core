using Mix.Heart.Entities;
using Newtonsoft.Json.Linq;

namespace Mix.Mixdb.Entities
{
    public class MixDbEventSubscriber : EntityBase<int>
    {
        public int MixTenantId { get; set; }
        public string? MixDbName { get; set; }
        public string? Action { get; set; }
        public JObject? Callback { get; set; }
    }
}
