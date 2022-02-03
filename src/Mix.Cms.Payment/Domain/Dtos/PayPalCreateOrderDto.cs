using Mix.Cms.Payment.Domain.Enums;
using Mix.Cms.Payment.Domain.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Mix.Cms.Payment.Domain.Dtos
{
    public class PayPalCreateOrderDto
    {
        [JsonProperty("purchase_units")]
        public IEnumerable<PayPalPurchaseUnit> PurchaseUnit { get; set; }
    }
}
