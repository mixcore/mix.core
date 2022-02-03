using Newtonsoft.Json;

namespace Mix.Cms.Payment.Domain.Models
{
    public class PayPalLink
    {
        [JsonProperty("href")]
        public string Href { get; set; }
        [JsonProperty("rel")]
        public string Rel { get; set; }
        [JsonProperty("method")]
        public string Method { get; set; }
    }
}
