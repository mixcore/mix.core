using CommandLine;
using Mix.Heart.Entities;
using Mix.Services.Ecommerce.Lib.Enums;
using Mix.Services.Ecommerce.Lib.Models.Paypal;
using Newtonsoft.Json.Linq;

namespace Mix.Services.Ecommerce.Lib.Entities.Paypal
{
    public class PaypalTransactionResponse : EntityBase<int>
    {
        public string PaypalId { get; set; }
        public string PaypalStatus { get; set; }
        public JObject Payer { get; set; }
        public JObject PaymentSource { get; set; }
        public JArray PurchaseUnits { get; set; }
        public JArray Links { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public int TentantId { get; set; }
        public PaypalTransactionResponse()
        {
            
        }
        public PaypalTransactionResponse(PaypalOrderCapturedResponse response)
        {
            PaypalId = response.id;
            PaypalStatus = response.status;
            PaymentSource = response.payment_source != null ? JObject.FromObject(response.payment_source) : new();
            Payer = response.payer != null ? JObject.FromObject(response.payer) : new();
            PurchaseUnits = response.purchase_units != null ? JArray.FromObject(response.purchase_units) : new();
            Links = response.links != null ? JArray.FromObject(response.links) : new();
        }
    }
}
