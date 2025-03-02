using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Mix.Constant.Constants;
using Mix.Heart.Enums;
using Mix.Heart.Exceptions;
using Mix.Heart.Services;
using Mix.Heart.UnitOfWork;
using Mix.Service.Services;
using Mix.Services.Ecommerce.Lib.Entities.Paypal;
using Mix.Services.Ecommerce.Lib.Enums;
using Mix.Services.Ecommerce.Lib.Interfaces;
using Mix.Services.Ecommerce.Lib.Models.Paypal;
using Mix.Services.Ecommerce.Lib.ViewModels;
using Mix.Services.Ecommerce.Lib.ViewModels.Paypal;
using Mix.Shared.Services;
using Newtonsoft.Json.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using Mix.Services.Payments.Lib.Constants;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.Text.RegularExpressions;
using Quartz.Util;
using Mix.Lib.Interfaces;

namespace Mix.Services.Ecommerce.Lib.Services
{
    // Ref https://juldhais.net/paypal-checkout-integration-with-asp-net-core-90cb22cd465d
    public sealed class PaypalService : TenantServiceBase, IPaymentService
    {
        private readonly UnitOfWorkInfo<PaypalDbContext> _cmsUow;
        private readonly HttpClient _httpClient;
        private PaypalConfigurations Settings { get; set; } = new PaypalConfigurations();
        public PayPalAccessToken Token { get => GetPayPalAccessTokenAsync() ?? throw new MixException(MixErrorStatus.Badrequest, "Cannot get token"); }
        public PaypalService(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            UnitOfWorkInfo<PaypalDbContext> cmsUow,
            MixCacheService cacheService,
            IMixTenantService mixTenantService)
            : base(httpContextAccessor, cacheService, mixTenantService)
        {
            var session = configuration.GetSection(MixAppSettingsSection.Payments).GetSection("Paypal");
            session.Bind(Settings);
            _cmsUow = cmsUow;
            _httpClient = GetPaypalHttpClient();
        }

        public async Task<JObject> GetPaymentRequestAsync(
            OrderViewModel order,
            string cancelUrl, string returnUrl, CancellationToken cancellationToken)
        {
            var request = new PaypalOrderRequest(PaypalConstants.CreateOrderIntent, order, $"{returnUrl.TrimEnd('/')}?orderId={order.Id}", $"{cancelUrl.TrimEnd('/')}?orderId={order.Id}");
            await SaveOrderRequest(request, OrderStatus.WAITING_FOR_PAYMENT, cancellationToken);
            var payment = JObject.FromObject(request);

            return payment;
        }

        public async Task<string?> GetPaymentUrl(OrderViewModel order, string againUrl, string returnUrl, CancellationToken cancellationToken)
        {
            try
            {
                var createdPayment = await CreatePaypalOrderAsync(order, $"{againUrl.TrimEnd('/')}?orderId={order.Id}", $"{returnUrl.TrimEnd('/')}?orderId={order.Id}", cancellationToken);
                var approval_url = createdPayment!.links.First(x => x.rel == "approve").href;
                return approval_url;
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.Badrequest, ex);
            }
        }

