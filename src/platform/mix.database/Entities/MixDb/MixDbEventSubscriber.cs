using Newtonsoft.Json.Linq;

namespace Mix.Database.Entities.MixDb
{
    public class MixDbEventSubscriber : EntityBase<int>
    {
        public int MixTenantId { get; set; }
        public string MixDbName { get; set; }
        public string Action { get; set; }
        public JObject Callback { get; set; }
    }
}
