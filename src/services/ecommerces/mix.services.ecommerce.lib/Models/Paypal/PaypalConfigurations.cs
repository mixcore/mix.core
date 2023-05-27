using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Services.Ecommerce.Lib.Models.Paypal
{
    public sealed class PaypalConfigurations
    {
        public string ClientId { get; set; }
        public string SecretKey { get; set; }
        public string Mode { get; set; }
        public string BaseUrl { get; set; }

    }
}
