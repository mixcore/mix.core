using Newtonsoft.Json;
using System;

namespace Mix.Cms.Lib.SignalR.Models
{
    public class HubRequest<T>
    {
        [JsonProperty("uid")]
        public string Uid { get; set; }

        [JsonProperty("connection")]
        public MessengerConnection Connection { get; set; }

        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("to")]
        public string To { get; set; }

        [JsonProperty("mixDatabaseName")]
        public string MixDatabaseName { get; set; }

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

        [JsonProperty("isSave")]
        public bool IsSave { get; set; }

        [JsonProperty("specificulture")]
        public string Specificulture { get; set; }
    }
}