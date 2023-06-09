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
    public sealed class EcommerceService : TenantServiceBase, IEcommerceService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly TenantUserManager _userManager;
        private readonly UnitOfWorkInfo<EcommerceDbContext> _uow;
        private readonly PaymentConfigurationModel _paymentConfiguration = new();
        private readonly IMixEdmService _edmService;
        public EcommerceService(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            UnitOfWorkInfo<EcommerceDbContext> uow,
            TenantUserManager userManager,
            IServiceProvider serviceProvider,
            IMixEdmService edmService,
            MixCacheService cacheService) : base(httpContextAccessor, cacheService)
        {
            _uow = uow;
            _userManager = userManager;
            _serviceProvider = serviceProvider;

            var session = configuration.GetSection(MixAppSettingsSection.Payments);
            session.Bind(_paymentConfiguration);
            _edmService = edmService;
        }

        public async Task<OrderViewModel?> GetShoppingOrder(Guid userId, CancellationToken cancellationToken = default)
        {
            return await OrderViewModel
                .GetRepository(_uow, CacheService)
                .GetSingleAsync(
                    m => m.MixTenantId == CurrentTenant.Id
                         && m.OrderStatus == OrderStatus.NEW
                         && m.UserId == userId,
                    cancellationToken);
        }
        
        public async Task<OrderViewModel> GetOrCreateShoppingOrder(ClaimsPrincipal principal, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.GetUserAsync(principal);
            if (user == null)
            {
                throw new MixException(MixErrorStatus.UnAuthorized);
            }
            var cart = await GetShoppingOrder(user.Id, cancellationToken);

            if (cart == null)
            {
                cart = new OrderViewModel(_uow)
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

        public async Task UpdateOrderStatus(int orderId, OrderStatus orderStatus, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var order = await OrderViewModel.GetRepository(_uow, CacheService).GetSingleAsync(orderId, cancellationToken);
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

        public async Task<OrderViewModel> AddToCart(
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
                var product = await WarehouseViewModel.GetRepository(_uow, CacheService).GetSingleAsync(
                    m => m.MixTenantId == CurrentTenant.Id && m.Sku == item.Sku,
                    cancellationToken);

                if (product == null || !product.Price.HasValue)
                {
                    throw new MixException(MixErrorStatus.Badrequest, "Invalid Product");
                }

                var orderItem = new OrderItemViewModel(_uow)
                {
                    OrderDetailId = cart.Id,
                    IsActive = true,
                    MixTenantId = CurrentTenant.Id,
                    Price = product.Price.Value
                };
                ReflectionHelper.Map(item, orderItem);
                orderItem.Calculate();
                cart.OrderItems.Add(orderItem);
            }
            await cart.SaveAsync(cancellationToken);
            return cart;
        }

        public async Task<OrderViewModel> UpdateSelectedCartItem(
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

        public async Task<OrderViewModel> RemoveFromCart(
            ClaimsPrincipal principal,
            int itemId,
            CancellationToken cancellationToken = default)
        {
            var cart = await GetOrCreateShoppingOrder(principal, cancellationToken);
            var currentItem = cart.OrderItems.FirstOrDefault(m => m.Id == itemId);
            if (currentItem != null)
            {
                currentItem.SetUowInfo(_uow, CacheService);
                await currentItem.DeleteAsync(cancellationToken);
                cart.OrderItems.Remove(currentItem);
            }
            await cart.SaveAsync(cancellationToken);
            return cart;
        }

        public async Task<string?> Checkout(
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
            checkoutCart.Email ??= user!.Email;
            await FilterCheckoutCartAsync(checkoutCart, myCart);

            var paymentService = PaymentServiceFactory.GetPaymentService(_serviceProvider, gateway);

            if (paymentService == null)
            {
                throw new MixException(MixErrorStatus.ServerError, $"Not Implement {gateway} payment");
            }

            string returnUrl = $"{_paymentConfiguration.Urls.PaymentResponseUrl}/{checkoutCart.Id}";
            string againUrl = $"{_paymentConfiguration.Urls.PaymentCartUrl}/{checkoutCart.Id}";
            var request = await paymentService.GetPaymentRequestAsync(checkoutCart, againUrl, returnUrl, cancellationToken);
            var url = await paymentService.GetPaymentUrl(checkoutCart, againUrl, returnUrl, cancellationToken);

            checkoutCart.PaymentRequest = request;
            await checkoutCart.SaveAsync(cancellationToken);
            await myCart.SaveAsync(cancellationToken);
            await LogAction(checkoutCart.Id, OrderTrackingAction.CHECKOUT);
            return url;
        }

        public async Task<string?> CheckoutGuest(
            PaymentGateway gateway,
            OrderViewModel checkoutCart,
            CancellationToken cancellationToken = default)
        {

            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                checkoutCart.TempId = Guid.NewGuid();
                checkoutCart.PaymentGateway = gateway;
                await FilterGuestCheckoutCartAsync(checkoutCart);

                var paymentService = PaymentServiceFactory.GetPaymentService(_serviceProvider, gateway);

                if (paymentService == null)
                {
                    throw new MixException(MixErrorStatus.ServerError, $"Not Implement {gateway} payment");
                }

                string returnUrl = $"{_paymentConfiguration.Urls.PaymentResponseUrl}/{checkoutCart.Id}";
                string againUrl = $"{_paymentConfiguration.Urls.PaymentCartUrl}/{checkoutCart.Id}";
                var request = await paymentService.GetPaymentRequestAsync(checkoutCart, againUrl, returnUrl, cancellationToken);
                var url = await paymentService.GetPaymentUrl(checkoutCart, againUrl, returnUrl, cancellationToken);

                checkoutCart.PaymentRequest = request;
                var id = await checkoutCart.SaveAsync(cancellationToken);
                await LogAction(id, OrderTrackingAction.CHECKOUT);
                return url;
            }
            catch(Exception ex)
            {
                throw new MixException(MixErrorStatus.ServerError, ex);
            }
        }

        private async Task FilterGuestCheckoutCartAsync(OrderViewModel checkoutCart)
        {
            checkoutCart.SetUowInfo(_uow, CacheService);
            checkoutCart.OrderItems = checkoutCart.OrderItems.Where(m => m.IsActive).ToList();
            var skus = checkoutCart.OrderItems.Select(m => m.Sku).ToList();

            var orderProducts = _uow.DbContext.Warehouse.Where(m => skus.Contains(m.Sku));
            if (orderProducts.Count(m => m.Inventory == 0) > 0)
            {
                var soldOutProducts = orderProducts.Where(m => m.Inventory == 0).Select(m => $"{m.Sku} sold out").ToArray();
                throw new MixException(MixErrorStatus.Badrequest, soldOutProducts);
            }

            foreach (var item in orderProducts)
            {
                item.Inventory -= 1;
                item.Sold += 1;
            }

            checkoutCart.Calculate();

            checkoutCart.SetUowInfo(_uow, CacheService);
            //checkoutCart.Id = (_uow.DbContext.OrderDetail.Max(m => m.Id) ?? 0) + 1;
            checkoutCart.LastModified = DateTime.UtcNow;
            checkoutCart.OrderStatus = OrderStatus.WAITING_FOR_PAYMENT;

            checkoutCart.CreatedDateTime = DateTime.UtcNow;
            checkoutCart.Calculate();

            await _uow.DbContext.SaveChangesAsync();
        }
        
        private async Task FilterCheckoutCartAsync(OrderViewModel checkoutCart, OrderViewModel myCart)
        {
            myCart.SetUowInfo(_uow, CacheService);
            myCart.OrderItems = checkoutCart.OrderItems.Where(m => !m.IsActive).ToList();
            checkoutCart.OrderItems = checkoutCart.OrderItems.Where(m => m.IsActive).ToList();
            var skus = checkoutCart.OrderItems.Select(m => m.Sku).ToList();

            var orderProducts = _uow.DbContext.Warehouse.Where(m => skus.Contains(m.Sku));
            if (orderProducts.Count(m => m.Inventory == 0) > 0)
            {
                var soldOutProducts = orderProducts.Where(m => m.Inventory == 0).Select(m => $"{m.Sku} sold out").ToArray();
                throw new MixException(MixErrorStatus.Badrequest, soldOutProducts);
            }

            foreach (var item in orderProducts)
            {
                item.Inventory -= 1;
                item.Sold += 1;
            }

            myCart.Calculate();

            checkoutCart.SetUowInfo(_uow, CacheService);
            checkoutCart.Id = _uow.DbContext.OrderDetail.Max(m => m.Id) + 1;
            checkoutCart.LastModified = DateTime.UtcNow;
            checkoutCart.OrderStatus = OrderStatus.WAITING_FOR_PAYMENT;

            checkoutCart.CreatedDateTime = DateTime.UtcNow;
            checkoutCart.Calculate();

            await _uow.DbContext.SaveChangesAsync();
        }

        public async Task<OrderStatus> ProcessPaymentResponse(
            int orderId,
            JObject paymentResponse,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var order = await OrderViewModel.GetRepository(_uow, CacheService).GetSingleAsync(orderId, cancellationToken);
            if (order == null)
            {
                throw new MixException(MixErrorStatus.ServerError, $"Invalid Order");
            }

            var paymentService = PaymentServiceFactory.GetPaymentService(_serviceProvider, order.PaymentGateway!.Value);

            if (paymentService == null)
            {
                throw new MixException(MixErrorStatus.ServerError, $"Not Implement {order.PaymentGateway} payment");
            }
            order.PaymentResponse = paymentResponse;
            order.PaymentStatus = await paymentService.ProcessPaymentResponse(order, paymentResponse, cancellationToken);
            order.LastModified = DateTime.UtcNow;
            if (order.PaymentStatus == PaymentStatus.SUCCESS)
            {
                order.OrderStatus = OrderStatus.PAID;
                if (!string.IsNullOrEmpty(order.Email))
                {
                    await _edmService.SendMailWithEdmTemplate("Payment Success", "PaymentSuccess", JObject.FromObject(order), order.Email);
                }
                await LogAction(order.Id, OrderTrackingAction.PAID);
            }
            else
            {
                await LogAction(order.Id, OrderTrackingAction.PAYMENT_FAILED);
            }
            await order.SaveAsync(cancellationToken);
            return order.OrderStatus;
        }

        public async Task LogAction(int orderId, OrderTrackingAction action, string? note = "")
        {
            OrderTrackingViewModel log = new(_uow)
            {
                OrderDetailId = orderId,
                Action = action,
                Note = note,
                MixTenantId = CurrentTenant.Id
            };
            await log.SaveAsync();
        }


    }
}
