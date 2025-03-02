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
        public List<ResponsePurchaseUnit> purchase_units { get; set; }
        public Paypal payer { get; set; }
        public List<PaypalLink> links { get; set; }
    }
    public sealed class Paypal
    {
        public PaypalName name { get; set; }
        public string email_address { get; set; }
        public string account_id { get; set; }
        public string payer_id { get; set; }
        public string account_status { get; set; }
        public PaypalAddress address { get; set; }
    }

    public class PaypalAddress
    {
        public string address_line_1 { get; set; }
        public string address_line_2 { get; set; }
        public string admin_area_2 { get; set; }
        public string admin_area_1 { get; set; }
        public string postal_code { get; set; }
        public string country_code { get; set; }
    }

    public sealed class PaypalName
    {
        public string given_name { get; set; }
        public string surname { get; set; }
    }
    public sealed class PaymentSource
    {
        public Paypal paypal { get; set; }
    }
}
