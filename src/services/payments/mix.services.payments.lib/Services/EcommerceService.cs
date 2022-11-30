using Microsoft.AspNetCore.Http;
using Mix.Heart.Enums;
using Mix.Heart.Exceptions;
using Mix.Heart.Helpers;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Base;
using Mix.Lib.Services;
using Mix.Queue.Engines;
using Mix.Services.Payments.Lib.Dtos;
using Mix.Services.Payments.Lib.Entities.Mix;
using Mix.Services.Payments.Lib.Enums;
using Mix.Services.Payments.Lib.Providers;
using Mix.Services.Payments.Lib.ViewModels.Mix;
using Newtonsoft.Json.Linq;
using System.Security.Claims;

namespace Mix.Services.Payments.Lib.Services
{
    public class EcommerceService : TenantServiceBase
    {
        private IServiceProvider _serviceProvider;
        private readonly TenantUserManager _userManager;
        private readonly UnitOfWorkInfo<EcommerceDbContext> _uow;
        public EcommerceService(
            IHttpContextAccessor httpContextAccessor,
            UnitOfWorkInfo<EcommerceDbContext> uow,
            TenantUserManager userManager,
            IServiceProvider serviceProvider) : base(httpContextAccessor)
        {
            _uow = uow;
            _userManager = userManager;
            _serviceProvider = serviceProvider;
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
                currentItem.Quantity += item.Quantity;
            }
            else
            {
                var orderItem = new OrderItemViewModel(_uow)
                {
                    OrderId = cart.Id,
                    MixTenantId = CurrentTenant.Id
                };
                ReflectionHelper.MapObject(item, orderItem);
                await orderItem.SaveAsync(cancellationToken);

                cart.OrderItems.Add(orderItem);
            }

            return cart;
        }

        public async Task<OrderViewModel> RemoveFromCart(
            ClaimsPrincipal principal,
            int postId,
            CancellationToken cancellationToken = default)
        {
            var cart = await GetOrCreateShoppingOrder(principal, cancellationToken);
            var currentItem = cart.OrderItems.FirstOrDefault(m => m.PostId == postId);
            if (currentItem != null)
            {
                cart.OrderItems.Remove(currentItem);
                await currentItem.DeleteAsync(cancellationToken);
            }
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
                throw new MixException(MixErrorStatus.ServerError, $"UnImplement {gateway} payment");
            }

            string returnUrl = $"{HttpContextAccessor.HttpContext.Request.Scheme}//{CurrentTenant.PrimaryDomain}/checkout/{gateway}";
            var url = await paymentService.GetPaymentUrl(cart, returnUrl) ;
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
                throw new MixException(MixErrorStatus.ServerError, $"UnImplement {gateway} payment");
            }
            await paymentService.ProcessPaymentResponse(paymentResponse);
        }

    }
}
