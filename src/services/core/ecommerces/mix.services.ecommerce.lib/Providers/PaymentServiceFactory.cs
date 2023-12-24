using Microsoft.Extensions.DependencyInjection;
using Mix.Heart.Enums;
using Mix.Heart.Exceptions;
using Mix.Services.Ecommerce.Lib.Enums;
using Mix.Services.Ecommerce.Lib.Interfaces;
using Mix.Services.Ecommerce.Lib.Services;

namespace Mix.Services.Ecommerce.Lib.Providers
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
                case PaymentGateway.Paypal:
                    return serviceProvider.GetRequiredService<PaypalService>();
                case PaymentGateway.Momo:
                    break;
                default:
                    throw new MixException(MixErrorStatus.ServerError, $"Not Implement {provider} payment");
            }
            return default;
        }
    }
}
