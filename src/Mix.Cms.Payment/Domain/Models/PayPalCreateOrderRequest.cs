using Mix.Cms.Payment.Domain.Dtos;
using Mix.Cms.Payment.Domain.Enums;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Mix.Cms.Payment.Domain.Models
{
    public class PayPalCreateOrderRequest
    {
        [JsonProperty("intent")]
        public PayPalIntentEnum Intent { get; set; }
        [JsonProperty("purchase_units")]
        public IEnumerable<PayPalPurchaseUnit> PurchaseUnit { get; set; }

        public PayPalCreateOrderRequest(PayPalCreateOrderDto dto)
        {
            Intent = PayPalIntentEnum.CAPTURE;
            PurchaseUnit = dto.PurchaseUnit;
        }
    }
}
