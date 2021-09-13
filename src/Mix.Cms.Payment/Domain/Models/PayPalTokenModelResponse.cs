using Newtonsoft.Json;

namespace Mix.Cms.Payment.Domain.Models
{
    public class PayPalTokenModelResponse
    {
        [JsonProperty("scope")]
        public string Scope { get; set; }
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("token_type")]
        public string TokenType { get; set; }
        [JsonProperty("app_id")]
        public string AppId { get; set; }
        [JsonProperty("expires_in")]
        public string ExpiresIn { get; set; }
        [JsonProperty("nonce")]
        public string Nonce { get; set; }
    }
}
