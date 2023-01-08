using Microsoft.AspNetCore.Http;
using Mix.Heart.Enums;
using Mix.Heart.Exceptions;
using Mix.Heart.Helpers;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Base;
using Mix.Lib.Services;
using Mix.Services.Ecommerce.Lib.Dtos;
using Mix.Services.Ecommerce.Lib.Enums;
using Mix.Services.Ecommerce.Lib.Providers;
using Mix.Services.Ecommerce.Lib.ViewModels;
using Mix.Services.Ecommerce.Lib.Entities.Mix;
using Newtonsoft.Json.Linq;
using System.Security.Claims;
using Mix.Database.Entities.Cms;
using Mix.Services.Ecommerce.Lib.Models;
using Mix.Constant.Constants;
using Microsoft.Extensions.Configuration;
using Mix.Services.Ecommerce.Lib.Entities.Onepay;

namespace Mix.Services.Ecommerce.Lib.Services
{
    public class EcommerceService : TenantServiceBase
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly TenantUserManager _userManager;
        private readonly UnitOfWorkInfo<MixCmsContext> _cmsUOW;
        private readonly UnitOfWorkInfo<EcommerceDbContext> _uow;
        private readonly PaymentConfigurationModel _paymentConfiguration = new();
        private readonly MixEdmService _edmService;
        public EcommerceService(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            UnitOfWorkInfo<EcommerceDbContext> uow,
            TenantUserManager userManager,
            IServiceProvider serviceProvider,
            UnitOfWorkInfo<MixCmsContext> cmsUOW,
            MixEdmService edmService) : base(httpContextAccessor)
        {
            _uow = uow;
            _userManager = userManager;
            _serviceProvider = serviceProvider;
            _cmsUOW = cmsUOW;

            var session = configuration.GetSection(MixAppSettingsSection.Payments);
            session.Bind(_paymentConfiguration);
            _edmService = edmService;
        }

        public async Task<OrderViewModel?> GetShoppingOrder(Guid userId, CancellationToken cancellationToken = default)
        {
            return await OrderViewModel.GetRepository(_uow)
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
                var product = await ProductVariantViewModel.GetRepository(_uow).GetSingleAsync(
                        m => m.MixTenantId == CurrentTenant.Id
                            && m.Sku == item.Sku);

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
                ReflectionHelper.MapObject(item, orderItem);
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
                cart.OrderItems.Remove(currentItem);
                await currentItem.DeleteAsync(cancellationToken);
            }
            cart.OrderItems.Remove(currentItem);
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
            FilterCheckoutCart(checkoutCart, myCart);

            var paymentService = PaymentServiceFactory.GetPaymentService(_serviceProvider, gateway);

            if (paymentService == null)
            {
                throw new MixException(MixErrorStatus.ServerError, $"Not Implement {gateway} payment");
            }

            string returnUrl = $"{_paymentConfiguration.Urls.PaymentResponseUrl}/{checkoutCart.Id}";
            string againUrl = $"{_paymentConfiguration.Urls.PaymentCartUrl}/{checkoutCart.Id}";
            var request = paymentService.GetPaymentRequest(checkoutCart, againUrl, returnUrl, cancellationToken);
            var url = await paymentService.GetPaymentUrl(checkoutCart, againUrl, returnUrl, cancellationToken);

            checkoutCart.PaymentRequestData = request;
            await checkoutCart.SaveAsync();
            await myCart.SaveAsync();
            return url;
        }

        private void FilterCheckoutCart(OrderViewModel checkoutCart, OrderViewModel myCart)
        {
            myCart.SetUowInfo(_uow);
            myCart.OrderItems = checkoutCart.OrderItems.Where(m => !m.IsActive).ToList();
            myCart.Calculate();

            checkoutCart.SetUowInfo(_uow);
            checkoutCart.Id = _uow.DbContext.OrderDetail.Max(m => m.Id) + 1;
            checkoutCart.LastModified = DateTime.UtcNow;
            checkoutCart.OrderStatus = OrderStatus.WAITING_FOR_PAYMENT;
            checkoutCart.OrderItems = checkoutCart.OrderItems.Where(m => m.IsActive).ToList();
            checkoutCart.Calculate();
        }

        public async Task<OrderStatus> ProcessPaymentResponse(
            int orderId,
            JObject paymentResponse,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var order = await OrderViewModel.GetRepository(_uow).GetSingleAsync(orderId);
            if (order == null)
            {
                throw new MixException(MixErrorStatus.ServerError, $"Invalid Order");
            }

            var paymentService = PaymentServiceFactory.GetPaymentService(_serviceProvider, order.PaymentGateway!.Value);

            if (paymentService == null)
            {
                throw new MixException(MixErrorStatus.ServerError, $"Not Implement {order.PaymentGateway} payment");
            }
            order.PaymentResponseData = paymentResponse;
            order.PaymentStatus = await paymentService.ProcessPaymentResponse(order, paymentResponse, cancellationToken);
            order.LastModified = DateTime.UtcNow;
            if (order.PaymentStatus == PaymentStatus.SUCCESS)
            {
                order.OrderStatus = OrderStatus.PAID;
                if (!string.IsNullOrEmpty(order.Email))
                {
                    await _edmService.SendMailWithEdmTemplate("Payment Success", "PaymentSuccess", JObject.FromObject(order), order.Email);
                }
            }
            await order.SaveAsync(cancellationToken);
            return order.OrderStatus;
        }

    }
}
