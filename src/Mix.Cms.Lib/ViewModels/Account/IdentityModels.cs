using Newtonsoft.Json;
using System;

namespace Mix.Cms.Lib.ViewModels.Account
{
    public class AccessTokenViewModel
    {
        [JsonProperty("access_token")]
        public string Access_token { get; set; }

        [JsonProperty("token_type")]
        public string Token_type { get; set; }

        [JsonProperty("refresh_token")]
        public string Refresh_token { get; set; }

        [JsonProperty("expires_in")]
        public int Expires_in { get; set; }

        [JsonProperty("client_id")]
        public string Client_id { get; set; }

        [JsonProperty("issued")]
        public DateTime Issued { get; set; }

        [JsonProperty("expires")]
        public DateTime Expires { get; set; }

        [JsonProperty("deviceId")]
        public string DeviceId { get; set; }

        [JsonProperty("info")]
        public MixUserViewModel Info { get; set; }

        [JsonProperty("lastUpdateConfiguration")]
        public DateTime? LastUpdateConfiguration { get; set; }
    }
}