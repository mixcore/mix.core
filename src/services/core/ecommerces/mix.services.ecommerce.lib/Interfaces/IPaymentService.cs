﻿using Mix.Services.Ecommerce.Lib.Enums;
using Mix.Services.Ecommerce.Lib.ViewModels;
using Newtonsoft.Json.Linq;

namespace Mix.Services.Ecommerce.Lib.Interfaces
{
    public interface IPaymentService
    {
        Task<JObject> GetPaymentRequestAsync(OrderViewModel request, string againUrl, string returnUrl, CancellationToken cancellationToken);
        Task<string?> GetPaymentUrl(OrderViewModel request, string againUrl, string returnUrl, CancellationToken cancellationToken);
        Task<PaymentStatus> ProcessPaymentResponse(OrderViewModel orderDetail, JObject response, CancellationToken cancellationToken);
    }
}