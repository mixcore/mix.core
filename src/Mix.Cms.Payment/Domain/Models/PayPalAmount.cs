using Newtonsoft.Json;

namespace Mix.Cms.Payment.Domain.Models
{
    public class PayPalAmount
    {
        [JsonProperty("currency_code")]
        public string CurrencyCode { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
