using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mix.Cms.Payment.Domain.Constants
{
    public class PayPalConstants
    {
        public const string CONFIG_CLIENT_ID = "PayPal.ClienID";
        public const string CONFIG_SECRET = "PayPal.Secret";
        public const string CONFIG_TOKEN_ENDPOINT = "PayPal.TokenEndpoint";

        public const string ENDPOINT_ORDER = @"/v2/checkout/orders";
    }
}
