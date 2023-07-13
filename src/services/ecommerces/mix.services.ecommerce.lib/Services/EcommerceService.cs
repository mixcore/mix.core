using Microsoft.AspNetCore.Http;
using Mix.Heart.Enums;
using Mix.Heart.Exceptions;
using Mix.Heart.Helpers;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Services;
using Mix.Services.Ecommerce.Lib.Dtos;
using Mix.Services.Ecommerce.Lib.Enums;
using Mix.Services.Ecommerce.Lib.Providers;
using Mix.Services.Ecommerce.Lib.ViewModels;
using Mix.Services.Ecommerce.Lib.Entities.Mix;
using Newtonsoft.Json.Linq;
using System.Security.Claims;
using Mix.Services.Ecommerce.Lib.Models;
using Mix.Constant.Constants;
using Microsoft.Extensions.Configuration;
using Mix.Service.Services;
using Mix.Services.Ecommerce.Lib.Interfaces;
using Mix.Lib.Interfaces;
using Mix.Heart.Services;

namespace Mix.Services.Ecommerce.Lib.Services
{
    public class EcommerceService : TenantServiceBase, IEcommerceService
    {
        protected readonly IServiceProvider _serviceProvider;

        protected readonly MixConfigurationService ConfigService;
        protected readonly TenantUserManager _userManager;
        protected readonly UnitOfWorkInfo<EcommerceDbContext> EcommerceUow;
        protected readonly PaymentConfigurationModel _paymentConfiguration = new();
        protected readonly IMixEdmService _edmService;
        protected double _exchangeRate = 1;
        protected double _extraFee = 0;

        public EcommerceService(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            UnitOfWorkInfo<EcommerceDbContext> ecommerceUow,
            TenantUserManager userManager,
            IServiceProvider serviceProvider,
            IMixEdmService edmService,
            MixCacheService cacheService,
            MixConfigurationService configService) : base(httpContextAccessor, cacheService)
        {
            EcommerceUow = ecommerceUow;
            _userManager = userManager;
            _serviceProvider = serviceProvider;

            var session = configuration.GetSection(MixAppSettingsSection.Payments);
            session.Bind(_paymentConfiguration);
            _edmService = edmService;
            ConfigService = configService;
            _exchangeRate = ConfigService.Configs.FirstOrDefault(m => m.SystemName == "exchangeRate")?.GetValue<double>() ?? 1;
            _extraFee = ConfigService.Configs.FirstOrDefault(m => m.SystemName == "extraFee")?.GetValue<double>() ?? 1;
        }

        public virtual async Task<OrderViewModel?> GetShoppingOrder(Guid userId, CancellationToken cancellationToken = default)
        {
            return await OrderViewModel
                .GetRepository(EcommerceUow, CacheService)
                .GetSingleAsync(
                    m => m.MixTenantId == CurrentTenant.Id
                         && m.OrderStatus == OrderStatus.NEW
                         && m.UserId == userId,
                    cancellationToken);
        }

        public virtual async Task<OrderViewModel> GetOrCreateShoppingOrder(ClaimsPrincipal principal, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.GetUserAsync(principal);
            if (user == null)
            {
                throw new MixException(MixErrorStatus.UnAuthorized);
            }
            var cart = await GetShoppingOrder(user.Id, cancellationToken);

            if (cart == null)
            {
                cart = new OrderViewModel(EcommerceUow)
                {
                    UserId = user.Id,
                    Title = $"{user.UserName} Cart",
                    Email = user.Email,
                    OrderStatus = OrderStatus.NEW,
                    MixTenantId = CurrentTenant.Id,
                    CreatedBy = user.UserName,
                    LastModified = DateTime.UtcNow
                };
                await cart.SaveAsync(cancellationToken);
            }
            return cart;
        }

        public virtual async Task UpdateOrderStatus(int orderId, OrderStatus orderStatus, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var order = await OrderViewModel.GetRepository(EcommerceUow, CacheService).GetSingleAsync(orderId, cancellationToken);
            if (order == null)
            {
                throw new MixException(MixErrorStatus.NotFound);
            }

            switch (orderStatus)
            {
                case OrderStatus.NEW:
                    break;
                case OrderStatus.WAITING_FOR_PAYMENT:
                    await LogAction(orderId, OrderTrackingAction.CHECKOUT);
                    break;
                case OrderStatus.CANCELED:
                    await LogAction(orderId, OrderTrackingAction.CANCELED);
                    break;
                case OrderStatus.PAID:
                    await LogAction(orderId, OrderTrackingAction.PAID);
                    break;
                case OrderStatus.SHIPPING:
                    if (order.PaymentStatus != PaymentStatus.SUCCESS)
                    {
                        throw new MixException(MixErrorStatus.Badrequest, "Cannot ship unpaid order");
                    }
                    order.OrderStatus = orderStatus;
                    await order.SaveAsync(cancellationToken);
                    await LogAction(orderId, OrderTrackingAction.SHIPPING);
                    break;
                case OrderStatus.SUCCESS:
                    await LogAction(orderId, OrderTrackingAction.SUCCESS);
                    break;
                case OrderStatus.PAYMENT_FAILED:
                    break;
                case OrderStatus.SHIPPING_FAILED:
                    break;
                default:
                    break;
            }
            order.OrderStatus = orderStatus;
            await order.SaveAsync(cancellationToken);
        }

