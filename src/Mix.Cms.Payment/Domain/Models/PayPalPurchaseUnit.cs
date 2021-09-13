using Newtonsoft.Json;

namespace Mix.Cms.Payment.Domain.Models
{
    public class PayPalPurchaseUnit
    {
        [JsonProperty("amount")]
        public PayPalAmount Amount { get; set; }
    }
}
