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

namespace Mix.Services.Ecommerce.Lib.Services
{
    public sealed class PaypalService : TenantServiceBase, IPaymentService
    {
        private readonly UnitOfWorkInfo<PaypalDbContext> _cmsUow;
        private readonly HttpClient _httpClient;
        private PaypalConfigurations _settings { get; set; } = new PaypalConfigurations();
        protected PayPalAccessToken _token { get; set; }
        public PayPalAccessToken? Token { get => GetPayPalAccessTokenAsync(); }
        public PaypalService(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            UnitOfWorkInfo<PaypalDbContext> cmsUow,
            MixCacheService cacheService)
            : base(httpContextAccessor, cacheService)
        {
            _httpClient = GetPaypalHttpClient();
            _cmsUow = cmsUow;

            var session = configuration.GetSection(MixAppSettingsSection.Payments).GetSection("Paypal");
            session.Bind(_settings);
        }

        public async Task<JObject> GetPaymentRequestAsync(
            OrderViewModel order, 
            string cancelUrl, string returnUrl, CancellationToken cancellationToken)
        {
            var request = new PaypalOrderRequest(PaypalConstants.CreateOrderIntent, order, returnUrl, cancelUrl);
            await SaveOrderRequest(request, OrderStatus.WAITING_FOR_PAYMENT, cancellationToken);
            var payment = JObject.FromObject(request);

            return payment;
        }

        public async Task<string?> GetPaymentUrl(OrderViewModel order, string againUrl, string returnUrl, CancellationToken cancellationToken)
        {
            try
            {
                var createdPayment = await CreatePaypalOrderAsync(order, againUrl, returnUrl);
                var approval_url = createdPayment.links.First(x => x.rel == "approve").href;
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
                string? token = response.Value<string>("token");
                string? payerId = response.Value<string>("PayerID");
                var result = await CaptureOrderResult(token);
                var reference = result.purchase_units[0].reference_id;
                var status = !string.IsNullOrEmpty(reference) ? PaymentStatus.SUCCESS : PaymentStatus.FAILED;
                await SaveResponse(result, status, cancellationToken);
                return status;
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.Badrequest, ex);
            }
        }

        #region Private


        private async Task SaveResponse(PaypalOrderCapturedResponse response, PaymentStatus paymentStatus, CancellationToken cancellationToken)
        {
            var resp = new PaypalTransactionResponse(response);
            var vm = new PaypalTransactionResponseViewModel(resp, _cmsUow);
            vm.PaymentStatus = paymentStatus;
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
                BaseAddress = new Uri(_settings.BaseUrl),
                Timeout = TimeSpan.FromSeconds(30),
            };

            return http;
        }

        private PayPalAccessToken GetPayPalAccessTokenAsync()
        {
            if (_token != null && _token.expires_at > DateTime.Now)
            {
                return _token;
            }

            var clientId = _settings.ClientId;
            var secret = _settings.SecretKey;

            byte[] bytes = Encoding.GetEncoding("iso-8859-1").GetBytes($"{clientId}:{secret}");

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/v1/oauth2/token");
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(bytes));

            var form = new Dictionary<string, string>
            {
                ["grant_type"] = "client_credentials"
            };

            request.Content = new FormUrlEncodedContent(form);

            HttpResponseMessage response = _httpClient.SendAsync(request).GetAwaiter().GetResult();

            string content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            PayPalAccessToken accessToken = JsonConvert.DeserializeObject<PayPalAccessToken>(content);
            return accessToken;
        }



        private async Task<PaypalOrderCreatedResponse> CreatePaypalOrderAsync(
            OrderViewModel order, string againUrl, string returnUrl,
            CancellationToken cancellationToken = default)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "v2/checkout/orders");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token.access_token);

            var payment = await GetPaymentRequestAsync(order, againUrl, returnUrl, cancellationToken);

            request.Content = new StringContent(JsonConvert.SerializeObject(payment), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.SendAsync(request);

            string content = await response.Content.ReadAsStringAsync();
            PaypalOrderCreatedResponse paypalPaymentCreated = JsonConvert.DeserializeObject<PaypalOrderCreatedResponse>(content);
            return paypalPaymentCreated;
        }

        private async Task<PaypalOrderCapturedResponse> CaptureOrderResult(string orderId)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"v2/checkout/orders/{orderId}/capture");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token.access_token);
            request.Content = new StringContent("", Encoding.Default, "application/json");
            HttpResponseMessage response = await _httpClient.SendAsync(request);
            string content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<PaypalOrderCapturedResponse>(content);

            return result;
        }
        #endregion
    }

}