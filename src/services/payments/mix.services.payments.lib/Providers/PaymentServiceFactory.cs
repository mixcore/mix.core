using Mix.Constant.Enums;
using Mix.Queue.Engines.MixQueue;
using Mix.Queue.Interfaces;
using Mix.Queue.Models.QueueSetting;
using Mix.Queue.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
