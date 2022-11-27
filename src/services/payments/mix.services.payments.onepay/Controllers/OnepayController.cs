using Google.Api;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mix.Lib.Base;
using Mix.Lib.Services;
using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Mix.Service.Services;
using Mix.Services.Payments.Onepay.Domain.Entities;
using Mix.Services.Payments.Onepay.Domain.Models;
using Mix.Services.Payments.Onepay.Domain.Services;
using Mix.Shared.Services;

namespace mix.services.payments.onepay.Controllers
{
    [ApiController]
    [Route("onepay")]
    public class OnepayController : MixTenantApiControllerBase
    {
        private readonly OnepayService _onepayService;
        public OnepayController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixService mixService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            IQueueService<MessageQueueModel> queueService,
            OnepayService onepayService)
            : base(httpContextAccessor, configuration, mixService, translator, mixIdentityService, queueService)
        {
            _onepayService = onepayService;
        }

        [HttpGet("payment")]
        public async Task<ActionResult<OnepayTransactionResponse>> Payment([FromQuery] PaymentRequest request)
        {
            request.AgainLink = HttpContext.Request.GetEncodedUrl();
            request.vpc_TicketNo =  string.IsNullOrEmpty(request.vpc_TicketNo)
                                        ? Request.HttpContext.Connection.RemoteIpAddress!.ToString()
                                        : request.vpc_TicketNo;
            request.vpc_ReturnURL = System.Net.WebUtility.UrlEncode(request.vpc_ReturnURL);
            var result = await _onepayService.GetPaymentUrl(request);
            return result != null ? Redirect(result) : BadRequest();
        }
        
        [HttpGet("payment-response")]
        public async Task<ActionResult<OnepayTransactionResponse>> PaymentResponse([FromQuery] PaymentResponse response)
        {
            //request.vpc_TicketNo = "104.28.237.72";//Request.HttpContext.Connection.RemoteIpAddress!.ToString();
            //request.AgainLink = System.Net.WebUtility.UrlEncode($"https://{CurrentTenant.PrimaryDomain}/{request.vpc_OrderInfo}");
            //request.vpc_ReturnURL = System.Net.WebUtility.UrlEncode($"https://2fad-14-187-38-0.ap.ngrok.io/api/onepay/payment-response");
            //var result = await _onepayService.GetPaymentUrl(request);
            return Ok(response);
        }
    }
}