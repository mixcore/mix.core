using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Mix.Database.Entities.Cms;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Attributes;
using Mix.Lib.Base;
using Mix.Lib.Services;
using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Mix.Service.Services;
using Mix.Services.Payments.Lib.Dtos;
using Mix.Services.Payments.Lib.Enums;
using Mix.Services.Payments.Lib.Services;
using Mix.Services.Payments.Lib.ViewModels.Mix;
using Newtonsoft.Json.Linq;

namespace mix.services.ecommerce.Controllers
{
    [Route("api/ecommerce")]
    [ApiController]
    public class ApiEcommerceController : MixTenantApiControllerBase
    {
        private readonly EcommerceService _ecommerceService;
        protected UnitOfWorkInfo<MixCmsContext> _cmsUOW;
        public ApiEcommerceController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration, MixService mixService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            IQueueService<MessageQueueModel> queueService,
            EcommerceService ecommerceService, UnitOfWorkInfo<MixCmsContext> cmsUOW)
            : base(httpContextAccessor, configuration, mixService, translator, mixIdentityService, queueService)
        {
            _ecommerceService = ecommerceService;
            _cmsUOW = cmsUOW;
        }

        #region Routes

        [MixAuthorize]
        [HttpGet]
        [Route("my-shopping-cart")]
        public async Task<ActionResult<OrderViewModel>> MyShoppingOrder()
        {
            var cart = await _ecommerceService.GetOrCreateShoppingOrder(User, CancellationToken);
            return Ok(cart);
        }

        [MixAuthorize]
        [HttpPost]
        [Route("add-to-cart")]
        public async Task<ActionResult<OrderViewModel>> AddToCart(CartItemDto item)
        {
            var cart = await _ecommerceService.AddToCart(User, item, CancellationToken);
            return Ok(cart);
        }

        [MixAuthorize]
        [HttpDelete]
        [Route("remove-from-cart/{postId}")]
        public async Task<ActionResult> RemoveFromCart(int postId)
        {
            var cart = await _ecommerceService.RemoveFromCart(User, postId, CancellationToken);
            return Ok(cart);
        }

        [MixAuthorize]
        [HttpGet]
        [Route("payment-url/{gateway}")]
        public async Task<ActionResult<string>> Checkout(PaymentGateway gateway)
        {
            var url = await _ecommerceService.GetPaymentUrl(User, gateway, CancellationToken);
            return !string.IsNullOrEmpty(url) ? Ok(url) : BadRequest();
        }

        [HttpGet]
        [Route("payment-response/{gateway}")]
        public async Task<ActionResult<string>> PaymentResponse(PaymentGateway gateway)
        {
            if (!string.IsNullOrEmpty(Request.QueryString.Value))
            {
                var paymentResponse = JObject.FromObject(QueryHelpers.ParseQuery(Request.QueryString.Value));
                await _ecommerceService.ProcessPaymentResponse(gateway, paymentResponse, CancellationToken);
                return Ok();
            }
            return BadRequest();
        }

        #endregion
    }
}
