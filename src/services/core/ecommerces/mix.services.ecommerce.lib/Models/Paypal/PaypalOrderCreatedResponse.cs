using Mix.Services.Ecommerce.Lib.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Services.Ecommerce.Lib.Models.Paypal
{
    public class PaypalOrderCreatedResponse
    {
        public string id { get; set; }
        public string? intent { get; set; }
        public string? status { get; set; }
        public DateTime create_time { get; set; }
        public List<PaypalLink> links { get; set; }
        public List<ResponsePurchaseUnit> purchase_units { get; set; } = new();

        public PaypalOrderCreatedResponse()
        {
        }
    }

}
