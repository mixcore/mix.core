using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Mix.Database.Entities.Cms;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Attributes;
using Mix.Lib.Base;
using Mix.Lib.Models.Common;
using Mix.Lib.Services;
using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Mix.Service.Services;
using Mix.Services.Ecommerce.Lib.Dtos;
using Mix.Services.Ecommerce.Lib.Entities.Mix;
using Mix.Services.Ecommerce.Lib.Enums;
using Mix.Services.Ecommerce.Lib.Services;
using Mix.Services.Ecommerce.Lib.ViewModels;
using Mix.Shared.Dtos;
using Newtonsoft.Json.Linq;

namespace mix.services.ecommerce.Controllers
{
    [Route("api/v2/ecommerce")]
    [ApiController]
    public class ApiEcommerceController : MixTenantApiControllerBase
    {
        private readonly EcommerceService _ecommerceService;
        private readonly OrderService _orderService;
        protected UnitOfWorkInfo<MixCmsContext> _cmsUOW;
        public ApiEcommerceController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration, MixService mixService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            IQueueService<MessageQueueModel> queueService,
            EcommerceService ecommerceService, UnitOfWorkInfo<MixCmsContext> cmsUOW, OrderService orderService)
            : base(httpContextAccessor, configuration, mixService, translator, mixIdentityService, queueService)
        {
            _ecommerceService = ecommerceService;
            _cmsUOW = cmsUOW;
            _orderService = orderService;
        }

        #region Routes

        [MixAuthorize]
        [HttpGet]
        [Route("my-shopping-cart")]
        public async Task<ActionResult<OrderViewModel>> MyShoppingOrder(CancellationToken cancellationToken = default)
        {
            var cart = await _ecommerceService.GetOrCreateShoppingOrder(User, cancellationToken);
            return Ok(cart);
        }
        
        [MixAuthorize]
        [HttpGet]
        [Route("my-orders")]
        public async Task<ActionResult<OrderViewModel>> GetMyOrders([FromQuery] SearchRequestDto request, CancellationToken cancellationToken = default)
        {
            var cart = await _orderService.GetUserOrders(User, request, cancellationToken);
            return Ok(cart);
        }

        [MixAuthorize]
        [HttpPost]
        [Route("add-to-cart")]
        public async Task<ActionResult<OrderViewModel>> AddToCart(CartItemDto item, CancellationToken cancellationToken = default)
        {
            var cart = await _ecommerceService.AddToCart(User, item, cancellationToken);
            return Ok(cart);
        }

        [MixAuthorize]
        [HttpDelete]
        [Route("remove-from-cart/{itemId}")]
        public async Task<ActionResult> RemoveFromCart(int itemId, CancellationToken cancellationToken = default)
        {
            var cart = await _ecommerceService.RemoveFromCart(User, itemId, cancellationToken);
            return Ok(cart);
        }

        [MixAuthorize]
        [HttpGet]
        [Route("payment")]
        public async Task<ActionResult<string>> Checkout([FromQuery] PaymentGateway? gateway, CancellationToken cancellationToken = default)
        {
            if (gateway == null || string.IsNullOrEmpty(Request.QueryString.Value))
            {
                return BadRequest();

            }
            var url = await _ecommerceService.GetPaymentUrl(User, gateway.Value, cancellationToken);
            return !string.IsNullOrEmpty(url) ? Redirect(url) : BadRequest();
        }

        [HttpGet]
        [Route("payment-response/{gateway}")]
        public async Task<ActionResult<string>> PaymentResponse(PaymentGateway? gateway, CancellationToken cancellationToken = default)
        {
            if (gateway == null || string.IsNullOrEmpty(Request.QueryString.Value))
            {
                return BadRequest();

            }
            var paymentResponse = JObject.FromObject(QueryHelpers.ParseQuery(Request.QueryString.Value));
            await _ecommerceService.ProcessPaymentResponse(gateway.Value, paymentResponse, cancellationToken);
            return Ok();
        }

        #endregion
    }
}
