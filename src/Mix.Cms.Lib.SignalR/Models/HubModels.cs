using Newtonsoft.Json;
using System;

namespace Mix.Cms.Lib.SignalR.Models
{
    public class MessengerConnection
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("connectionId")]
        public string ConnectionId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("avatar")]
        public string Avatar { get; set; }

        [JsonProperty("deviceId")]
        public string DeviceId { get; set; }
    }

    public class HubMessage
    {
        //From Connection
        [JsonProperty("connection")]
        public MessengerConnection Connection { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("createdDate")]
        public DateTime CreatedDate { get; set; }
    }
}