        public async Task<PaymentStatus> ProcessPaymentResponse(OrderViewModel orderDetail, JObject response, CancellationToken cancellationToken)
        {
            try
            {
                if (orderDetail.PaymentStatus == PaymentStatus.Success || orderDetail.PaymentStatus == PaymentStatus.Failed)
                {
                    return orderDetail.PaymentStatus;
                }

                string? token = response.Value<string>("token");
                string? payerId = response.Value<string>("PayerID");
                var result = await CaptureOrderResult(token);
                if (string.IsNullOrEmpty(result?.id) && orderDetail.PaymentStatus == PaymentStatus.Sent)
                {
                    result = await GetOrderResult(token);
                }

                if (result is null)
                {
                    throw new MixException(MixErrorStatus.Badrequest, "Cannot get order result.");
                }

                var reference = result.purchase_units[0].reference_id;
                var status = !string.IsNullOrEmpty(reference) ? PaymentStatus.Success : PaymentStatus.Failed;
                await SaveResponse(result, status, cancellationToken);
                return status;

            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.Badrequest, ex);
            }
        }

        #region Private


        private async Task SaveResponse(PaypalOrderCapturedResponse? response, PaymentStatus paymentStatus, CancellationToken cancellationToken)
        {
            var resp = new PaypalTransactionResponse(response!);
            var vm = new PaypalTransactionResponseViewModel(resp, _cmsUow)
            {
                PaymentStatus = paymentStatus
            };
            await vm.SaveAsync(cancellationToken);
        }

        private async Task SaveOrderRequest(PaypalOrderRequest request, OrderStatus paymentStatus, CancellationToken cancellationToken)
        {
            PaypalTransactionRequest paypalRequest = new(request);
            var vm = new PaypalTransactionRequestViewModel(paypalRequest, _cmsUow);
            await vm.SaveAsync(cancellationToken);
        }

        private HttpClient GetPaypalHttpClient()
        {
            var http = new HttpClient
            {
                BaseAddress = new Uri(Settings.BaseUrl),
                Timeout = TimeSpan.FromSeconds(30),
            };

            return http;
        }

        private PayPalAccessToken? GetPayPalAccessTokenAsync()
        {
            var clientId = Settings.ClientId;
            var secret = Settings.SecretKey;

            byte[] bytes = Encoding.GetEncoding("iso-8859-1").GetBytes($"{clientId}:{secret}");

            HttpRequestMessage request = new(HttpMethod.Post, "/v1/oauth2/token");
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(bytes));

            var form = new Dictionary<string, string>
            {
                ["grant_type"] = "client_credentials"
            };

            request.Content = new FormUrlEncodedContent(form);

            HttpResponseMessage response = SendPaypalRequest(request).GetAwaiter().GetResult();
            string content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            PayPalAccessToken? accessToken = JsonConvert.DeserializeObject<PayPalAccessToken>(content);
            return accessToken;
        }

        private async Task<PaypalOrderCreatedResponse?> CreatePaypalOrderAsync(
            OrderViewModel order, string againUrl, string returnUrl,
            CancellationToken cancellationToken = default)
        {
            HttpRequestMessage request = new(HttpMethod.Post, "v2/checkout/orders");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Token.access_token);
            var payment = await GetPaymentRequestAsync(order, againUrl, returnUrl, cancellationToken);

            request.Content = new StringContent(JsonConvert.SerializeObject(payment), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await SendPaypalRequest(request);

            string content = await response.Content.ReadAsStringAsync(cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                PaypalOrderCreatedResponse? paypalPaymentCreated = JsonConvert.DeserializeObject<PaypalOrderCreatedResponse>(content);
                return paypalPaymentCreated;
            }

            throw new MixException(MixErrorStatus.Badrequest, content);
        }

        private async Task<PaypalOrderCapturedResponse?> CaptureOrderResult(string? orderId)
        {
            HttpRequestMessage request = new(HttpMethod.Post, $"v2/checkout/orders/{orderId}/capture");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Token.access_token);
            request.Content = new StringContent("", Encoding.Default, "application/json");
            HttpResponseMessage response = await SendPaypalRequest(request);
            string content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<PaypalOrderCapturedResponse>(content);

            return result;
        }

        private async Task<PaypalOrderCapturedResponse?> GetOrderResult(string? orderId)
        {
            HttpRequestMessage request = new(HttpMethod.Get, $"v2/checkout/orders/{orderId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Token.access_token);
            request.Content = new StringContent("", Encoding.Default, "application/json");
            HttpResponseMessage response = await SendPaypalRequest(request);
            string content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<PaypalOrderCapturedResponse>(content);

            return result;
        }

        private async Task<HttpResponseMessage> SendPaypalRequest(HttpRequestMessage request)
        {
            return await _httpClient.SendAsync(request);
        }
        #endregion
    }

}