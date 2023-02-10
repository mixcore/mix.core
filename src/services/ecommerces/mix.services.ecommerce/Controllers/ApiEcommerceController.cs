using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Mix.Constant.Constants;
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
using Mix.Services.Ecommerce.Lib.Models;
using Mix.Services.Ecommerce.Lib.Services;
using Mix.Services.Ecommerce.Lib.ViewModels;
using Mix.Services.Payments.Lib.Constants;
using Mix.Shared.Dtos;
using Newtonsoft.Json.Linq;
using System.Web;

namespace Mix.Services.ecommerce.Controllers
{
    [Route("api/v2/ecommerce")]
    [ApiController]
    public class ApiEcommerceController : MixTenantApiControllerBase
    {
        private readonly PaymentConfigurationModel _paymentConfiguration = new();
        private readonly EcommerceService _ecommerceService;
        private readonly OrderService _orderService;
        protected UnitOfWorkInfo<MixCmsContext> _cmsUOW;
        public ApiEcommerceController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration, 
            MixService mixService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            IQueueService<MessageQueueModel> queueService,
            EcommerceService ecommerceService, UnitOfWorkInfo<MixCmsContext> cmsUOW, OrderService orderService)
            : base(httpContextAccessor, configuration, mixService, translator, mixIdentityService, queueService)
        {
            _ecommerceService = ecommerceService;
            _cmsUOW = cmsUOW;
            _orderService = orderService;
            var session = configuration.GetSection(MixAppSettingsSection.Payments);
            session.Bind(_paymentConfiguration);
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
        public async Task<ActionResult<OrderViewModel>> GetMyOrders([FromQuery] FilterOrderDto request, CancellationToken cancellationToken = default)
        {
            var cart = await _orderService.GetUserOrders(User, request, cancellationToken);
            return Ok(cart);
        }
        
        [MixAuthorize]
        [HttpGet]
        [Route("my-orders/{id}")]
        public async Task<ActionResult<OrderViewModel>> GetMyOrders(int id, CancellationToken cancellationToken = default)
        {
            var order = await _orderService.GetUserOrder(User, id, cancellationToken);
            return Ok(order);
        }
        
        [MixAuthorize]
        [HttpGet]
        [Route("cancel-order/{id}")]
        public async Task<ActionResult<OrderViewModel>> CancelOrder(int id, CancellationToken cancellationToken = default)
        {
            await _orderService.CancelOrder(User, id, cancellationToken);
            return Ok();
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
        [HttpPost]
        [Route("selected-cart-item")]
        public async Task<ActionResult<OrderViewModel>> SelectedCartItem(CartItemDto item, CancellationToken cancellationToken = default)
        {
            var cart = await _ecommerceService.UpdateSelectedCartItem(User, item, cancellationToken);
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
        [HttpPost]
        [Route("checkout/{gateway}")]
        public async Task<ActionResult<JObject>> Checkout(PaymentGateway? gateway, [FromBody] OrderViewModel cart, CancellationToken cancellationToken = default)
        {
            if (gateway == null)
            {
                return BadRequest();
            }
            var url = await _ecommerceService.Checkout(User, gateway.Value, cart, cancellationToken);
            return !string.IsNullOrEmpty(url) ? Ok(new JObject(new JProperty("url", url))) : BadRequest();
        }

        [HttpGet]
        [Route("payment-response/{orderId}")]
        public async Task<ActionResult> PaymentResponse(int orderId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(Request.QueryString.Value))
            {
                return BadRequest();

            }
            var query = HttpUtility.ParseQueryString(Request.QueryString.Value);
            var paymentResponse = JObject.FromObject(query!.AllKeys.ToDictionary(k => k, k => query[k]));
            
            var result = await _ecommerceService.ProcessPaymentResponse(orderId, paymentResponse, cancellationToken);
            string url =
            result == OrderStatus.PAID
                ? $"{_paymentConfiguration.Urls.PaymentSuccessUrl}?id={orderId}"
                : $"{_paymentConfiguration.Urls.PaymentFailUrl}?id={orderId}";
            return Redirect(url);
        }

        #endregion
    }
}
