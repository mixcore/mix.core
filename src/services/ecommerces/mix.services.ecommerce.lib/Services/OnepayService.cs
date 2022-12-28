using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Mix.Constant.Constants;
using Mix.Heart.Enums;
using Mix.Heart.Exceptions;
using Mix.Heart.Helpers;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Base;
using Mix.Lib.Services;
using Mix.Services.Ecommerce.Lib.Entities.Mix;
using Mix.Services.Ecommerce.Lib.Entities.Onepay;
using Mix.Services.Ecommerce.Lib.Enums;
using Mix.Services.Ecommerce.Lib.Interfaces;
using Mix.Services.Ecommerce.Lib.Models.Onepay;
using Mix.Services.Ecommerce.Lib.ViewModels;
using Mix.Services.Ecommerce.Lib.ViewModels.Onepay;
using Mix.Shared.Services;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Mix.Services.Ecommerce.Lib.Services
{
    public sealed class OnepayService : TenantServiceBase, IPaymentService
    {
        private readonly UnitOfWorkInfo<OnepayDbContext> _cmsUow;
        private readonly UnitOfWorkInfo<EcommerceDbContext> _ecommerceUow;
        private readonly HttpService _httpService;
        private readonly MixEdmService _edmService;
        private MixOnepayConfigurations Settings { get; set; } = new MixOnepayConfigurations();
        public OnepayService(IHttpContextAccessor httpContextAccessor, HttpService httpService, IConfiguration configuration, UnitOfWorkInfo<OnepayDbContext> cmsUow, UnitOfWorkInfo<EcommerceDbContext> ecommerceUow, MixEdmService edmService)
            : base(httpContextAccessor)
        {
            _httpService = httpService;
            _cmsUow = cmsUow;
            _ecommerceUow = ecommerceUow;
            _edmService = edmService;

            var session = configuration.GetSection(MixAppSettingsSection.Payments).GetSection("Onepay");
            session.Bind(Settings);
        }

        public async Task<string?> GetPaymentUrl(OrderViewModel request, string returnUrl, CancellationToken cancellationToken)
        {
            return await ParseRequestUrlAsync(request, returnUrl, cancellationToken);
        }

        private async Task<string?> ParseRequestUrlAsync(OrderViewModel order, string returnUrl, CancellationToken cancellationToken)
        {
            try
            {
                PaymentRequest request = new PaymentRequest(order);
                request.vpc_Merchant = Settings.Merchant;
                request.vpc_AccessCode = Settings.AccessCode;
                request.vpc_Locale = Settings.Locale;
                request.vpc_TicketNo = HttpContextAccessor.HttpContext!.Connection.RemoteIpAddress?.ToString();//"14.241.244.43";// 
                request.AgainLink = System.Net.WebUtility.UrlEncode(returnUrl);
                request.vpc_ReturnURL = System.Net.WebUtility.UrlEncode(returnUrl);

                await SaveRequest(request, OrderStatus.PENDING, cancellationToken);

                Dictionary<string, string> parameters = ReflectionHelper.ConverObjectToDictinary(request);
                parameters["vpc_SecureHash"] = CreateSHA256Signature(parameters);
                string query = string.Join("&",
                                parameters
                                .Where(m => !string.IsNullOrEmpty(m.Value))
                                .Select(x => $"{x.Key}={x.Value}"));
                return $"{Settings.PaymentEndpoint}?{query}";
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.ServerError, ex);
            }
        }

        public string CreateSHA256Signature(Dictionary<string, string> parameters)
        {
            // Hex Decode the Secure Secret for use in using the HMACSHA256 hasher
            // hex decoding eliminates this source of error as it is independent of the character encoding
            // hex decoding is precise in converting to a byte array and is the preferred form for representing binary values as hex strings. 
            byte[] convertedHash = new byte[Settings.SecureHashKey.Length / 2];
            for (int i = 0; i < Settings.SecureHashKey.Length / 2; i++)
            {
                convertedHash[i] = (byte)int.Parse(Settings.SecureHashKey.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber);
            }

            // Build string from collection in preperation to be hashed
            StringBuilder sb = new StringBuilder();
            foreach (var kvp in parameters)
            {
                if (!string.IsNullOrEmpty(kvp.Value) && (kvp.Key.StartsWith("vpc_") || kvp.Key.StartsWith("user_")))
                    sb.Append(kvp.Key + "=" + kvp.Value + "&");
            }
            // remove trailing & from string
            if (sb.Length > 0)
                sb.Remove(sb.Length - 1, 1);
            //string tmp = "vpc_AccessCode=6BEB2566&vpc_Amount=2490000&vpc_Command=pay&vpc_Currency=VND&vpc_Locale=vn&vpc_MerchTxnRef=638073070064044677&vpc_Merchant=TESTONEPAY30&vpc_OrderInfo=ebd10af0-2e73-40fc-b975-c05c506d0da6_3&vpc_ReturnURL=https//nesto.tanconstructions.com.au/checkout/Onepay&vpc_TicketNo=172.70.142.147&vpc_Version=2";
            // Create secureHash on string
            string hexHash = "";
            using (HMACSHA256 hasher = new HMACSHA256(convertedHash))
            {
                byte[] hashValue = hasher.ComputeHash(Encoding.UTF8.GetBytes(sb.ToString()));//(tmp)); //
                foreach (byte b in hashValue)
                {
                    hexHash += b.ToString("X2");
                }
            }
            return hexHash;
        }

        private async Task SaveResponse(OnepayTransactionResponse response, OrderStatus paymentStatus, CancellationToken cancellationToken)
        {
            var vm = new OnepayTransactionResponseViewModel(response, _cmsUow);
            vm.PaymentStatus = paymentStatus;
            await vm.SaveAsync(cancellationToken);
        }

        private async Task SaveRequest(PaymentRequest request, OrderStatus paymentStatus, CancellationToken cancellationToken)
        {
            var vm = await OnepayTransactionRequestViewModel.GetRepository(_cmsUow).GetSingleAsync(m => m.vpc_OrderInfo == request.vpc_OrderInfo);
            if (vm == null)
            {
                OnepayTransactionRequest onepayRequest = new();
                ReflectionHelper.MapObject(request, onepayRequest);
                vm = new OnepayTransactionRequestViewModel(onepayRequest, _cmsUow);
            }
            vm.PaymentStatus = paymentStatus;

            await vm.SaveAsync(cancellationToken);
        }

        public async Task<PaymentQueryResponse> Query(PaymentQueryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                string url = $"{Settings.Endpoint.TrimEnd('/')}/onecomm-pay/Vpcdps.op";
                Dictionary<string, string> parameters = ReflectionHelper.ConverObjectToDictinary(request);
                var response = await _httpService.GetAsync<PaymentQueryResponse>(url, parameters);
                return response;
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.ServerError, $"{GetType().FullName} Query: ", ex.Message);
            }
        }

        public async Task<OrderStatus> ProcessPaymentResponse(JObject responseObj, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var response = responseObj.ToObject<OnepayTransactionResponse>();
                var orderDetail = await OrderViewModel.GetRepository(_ecommerceUow).GetSingleAsync(m => m.Id == int.Parse(response.vpc_OrderInfo));

                if (orderDetail == null)
                {
                    throw new MixException($"Invalid payment response {response.vpc_OrderInfo}");
                }

                if (!response.vpc_TxnResponseCode.Equals("0") && !string.IsNullOrEmpty(response.vpc_Message))
                {
                    if (!string.IsNullOrEmpty(response.vpc_SecureHash))
                    {
                        if (!Settings.SecureHashKey.Equals(response.vpc_SecureHash))
                        {
                            orderDetail.OrderStatus = OrderStatus.INVALIDRESPONSE;
                        }
                    }
                    orderDetail.OrderStatus = OrderStatus.PENDING;
                }

                if (string.IsNullOrEmpty(response.vpc_SecureHash))
                {
                    orderDetail.OrderStatus = OrderStatus.INVALIDRESPONSE;
                }
                if (!Settings.SecureHashKey.Equals(response.vpc_SecureHash))
                {
                    orderDetail.OrderStatus = OrderStatus.INVALIDRESPONSE;
                }
                await SaveResponse(response, orderDetail.OrderStatus, cancellationToken);
                await orderDetail.SaveAsync(cancellationToken);
                if (orderDetail.OrderStatus == OrderStatus.SUCCESS)
                {
                    await _edmService.SendMailWithEdmTemplate("Payment Success", "PaymentSuccess", JObject.FromObject(orderDetail), orderDetail.Email);
                }
                return orderDetail.OrderStatus;
            }
            catch(Exception ex)
            {
                throw new MixException(MixErrorStatus.Badrequest, ex);
            }
        }

    }
}
