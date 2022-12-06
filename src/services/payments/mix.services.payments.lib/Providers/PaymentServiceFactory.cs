using Mix.Services.Payments.Lib.Interface;
using Mix.Services.Payments.Lib.Enums;
using Mix.Services.Payments.Lib.Constants.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Mix.Services.Payments.Lib.Providers
{
    public class PaymentServiceFactory
    {
        public static IPaymentService? GetPaymentService(
            IServiceProvider serviceProvider,
            PaymentGateway provider)
        {
            switch (provider)
            {
                case PaymentGateway.Onepay:
                    return serviceProvider.GetRequiredService<OnepayService>();
                case PaymentGateway.Momo:
                    break;
            }
            return default;
        }
    }
}
