using Mix.Services.Ecommerce.Lib.Enums;
using Mix.Services.Ecommerce.Lib.Models.Onepay;
using Mix.Services.Ecommerce.Lib.ViewModels;
using Newtonsoft.Json.Linq;

namespace Mix.Services.Ecommerce.Lib.Interfaces
{
    public interface IPaymentService
    {
        string CreateSHA256Signature(Dictionary<string, string> parameters);
        Task<string?> GetPaymentUrl(OrderViewModel request, string returnUrl, CancellationToken cancellationToken);
        Task<PaymentStatus> ProcessPaymentResponse(JObject response, CancellationToken cancellationToken);
        Task<PaymentQueryResponse> Query(PaymentQueryRequest request, CancellationToken cancellationToken);
    }
}