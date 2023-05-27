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
        public string Intent { get; set; }
        public string State { get; set; }
        public string Cart { get; set; }
        public JObject Payer { get; set; }
        public JArray Transactions { get; set; }
        public DateTime CreatedTime { get; set; }
        public JArray Links { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public int MixTentantId { get; set; }
        public PaypalTransactionResponse()
        {
            
        }
        public PaypalTransactionResponse(PayPalPaymentExecutedResponse response)
        {
            PaypalId = response.id;
            Intent = response.intent;
            State = response.state;
            Cart = response.cart;
            CreatedTime = response.create_time;
            Payer = response.payer != null ? JObject.FromObject(response.payer) : new();
            Transactions = response.transactions != null ? JArray.FromObject(response.transactions) : new();
            Links = response.links != null ? JArray.FromObject(response.links) : new();
        }
    }
}
