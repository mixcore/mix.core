using Mix.Services.Ecommerce.Lib.Models.Onepay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Services.Ecommerce.Lib.Models
{
    public sealed class PaymentConfigurationModel
    {
        public MixOnepayConfigurations Onepay { get; set; }
        public PaymentUrls Urls { get; set; }
    }
    public sealed class PaymentUrls
    {
        public string PaymentResponseUrl { get; set; }
        public string PaymentCartUrl { get; set; }
        public string PaymentSuccessUrl { get; set; }
        public string PaymentFailUrl { get; set; }
    }
}
