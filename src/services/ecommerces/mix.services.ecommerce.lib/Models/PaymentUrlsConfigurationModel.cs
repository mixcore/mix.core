using Mix.Services.Ecommerce.Lib.Models.Onepay;

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
