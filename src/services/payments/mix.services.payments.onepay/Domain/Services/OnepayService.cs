using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Mix.Constant.Constants;
using Mix.Database.Entities.Cms;
using Mix.Heart.Entities;
using Mix.Heart.Enums;
using Mix.Heart.Exceptions;
using Mix.Heart.Helpers;
using Mix.Heart.UnitOfWork;
using Mix.Service.Services;
using Mix.Services.Payments.Onepay.Domain.Entities;
using Mix.Services.Payments.Onepay.Domain.Enums;
using Mix.Services.Payments.Onepay.Domain.Models;
using Mix.Services.Payments.Onepay.Domain.ViewModels;
using Mix.Shared.Services;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Mix.Services.Payments.Onepay.Domain.Services
{
    public sealed class OnepayService
    {
        private readonly UnitOfWorkInfo<MixCmsContext> _cmsUOW;
        private readonly HttpService _httpService;
        private MixOnepayConfigurations _settings { get; set; } = new MixOnepayConfigurations();
        public OnepayService(HttpService httpService, IConfiguration configuration, UnitOfWorkInfo<MixCmsContext> cmsUOW)
        {
            _httpService = httpService;
            _cmsUOW = cmsUOW;
            var session = configuration.GetSection(MixAppSettingsSection.Payments).GetSection("Onepay");
            session.Bind(_settings);
        }

        public async Task<string?> GetPaymentUrl(PaymentRequest request)
        {
            request.vpc_Merchant = _settings.Merchant;
            request.vpc_AccessCode = _settings.AccessCode;
            request.vpc_Locale = _settings.Locale;

            OnepayTransactionRequest onepayRequest = new();
            ReflectionHelper.MapObject(request, onepayRequest);

            var response = ParseRequestUrl(request);
            //await SaveRequest(onepayRequest, OnepayPaymentStatus.SENT);
            return response;
        }

        private string ParseRequestUrl(PaymentRequest request)
        {
            try
            {
                Dictionary<string, string> parameters = ReflectionHelper.ConverObjectToDictinary(request);
                parameters["vpc_SecureHash"] = CreateSHA256Signature(parameters);
                string query = string.Join("&", 
                                parameters
                                .Where(m => !string.IsNullOrEmpty(m.Value))
                                .Select(x => $"{x.Key}={x.Value}"));
                return $"{_settings.PaymentEndpoint}?{query}";
            }
            catch (Exception ex)
            {
                MixService.LogException(ex);
                return default;
            }
        }

        public string CreateSHA256Signature(Dictionary<string, string> parameters)
        {
            // Hex Decode the Secure Secret for use in using the HMACSHA256 hasher
            // hex decoding eliminates this source of error as it is independent of the character encoding
            // hex decoding is precise in converting to a byte array and is the preferred form for representing binary values as hex strings. 
            byte[] convertedHash = new byte[_settings.SecureHashKey.Length / 2];
            for (int i = 0; i < _settings.SecureHashKey.Length / 2; i++)
            {
                convertedHash[i] = (byte)Int32.Parse(_settings.SecureHashKey.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber);
            }

            // Build string from collection in preperation to be hashed
            StringBuilder sb = new StringBuilder();
            foreach (var kvp in parameters)
            {
                if (kvp.Key.StartsWith("vpc_") || kvp.Key.StartsWith("user_"))
                    sb.Append(kvp.Key + "=" + kvp.Value + "&");
            }
            // remove trailing & from string
            if (sb.Length > 0)
                sb.Remove(sb.Length - 1, 1);

            // Create secureHash on string
            string hexHash = "";
            using (HMACSHA256 hasher = new HMACSHA256(convertedHash))
            {
                byte[] hashValue = hasher.ComputeHash(Encoding.UTF8.GetBytes(sb.ToString()));
                foreach (byte b in hashValue)
                {
                    hexHash += b.ToString("X2");
                }
            }
            return hexHash;
        }

        private async Task SaveResponse(OnepayTransactionResponse response, OnepayPaymentStatus paymentStatus)
        {
            var vm = new OnepayTransactionResponseViewModel(response, _cmsUOW);
            vm.PaymentStatus = paymentStatus;
            await vm.SaveAsync();
        }

        private async Task SaveRequest(OnepayTransactionRequest request, OnepayPaymentStatus paymentStatus)
        {
            var vm = new OnepayTransactionRequestViewModel(request, _cmsUOW);
            vm.PaymentStatus = paymentStatus;
            await vm.SaveAsync();
        }

        public async Task<PaymentQueryResponse> Query(PaymentQueryRequest request)
        {
            try
            {
                string url = $"{_settings.Endpoint.TrimEnd('/')}/onecomm-pay/Vpcdps.op";
                Dictionary<string, string> parameters = ReflectionHelper.ConverObjectToDictinary(request);
                var response = await _httpService.GetAsync<PaymentQueryResponse>(url, parameters);
                return response;
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.ServerError, $"{GetType().FullName} Query: ", ex.Message);
            }
        }

        public OnepayPaymentStatus ProcessPaymentResponse(OnepayTransactionResponse response)
        {
            if (!response.vpc_TxnResponseCode.Equals("0") && !string.IsNullOrEmpty(response.vpc_Message))
            {
                if (!string.IsNullOrEmpty(response.vpc_SecureHash))
                {
                    if (!_settings.SecureHashKey.Equals(response.vpc_SecureHash))
                    {
                        return OnepayPaymentStatus.INVALIDRESPONSE;
                    }
                }
                return OnepayPaymentStatus.PENDING;
            }

            if (string.IsNullOrEmpty(response.vpc_SecureHash))
            {
                return OnepayPaymentStatus.INVALIDRESPONSE;
            }
            if (!_settings.SecureHashKey.Equals(response.vpc_SecureHash))
            {
                return OnepayPaymentStatus.INVALIDRESPONSE;
            }
            return OnepayPaymentStatus.SUCCESS;
        }
    }
}
