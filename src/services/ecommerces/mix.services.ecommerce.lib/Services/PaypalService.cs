using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Mix.Constant.Constants;
using Mix.Heart.Enums;
using Mix.Heart.Exceptions;
using Mix.Heart.Helpers;
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
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using static Org.BouncyCastle.Math.EC.ECCurve;
using System.Net.Http.Headers;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Mix.Services.Ecommerce.Lib.Services
{
    public sealed class PaypalService : TenantServiceBase, IPaymentService
    {
        private readonly UnitOfWorkInfo<PaypalDbContext> _cmsUow;
        private readonly HttpService _httpService;
        private PaypalConfigurations _settings { get; set; } = new PaypalConfigurations();
        public PaypalService(
            IHttpContextAccessor httpContextAccessor,
            HttpService httpService,
            IConfiguration configuration,
            UnitOfWorkInfo<PaypalDbContext> cmsUow,
            MixCacheService cacheService)
            : base(httpContextAccessor, cacheService)
        {
            _httpService = httpService;
            _cmsUow = cmsUow;

            var session = configuration.GetSection(MixAppSettingsSection.Payments).GetSection("Paypal");
            session.Bind(_settings);
        }

        public JObject GetPaymentRequest(OrderViewModel order, string againUrl, string returnUrl, CancellationToken cancellationToken)
        {
            var payment = JObject.FromObject(new PaypalRequest()
            {
                intent = "sale",
                redirect_urls = new()
                {
                    return_url = returnUrl,
                    cancel_url = againUrl
                },
                payer = new() { payment_method = "paypal" },
                transactions = new()
                {
                    new()
                    {
                        amount = new()
                        {
                            total = order.Total,
                            currency = order.Currency ?? "USD"
                        }
                    }
                }
            });

            return payment;
        }

        public async Task<string?> GetPaymentUrl(OrderViewModel order, string againUrl, string returnUrl, CancellationToken cancellationToken)
        {
            try
            {
                HttpClient http = GetPaypalHttpClient();
                // Step 1: Get an access token
                PayPalAccessToken accessToken = await GetPayPalAccessTokenAsync(http);
                PayPalPaymentCreatedResponse createdPayment = await CreatePaypalPaymentAsync(order, againUrl, returnUrl, http, accessToken);
                var approval_url = createdPayment.links.First(x => x.rel == "approval_url").href;
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
                HttpClient http = GetPaypalHttpClient();
                // Step 1: Get an access token
                PayPalAccessToken accessToken = await GetPayPalAccessTokenAsync(http);
                string? paymentId = response.Value<string>("paymentId");
                string? payerId = response.Value<string>("payerId");
                var result = await ExecutePaypalPaymentAsync(http, accessToken, paymentId, payerId);
                return PaymentStatus.SUCCESS;
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.Badrequest, ex);
            }
        }

        #region Private
        private HttpClient GetPaypalHttpClient()
        {
            var http = new HttpClient
            {
                BaseAddress = new Uri(_settings.BaseUrl),
                Timeout = TimeSpan.FromSeconds(30),
            };

            return http;
        }
        private async Task<PayPalAccessToken> GetPayPalAccessTokenAsync(HttpClient http)
        {
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

            HttpResponseMessage response = await http.SendAsync(request);

            string content = await response.Content.ReadAsStringAsync();
            PayPalAccessToken accessToken = JsonConvert.DeserializeObject<PayPalAccessToken>(content);
            return accessToken;
        }

        private async Task<PayPalPaymentCreatedResponse> CreatePaypalPaymentAsync(
            OrderViewModel order, string againUrl, string returnUrl,
            HttpClient http, PayPalAccessToken accessToken,
            CancellationToken cancellationToken = default)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "v1/payments/payment");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.access_token);

            var payment = GetPaymentRequest(order, againUrl, returnUrl, cancellationToken);

            request.Content = new StringContent(JsonConvert.SerializeObject(payment), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await http.SendAsync(request);

            string content = await response.Content.ReadAsStringAsync();
            PayPalPaymentCreatedResponse paypalPaymentCreated = JsonConvert.DeserializeObject<PayPalPaymentCreatedResponse>(content);
            return paypalPaymentCreated;
        }

        private async Task<PayPalPaymentExecutedResponse> ExecutePaypalPaymentAsync(
            HttpClient http, PayPalAccessToken accessToken, string paymentId, string payerId)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"v1/payments/payment/{paymentId}/execute");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.access_token);

            var payment = JObject.FromObject(new
            {
                payer_id = payerId
            });

            request.Content = new StringContent(JsonConvert.SerializeObject(payment), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await http.SendAsync(request);
            string content = await response.Content.ReadAsStringAsync();
            PayPalPaymentExecutedResponse executedPayment = JsonConvert.DeserializeObject<PayPalPaymentExecutedResponse>(content);
            return executedPayment;
        }
        #endregion
    }

}