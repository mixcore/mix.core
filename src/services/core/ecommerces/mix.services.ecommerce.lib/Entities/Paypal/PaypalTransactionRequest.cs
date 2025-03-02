using Mix.Heart.Entities;
using Mix.Services.Ecommerce.Lib.Enums;
using Mix.Services.Ecommerce.Lib.Models.Paypal;
using Newtonsoft.Json.Linq;

namespace Mix.Services.Ecommerce.Lib.Entities.Paypal
{
    public class PaypalTransactionRequest : EntityBase<int>
    {
        public string? Intent { get; set; }
        public JObject ApplicationContext { get; set; }
        public JArray PurchaseUnits { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public PaypalTransactionRequest()
        {
        }
        public PaypalTransactionRequest(PaypalOrderRequest request)
        {
            Intent = request.intent;
            ApplicationContext = request.application_context != null ? JObject.FromObject(request.application_context) : new();
            PurchaseUnits = request.purchase_units != null ? JArray.FromObject(request.purchase_units) : new();
        }
    }
}
