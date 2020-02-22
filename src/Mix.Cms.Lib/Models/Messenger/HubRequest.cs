using Newtonsoft.Json;
using System;

namespace Mix.Cms.Messenger.Models
{
    public class HubRequest<T>
    {
        [JsonProperty("uid")]
        public string Uid { get; set; }

        [JsonProperty("attributeSetName")]
        public string AttributeSetName { get; set; }

        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("data")]
        public T Data { get; set; }

        [JsonProperty("requestDate")]
        public DateTime RequestDate { get { return DateTime.UtcNow; } }

        [JsonProperty("room")]
        public string Room { get; set; }

        [JsonProperty("isMyself")]
        public bool IsMySelf { get; set; }

        [JsonProperty("specificulture")]
        public string Specificulture { get; set; }
    }
}