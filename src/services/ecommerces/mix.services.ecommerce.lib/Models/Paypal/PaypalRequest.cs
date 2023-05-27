using Mix.Services.Ecommerce.Lib.ViewModels;

namespace Mix.Services.Ecommerce.Lib.Models.Paypal
{
    public sealed class PaypalRequest
    {
        public string? intent { get; set; }
        public Payer payer { get; set; }
        public RedirecUrls redirect_urls { get; set; }
        public List<Transaction> transactions { get; set; }

        public PaypalRequest()
        {

        }
    }

    public class RedirecUrls
    {
        public string return_url { get; set; }
        public string cancel_url { get; set; }
    }
}