        public virtual async Task<OrderViewModel> AddToCart(
            ClaimsPrincipal principal,
            CartItemDto item,
            CancellationToken cancellationToken = default)
        {
            var cart = await GetOrCreateShoppingOrder(principal, cancellationToken);
            var currentItem = cart.OrderItems.FirstOrDefault(m => m.Sku == item.Sku);
            if (currentItem != null)
            {
                currentItem.Quantity = item.Quantity;
                currentItem.Calculate();
            }
            else
            {
                var product = await WarehouseViewModel.GetRepository(EcommerceUow, CacheService).GetSingleAsync(
                    m => m.MixTenantId == CurrentTenant.Id && m.Sku == item.Sku,
                    cancellationToken);

                if (product == null || !product.Price.HasValue)
                {
                    throw new MixException(MixErrorStatus.Badrequest, "Invalid Product");
                }

                var orderItem = new OrderItemViewModel(EcommerceUow)
                {
                    OrderDetailId = cart.Id,
                    IsActive = true,
                    MixTenantId = CurrentTenant.Id,
                    Price = product.Price.Value,
                    Sku = item.Sku,
                    ProductId = item.ProductId
                };
                ReflectionHelper.Map(item, orderItem);
                orderItem.Calculate();
                cart.OrderItems.Add(orderItem);
            }
            await cart.SaveAsync(cancellationToken);
            return cart;
        }

        public virtual async Task<OrderViewModel> UpdateSelectedCartItem(
            ClaimsPrincipal principal,
            CartItemDto item,
            CancellationToken cancellationToken = default)
        {
            var user = await _userManager.GetUserAsync(principal);
            if (user == null)
            {
                throw new MixException(MixErrorStatus.Badrequest, "Invalid Cart");
            }

            var cart = await GetShoppingOrder(user.Id, cancellationToken);
            if (cart == null)
            {
                throw new MixException(MixErrorStatus.Badrequest, "Invalid Cart");
            }

            var currentItem = cart.OrderItems.FirstOrDefault(m => m.Sku == item.Sku);
            if (currentItem == null)
            {
                throw new MixException(MixErrorStatus.Badrequest, "Invalid Cart Item");
            }

            currentItem.IsActive = item.IsActive;
            currentItem.Calculate();

            await cart.SaveAsync(cancellationToken);
            return cart;
        }

        public virtual async Task<OrderViewModel> RemoveFromCart(
            ClaimsPrincipal principal,
            int itemId,
            CancellationToken cancellationToken = default)
        {
            var cart = await GetOrCreateShoppingOrder(principal, cancellationToken);
            var currentItem = cart.OrderItems.FirstOrDefault(m => m.Id == itemId);
            if (currentItem != null)
            {
                currentItem.SetUowInfo(EcommerceUow, CacheService);
                await currentItem.DeleteAsync(cancellationToken);
                cart.OrderItems.Remove(currentItem);
            }
            await cart.SaveAsync(cancellationToken);
            return cart;
        }

        public virtual async Task<string?> Checkout(
            ClaimsPrincipal principal,
            PaymentGateway gateway,
            OrderViewModel checkoutCart,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _userManager.GetUserAsync(principal);

            if (user == null)
            {
                throw new MixException(MixErrorStatus.UnAuthorized);
            }

            var myCart = await GetShoppingOrder(user.Id, cancellationToken);

            if (myCart == null)
            {
                throw new MixException(MixErrorStatus.Badrequest, $"user's cart cannot be null");
            }

            if (myCart.Id != checkoutCart.Id)
            {
                throw new MixException(MixErrorStatus.Badrequest, $"Invalid Cart");
            }
            checkoutCart.PaymentGateway = gateway;
            checkoutCart.MixTenantId = CurrentTenant.Id;
            checkoutCart.Email ??= user!.Email;
            await FilterCheckoutCartAsync(checkoutCart, myCart);

            var paymentService = PaymentServiceFactory.GetPaymentService(_serviceProvider, gateway);

            if (paymentService == null)
            {
                throw new MixException(MixErrorStatus.ServerError, $"Not Implement {gateway} payment");
            }

            await checkoutCart.SaveAsync(cancellationToken);
            var request = await paymentService.GetPaymentRequestAsync(checkoutCart, _paymentConfiguration.Urls.PaymentCartUrl, _paymentConfiguration.Urls.PaymentResponseUrl, cancellationToken);
            var url = await paymentService.GetPaymentUrl(checkoutCart, _paymentConfiguration.Urls.PaymentCartUrl, _paymentConfiguration.Urls.PaymentResponseUrl, cancellationToken);

            checkoutCart.PaymentRequest = request;
            await myCart.SaveAsync(cancellationToken);
            await LogAction(checkoutCart.Id, OrderTrackingAction.CHECKOUT);
            return url;
        }

