﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Mix.Constant.Constants;
using Mix.Heart.Enums;
using Mix.Heart.Exceptions;
using Mix.Heart.Helpers;
using Mix.Heart.Services;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Interfaces;
using Mix.Service.Services;
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
        private readonly HttpService _httpService;
        private OnepayConfigurations Settings { get; set; } = new OnepayConfigurations();
        public OnepayService(
            IHttpContextAccessor httpContextAccessor,
            HttpService httpService,
            IConfiguration configuration,
            UnitOfWorkInfo<OnepayDbContext> cmsUow,
            MixCacheService cacheService,
            IMixTenantService mixTenantService)
            : base(httpContextAccessor, cacheService, mixTenantService)
        {
            _httpService = httpService;
            _cmsUow = cmsUow;

            var session = configuration.GetSection(MixAppSettingsSection.Payments).GetSection("Onepay");
            session.Bind(Settings);
        }

        public async Task<string?> GetPaymentUrl(OrderViewModel order, string againUrl, string returnUrl, CancellationToken cancellationToken)
        {
            try
            {
                OnepayRequest request = new(order)
                {
                    vpc_Merchant = Settings.Merchant,
                    vpc_AccessCode = Settings.AccessCode,
                    vpc_Locale = order.Currency == "VND" ? "vn" : "us",
                    vpc_Currency = order.Currency,
                    vpc_TicketNo = HttpContextAccessor.HttpContext!.Connection.RemoteIpAddress?.ToString(),//"14.241.244.43";// 
                    AgainLink = $"{againUrl.TrimEnd('/')}?orderId={order.Id}",
                    vpc_ReturnURL = returnUrl
                };
                //request.vpc_CallbackURL = returnUrl;

                await SaveRequest(request, OrderStatus.WAITING_FOR_PAYMENT, cancellationToken);

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
            StringBuilder sb = new();
            foreach (var kvp in parameters)
            {
                if (!string.IsNullOrEmpty(kvp.Value) && kvp.Key != "vpc_SecureHash" && (kvp.Key.StartsWith("vpc_") || kvp.Key.StartsWith("user_")))
                    sb.Append(kvp.Key + "=" + kvp.Value + "&");
            }
            // remove trailing & from string
            if (sb.Length > 0)
                sb.Remove(sb.Length - 1, 1);
            // POST secureHash on string
            string hexHash = "";
            using (HMACSHA256 hasher = new(convertedHash))
            {
                byte[] hashValue = hasher.ComputeHash(Encoding.UTF8.GetBytes(sb.ToString()));//(tmp)); //
                foreach (byte b in hashValue)
                {
                    hexHash += b.ToString("X2");
                }
            }
            return hexHash;
        }

        private async Task SaveResponse(OnepayTransactionResponse response, PaymentStatus paymentStatus, CancellationToken cancellationToken)
        {
            var vm = new OnepayTransactionResponseViewModel(response, _cmsUow)
            {
                PaymentStatus = paymentStatus
            };

            await vm.SaveAsync(cancellationToken);
        }

        private async Task SaveRequest(OnepayRequest request, OrderStatus paymentStatus, CancellationToken cancellationToken)
        {
            var vm = await OnepayTransactionRequestViewModel.GetRepository(_cmsUow, CacheService).GetSingleAsync(m => m.vpc_OrderInfo == request.vpc_OrderInfo, cancellationToken);
            if (vm == null)
            {
                OnepayTransactionRequest onepayRequest = new();
                ReflectionHelper.Map(request, onepayRequest);
                vm = new OnepayTransactionRequestViewModel(onepayRequest, _cmsUow);
            }
            vm.PaymentStatus = paymentStatus;

            await vm.SaveAsync(cancellationToken);
        }

        public async Task<OnepayQueryResponse> Query(OnepayQueryRequest request)
        {
            try
            {
                string url = $"{Settings.Endpoint.TrimEnd('/')}/onecomm-pay/Vpcdps.op";
                Dictionary<string, string> parameters = ReflectionHelper.ConverObjectToDictinary(request);
                var response = await _httpService.GetAsync<OnepayQueryResponse>(url, parameters);
                return response;
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.ServerError, $"{GetType().FullName} Query: ", ex.Message);
            }
        }

        public async Task<PaymentStatus> ProcessPaymentResponse(OrderViewModel orderDetail, JObject responseObj, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var response = responseObj.ToObject<OnepayTransactionResponse>() ?? throw new MixException(MixErrorStatus.Badrequest, "Cannot convert response.");

                var paymentStatus = PaymentStatus.Success;
                if (!response.vpc_TxnResponseCode.Equals("0") && !string.IsNullOrEmpty(response.vpc_Message))
                {
                    if (!string.IsNullOrEmpty(response.vpc_SecureHash))
                    {
                        if (!Settings.SecureHashKey.Equals(response.vpc_SecureHash))
                        {
                            paymentStatus = PaymentStatus.InvalidResponse;
                        }
                    }
                    paymentStatus = PaymentStatus.Pending;
                }

                if (string.IsNullOrEmpty(response.vpc_SecureHash))
                {
                    paymentStatus = PaymentStatus.InvalidResponse;
                }
                Dictionary<string, string> parameters = ReflectionHelper.ConverObjectToDictinary(response);
                var secureHashKey = CreateSHA256Signature(parameters);
                //if (!secureHashKey.Equals(response.vpc_SecureHash, StringComparison.OrdinalIgnoreCase))
                //{
                //    orderDetail.OrderStatus = OrderStatus.PAYMENT_FAILED;
                //}

                await SaveResponse(response, paymentStatus, cancellationToken);

                return paymentStatus;
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.Badrequest, ex);
            }
        }

        public Task<JObject> GetPaymentRequestAsync(OrderViewModel order, string againUrl, string returnUrl, CancellationToken cancellationToken)
        {
            OnepayRequest request = new(order)
            {
                vpc_Merchant = Settings.Merchant,
                vpc_AccessCode = Settings.AccessCode,
                vpc_Locale = order.Currency == "VND" ? "vn" : "us",
                vpc_Currency = order.Currency,
                vpc_TicketNo = HttpContextAccessor.HttpContext!.Connection.RemoteIpAddress?.ToString(),//"14.241.244.43";// 
                AgainLink = $"{againUrl.TrimEnd('/')}?orderId={order.ExternalId}",
                vpc_ReturnURL = returnUrl
            };

            //request.vpc_CallbackURL = returnUrl;
            return Task.FromResult(ReflectionHelper.ParseObject(request));
        }
    }
}
