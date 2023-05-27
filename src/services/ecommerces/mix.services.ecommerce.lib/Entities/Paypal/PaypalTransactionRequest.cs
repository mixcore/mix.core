using Mix.Heart.Entities;
using Mix.Services.Ecommerce.Lib.Enums;
using Mix.Services.Ecommerce.Lib.Models.Paypal;
using Newtonsoft.Json.Linq;

namespace Mix.Services.Ecommerce.Lib.Entities.Paypal
{
    public class PaypalTransactionRequest : EntityBase<int>
    {
        public string? Intent { get; set; }
        public JObject Payer { get; set; }
        public JObject RedirectUrls { get; set; }
        public JArray Transactions { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public PaypalTransactionRequest()
        {
            
        }
        public PaypalTransactionRequest(PaypalRequest request)
        {
            Intent = request.intent;
            Payer = request.payer != null ? JObject.FromObject(request.payer) : new();
            RedirectUrls = request.redirect_urls != null ? JObject.FromObject(request.redirect_urls) : new();
            Transactions = request.transactions != null ? JArray.FromObject(request.transactions) : new();
        }
    }
}
