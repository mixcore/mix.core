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

namespace Mix.Services.Ecommerce.Lib.Services
{
    public class EcommerceService : TenantServiceBase
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly TenantUserManager _userManager;
        private readonly UnitOfWorkInfo<MixCmsContext> _cmsUOW;
        private readonly UnitOfWorkInfo<EcommerceDbContext> _uow;
        public EcommerceService(
            IHttpContextAccessor httpContextAccessor,
            UnitOfWorkInfo<EcommerceDbContext> uow,
            TenantUserManager userManager,
            IServiceProvider serviceProvider,
            UnitOfWorkInfo<MixCmsContext> cmsUOW) : base(httpContextAccessor)
        {
            _uow = uow;
            _userManager = userManager;
            _serviceProvider = serviceProvider;
            _cmsUOW = cmsUOW;
        }

        public async Task<OrderViewModel?> GetShoppingOrder(Guid userId, CancellationToken cancellationToken = default)
        {
            return await OrderViewModel.GetRepository(_uow)
                            .GetSingleAsync(
                                m => m.MixTenantId == CurrentTenant.Id
                                && m.OrderStatus == OrderStatus.New
                                && m.UserId == userId,
                                cancellationToken);
        }

        public async Task<OrderViewModel> GetOrCreateShoppingOrder(ClaimsPrincipal principal, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.GetUserAsync(principal);
            var cart = await GetShoppingOrder(user.Id, cancellationToken);

            if (cart == null)
            {
                cart = new OrderViewModel(_uow)
                {
                    UserId = user.Id,
                    Title = $"{user.UserName}'s Cart",
                    OrderStatus = OrderStatus.New,
                    MixTenantId = CurrentTenant.Id
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
            var currentItem = cart.OrderItems.FirstOrDefault(m => m.PostId == item.PostId);
            if (currentItem != null)
            {
                currentItem.Quantity = item.Quantity;
                currentItem.Calculate();
            }
            else
            {
                var product = await ProductDetailsViewModel.GetRepository(_uow).GetSingleAsync(
                        m => m.MixTenantId == CurrentTenant.Id 
                            && m.ParentId == item.PostId);
                if (product == null || !product.Price.HasValue)
                {
                    throw new MixException(MixErrorStatus.Badrequest, "Invalid Product");
                }

                var orderItem = new OrderItemViewModel(_uow)
                {
                    OrderId = cart.Id,
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

        public async Task<string?> GetPaymentUrl(
            ClaimsPrincipal principal,
            PaymentGateway gateway,
            CancellationToken cancellationToken = default)
        {
            var user = await _userManager.GetUserAsync(principal);

            if (user == null)
            {
                throw new MixException(MixErrorStatus.UnAuthorized);
            }

            var cart = await GetShoppingOrder(user.Id, cancellationToken);

            if (cart == null)
            {
                throw new MixException(MixErrorStatus.Badrequest, $"user's cart cannot be null");
            }

            var paymentService = PaymentServiceFactory.GetPaymentService(_serviceProvider, gateway);

            if (paymentService == null)
            {
                throw new MixException(MixErrorStatus.ServerError, $"Not Implement {gateway} payment");
            }

            string returnUrl = $"{HttpContextAccessor.HttpContext?.Request.Scheme}//{CurrentTenant.PrimaryDomain}/checkout/{gateway}";
            var url = await paymentService.GetPaymentUrl(cart, returnUrl, cancellationToken);
            return url;
        }

        public async Task ProcessPaymentResponse(
            PaymentGateway gateway,
            JObject paymentResponse,
            CancellationToken cancellationToken = default)
        {
            var paymentService = PaymentServiceFactory.GetPaymentService(_serviceProvider, gateway);

            if (paymentService == null)
            {
                throw new MixException(MixErrorStatus.ServerError, $"Not Implement {gateway} payment");
            }
            await paymentService.ProcessPaymentResponse(paymentResponse, cancellationToken);
        }

    }
}
