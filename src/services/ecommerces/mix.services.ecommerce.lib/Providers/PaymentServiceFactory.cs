using Microsoft.Extensions.DependencyInjection;
using Mix.Services.Ecommerce.Lib.Enums;
using Mix.Services.Ecommerce.Lib.Interfaces;

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
                    return serviceProvider.GetRequiredService<IPaymentService>();
                case PaymentGateway.Momo:
                    break;
            }
            return default;
        }
    }
}
