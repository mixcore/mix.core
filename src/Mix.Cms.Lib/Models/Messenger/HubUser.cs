using Newtonsoft.Json;

namespace Mix.Cms.Lib.Models.Messenger
{
    public class HubUser
    {
        [JsonProperty("connectionId")]
        public string ConnectionId { get; set; }
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("avatar")]
        public string Avatar { get; set; }
    }
}
