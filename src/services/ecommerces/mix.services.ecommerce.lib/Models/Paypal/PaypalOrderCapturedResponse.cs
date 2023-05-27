using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Google.Rpc.Help.Types;

namespace Mix.Services.Ecommerce.Lib.Models.Paypal
{
    public sealed class PaypalOrderCapturedResponse
    {
        public string id { get; set; }
        public string status { get; set; }
        public PaymentSource payment_source { get; set; }
        public List<PurchaseUnit> purchase_units { get; set; }
        public Paypal payer { get; set; }
        public List<Link> links { get; set; }
    }
    public sealed class Paypal
    {
        public Name name { get; set; }
        public string email_address { get; set; }
        public string account_id { get; set; }
        public string payer_id { get; set; }
    }
    public sealed class Name
    {
        public string given_name { get; set; }
        public string surname { get; set; }
    }
    public sealed class PaymentSource
    {
        public Paypal paypal { get; set; }
    }
}