        public virtual async Task<string?> CheckoutGuest(
            PaymentGateway gateway,
            OrderViewModel checkoutCart,
            CancellationToken cancellationToken = default)
        {

            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                checkoutCart.TempId = Guid.NewGuid();
                checkoutCart.MixTenantId = CurrentTenant.Id;
                checkoutCart.PaymentGateway = gateway;
                await FilterGuestCheckoutCartAsync(checkoutCart);

                var paymentService = PaymentServiceFactory.GetPaymentService(_serviceProvider, gateway);

                if (paymentService == null)
                {
                    throw new MixException(MixErrorStatus.ServerError, $"Not Implement {gateway} payment");
                }

                // Get Payment URL
                
                var id = await checkoutCart.SaveAsync(cancellationToken);
                var request = await paymentService.GetPaymentRequestAsync(checkoutCart, _paymentConfiguration.Urls.PaymentCartUrl, _paymentConfiguration.Urls.PaymentResponseUrl, cancellationToken);
                var url = await paymentService.GetPaymentUrl(checkoutCart, _paymentConfiguration.Urls.PaymentCartUrl, _paymentConfiguration.Urls.PaymentResponseUrl, cancellationToken);

                checkoutCart.PaymentRequest = request;
                await LogAction(id, OrderTrackingAction.CHECKOUT);
                return url;
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.ServerError, ex);
            }
        }


        public virtual async Task<OrderViewModel> ProcessPaymentResponse(
            int orderTempId,
            JObject paymentResponse,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var order = await OrderViewModel.GetRepository(EcommerceUow, CacheService).GetSingleAsync(m => m.Id == orderTempId, cancellationToken);
            if (order == null)
            {
                throw new MixException(MixErrorStatus.ServerError, $"Invalid Order");
            }

            var paymentService = PaymentServiceFactory.GetPaymentService(_serviceProvider, order.PaymentGateway!.Value);

            if (paymentService == null)
            {
                throw new MixException(MixErrorStatus.ServerError, $"Not Implement {order.PaymentGateway} payment");
            }

            if (order.PaymentStatus != PaymentStatus.SUCCESS && order.PaymentStatus != PaymentStatus.FAILED)
            {
                order.PaymentResponse = paymentResponse;
                order.PaymentStatus = await paymentService.ProcessPaymentResponse(order, paymentResponse, cancellationToken);
                order.LastModified = DateTime.UtcNow;
            }


            if (order.PaymentStatus == PaymentStatus.SUCCESS)
            {
                order.OrderStatus = OrderStatus.PAID;
                if (!string.IsNullOrEmpty(order.Email))
                {
                    await SendPaymentSuccessEdm(order);
                }
                await LogAction(order.Id, OrderTrackingAction.PAID);
            }
            else
            {
                await LogAction(order.Id, OrderTrackingAction.PAYMENT_FAILED);
            }
            await order.SaveAsync(cancellationToken);
            return order;
        }

        public virtual async Task LogAction(int orderId, OrderTrackingAction action, string? note = "")
        {
            OrderTrackingViewModel log = new(EcommerceUow)
            {
                OrderDetailId = orderId,
                Action = action,
                Note = note,
                MixTenantId = CurrentTenant.Id
            };
            await log.SaveAsync();
        }

        protected virtual async Task SendPaymentSuccessEdm(OrderViewModel order)
        {
            var data = ReflectionHelper.ParseObject(order);
            await _edmService.SendMailWithEdmTemplate("Payment Success", "PaymentSuccess", data, order.Email);
        }


        protected virtual async Task FilterGuestCheckoutCartAsync(OrderViewModel checkoutCart)
        {
            checkoutCart.SetUowInfo(EcommerceUow, CacheService);
            checkoutCart.LastModified = DateTime.UtcNow;
            checkoutCart.OrderStatus = OrderStatus.WAITING_FOR_PAYMENT;
            checkoutCart.CreatedDateTime = DateTime.UtcNow;
            await EcommerceUow.DbContext.SaveChangesAsync();
        }

        protected virtual async Task FilterCheckoutCartAsync(OrderViewModel checkoutCart, OrderViewModel myCart)
        {
            myCart.SetUowInfo(EcommerceUow, CacheService);
            checkoutCart.LastModified = DateTime.UtcNow;
            checkoutCart.OrderStatus = OrderStatus.WAITING_FOR_PAYMENT;
            checkoutCart.CreatedDateTime = DateTime.UtcNow;
            await EcommerceUow.DbContext.SaveChangesAsync();
        }


    }
}
