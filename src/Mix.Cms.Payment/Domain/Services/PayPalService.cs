using Mix.Cms.Lib.Services;
using Mix.Cms.Payment.Domain.Constants;
using Mix.Cms.Payment.Domain.Dtos;
using Mix.Cms.Payment.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mix.Cms.Payment.Domain.Services
{
    public class PayPalService
    {
        private readonly HttpService _httpService;
        public PayPalService(HttpService httpService)
        {
            _httpService = httpService;
        }

        // Ref: https://developer.paypal.com/docs/api/get-an-access-token-postman/
        public async Task<PayPalTokenModelResponse> GetAccessTokenAsync()
        {
            try
            {
                string username = MixService.GetAuthConfig<string>(PayPalConstants.CONFIG_CLIENT_ID);
                string password = MixService.GetAuthConfig<string>(PayPalConstants.CONFIG_SECRET);
                string requestUrl = GetRequestUrl();
                List<KeyValuePair<string, string>> body = new();
                body.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
                return await _httpService.PostBasicAuthAsync<PayPalTokenModelResponse, GetPayPalTokenDto>(
                    requestUrl, body, username, password);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<PayPalCreateOrderResponse> CreateOrderAsync(PayPalCreateOrderDto dto)
        {
            var token = await GetAccessTokenAsync();
            var request = new PayPalCreateOrderRequest(dto);
            string requestUrl = GetRequestUrl(PayPalConstants.ENDPOINT_ORDER);
            return await _httpService.PostAsync<PayPalCreateOrderResponse, PayPalCreateOrderRequest>(
                    requestUrl, request, token.AccessToken);
        }

        private string GetRequestUrl(string route = null)
        {
            string domain = MixService.GetAuthConfig<string>(PayPalConstants.CONFIG_TOKEN_ENDPOINT);
            return $"{domain.TrimEnd('/')}/{route.TrimStart('/')}";
        }
    }
}
