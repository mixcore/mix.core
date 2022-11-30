using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.EntityFrameworkCore;
using mix.services.ecommerce.Domain.Dtos;
using mix.services.ecommerce.Domain.Entities;
using mix.services.ecommerce.Domain.Enums;
using mix.services.ecommerce.Domain.ViewModels;
using Mix.Database.Entities.Account;
using Mix.Heart.Helpers;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Base;
using Mix.Lib.Services;
using Mix.Lib.ViewModels;
using System.Security.Claims;

namespace mix.services.ecommerce.Domain.Services
{
    public class EcommerceService : TenantServiceBase
    {
        private readonly TenantUserManager _userManager;
        private readonly UnitOfWorkInfo<EcommerceDbContext> _uow;
        public EcommerceService(
            IHttpContextAccessor httpContextAccessor,
            UnitOfWorkInfo<EcommerceDbContext> uow,
            TenantUserManager userManager) : base(httpContextAccessor)
        {
            _uow = uow;
            _userManager = userManager;
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
            CartItemDto item,
            CancellationToken cancellationToken = default)
        {
            var cart = await GetOrCreateShoppingOrder(principal, cancellationToken);
            var currentItem = cart.OrderItems.FirstOrDefault(m => m.PostId == item.PostId);
            if (currentItem != null)
            {
                cart.OrderItems.Remove(currentItem);
                await currentItem.DeleteAsync(cancellationToken);
            }
            return cart;
        }

    }
}
