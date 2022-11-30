using Mix.Services.Payments.Lib.Entities.Onepay;
using Mix.Services.Payments.Lib.Enums;
using Mix.Services.Payments.Lib.ViewModels.Mix;
using Mix.Services.Payments.Lib.ViewModels.Onepay.Onepay;
using Newtonsoft.Json.Linq;

namespace Mix.Services.Payments.Lib.Interface
{
    public interface IPaymentService
    {
        string CreateSHA256Signature(Dictionary<string, string> parameters);
        Task<string?> GetPaymentUrl(OrderViewModel request, string returnUrl);
        Task<PaymentStatus> ProcessPaymentResponse(JObject response);
        Task<PaymentQueryResponse> Query(PaymentQueryRequest request);
    }
}