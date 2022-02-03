using Mix.Cms.Payment.Domain.Enums;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Mix.Cms.Payment.Domain.Models
{
    public class PayPalCreateOrderResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("status")]
        public PayPalResponseStatusEnum Status { get; set; }
        [JsonProperty("links")]
        public IEnumerable<PayPalLink> Links { get; set; }
    }
}
