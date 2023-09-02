using Microsoft.AspNetCore.Mvc;
using Mix.Constant.Constants;
using Mix.Database.Entities.Cms;
using Mix.Heart.Services;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Attributes;
using Mix.Lib.Base;
using Mix.Lib.Interfaces;
using Mix.Lib.Services;
using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Mix.Service.Services;
using Mix.Services.Ecommerce.Lib.Dtos;
using Mix.Services.Ecommerce.Lib.Enums;
using Mix.Services.Ecommerce.Lib.Interfaces;
using Mix.Services.Ecommerce.Lib.Models;
using Mix.Services.Ecommerce.Lib.ViewModels;
using Mix.SignalR.Interfaces;
using Mix.SignalR.Models;
using MySqlX.XDevAPI.Common;
using Newtonsoft.Json.Linq;
using System.Web;

namespace Mix.Services.ecommerce.Controllers
{
    [Route("api/v2/mix/ecommerce")]
    [ApiController]
    public class ApiEcommerceController : MixTenantApiControllerBase
    {
        private readonly PaymentConfigurationModel _paymentConfiguration = new();
        private readonly IPortalHubClientService _portalHub;
        private readonly IEcommerceService _ecommerceService;
        private readonly IOrderService _orderService;
        protected UnitOfWorkInfo<MixCmsContext> CmsUow;

        public ApiEcommerceController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixCacheService cacheService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            IQueueService<MessageQueueModel> queueService,
            IEcommerceService ecommerceService,
            UnitOfWorkInfo<MixCmsContext> cmsUow,
            IOrderService orderService,
            IPortalHubClientService portalHub,
            IMixTenantService mixTenantService)
            : base(httpContextAccessor, configuration,
                  cacheService, translator, mixIdentityService, queueService, mixTenantService)
        {
            _ecommerceService = ecommerceService;
            CmsUow = cmsUow;
            _orderService = orderService;
            var session = configuration.GetSection(MixAppSettingsSection.Payments);
            session.Bind(_paymentConfiguration);
            _portalHub = portalHub;
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
        [Route("guest-order/{id}")]
        public async Task<ActionResult<OrderViewModel>> GetGuestOrder(int id, CancellationToken cancellationToken = default)
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

        [HttpPost]
        [Route("checkout-guest/{gateway}")]
        public async Task<ActionResult<JObject>> CheckoutGuest(PaymentGateway? gateway, [FromBody] OrderViewModel cart, CancellationToken cancellationToken = default)
        {
            if (gateway == null)
            {
                return BadRequest();
            }
            var url = await _ecommerceService.CheckoutGuest(gateway.Value, cart, cancellationToken);
            QueueService.PushQueue(CurrentTenant.Id, MixQueueTopics.MixBackgroundTasks, MixQueueActions.PlacedOrder, cart);
            return !string.IsNullOrEmpty(url) ? Ok(new JObject(new JProperty("url", url))) : BadRequest();
        }

        [HttpGet]
        [Route("payment-response")]
        public async Task<ActionResult> PaymentResponse([FromQuery] int? orderId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(Request.QueryString.Value))
            {
                return BadRequest();

            }
            var query = HttpUtility.ParseQueryString(Request.QueryString.Value);
            if (query is null || query.AllKeys is null)
            {
                return BadRequest();
            }

            var paymentResponse = JObject.FromObject(query.AllKeys.ToDictionary(k => k, k => query[k]));
            orderId ??= paymentResponse.Value<int?>("vpc_OrderInfo");
            if (!orderId.HasValue)
            {
                return BadRequest();
            }

            var result = await _ecommerceService.ProcessPaymentResponse(orderId.Value, paymentResponse, cancellationToken);
            string url =
            result.OrderStatus == OrderStatus.PAID
                ? $"{_paymentConfiguration.Urls.PaymentSuccessUrl}?id={orderId}"
                : $"{_paymentConfiguration.Urls.PaymentFailUrl}?id={orderId}";

            QueueService.PushQueue(CurrentTenant.Id, MixQueueTopics.MixBackgroundTasks, MixQueueActions.PaymentResponse, result);
            return Redirect(url);
        }

        #endregion
    }
}
