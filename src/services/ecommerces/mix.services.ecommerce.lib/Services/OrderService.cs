using Microsoft.AspNetCore.Http;
using Mix.Heart.Enums;
using Mix.Heart.Exceptions;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Services;
using Mix.Services.Ecommerce.Lib.Dtos;
using Mix.Services.Ecommerce.Lib.Enums;
using Mix.Services.Ecommerce.Lib.ViewModels;
using Mix.Services.Ecommerce.Lib.Entities.Mix;
using System.Security.Claims;
using Mix.Lib.Models.Common;
using Mix.Heart.Models;
using Mix.Heart.Extensions;
using Mix.Service.Services;
using Mix.Services.Ecommerce.Lib.Interfaces;
using Mix.Heart.Services;
using Mix.Lib.Interfaces;

namespace Mix.Services.Ecommerce.Lib.Services
{
    public class OrderService : TenantServiceBase, IOrderService
    {
        private readonly TenantUserManager _userManager;
        private readonly UnitOfWorkInfo<EcommerceDbContext> _uow;
        public OrderService(
            IHttpContextAccessor httpContextAccessor,
            UnitOfWorkInfo<EcommerceDbContext> uow,
            TenantUserManager userManager,
            MixCacheService cacheService,
            MixConfigurationService configService,
            IMixTenantService mixTenantService) : base(httpContextAccessor, cacheService, mixTenantService)
        {
            _uow = uow;
            _userManager = userManager;
        }

        public async Task<PagingResponseModel<OrderViewModel>> GetUserOrders(
            ClaimsPrincipal principal,
            FilterOrderDto request,
            CancellationToken cancellationToken = default)
        {
            var user = await _userManager.GetUserAsync(principal) ?? throw new MixException(MixErrorStatus.UnAuthorized);
            var searchRequest = new SearchQueryModel<OrderDetail, int>(
                    HttpContextAccessor.HttpContext!.Request,
                    request,
                    CurrentTenant.Id);

            searchRequest.Predicate = searchRequest.Predicate.AndAlsoIf(request.Statuses.Count == 0, m => m.UserId == user.Id && m.OrderStatus != OrderStatus.NEW);
            searchRequest.Predicate = searchRequest.Predicate.AndAlsoIf(request.Statuses.Count > 0, m => m.UserId == user.Id && request.Statuses.Contains(m.OrderStatus));
            var result = await OrderViewModel.GetRepository(_uow, CacheService)
                            .GetPagingAsync(
                                searchRequest.Predicate,
                                searchRequest.PagingData,
                                cancellationToken);
            return result;
        }

        public async Task<OrderViewModel> GetUserOrder(ClaimsPrincipal principal, int orderId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _userManager.GetUserAsync(principal) ?? throw new MixException(MixErrorStatus.UnAuthorized);
            var result = await OrderViewModel.GetRepository(_uow, CacheService).GetSingleAsync(m => m.Id == orderId && m.UserId == user.Id, cancellationToken);
            return result;
        }

        public async Task<OrderViewModel> GetGuestOrder(int orderId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await OrderViewModel.GetRepository(_uow, CacheService).GetSingleAsync(m => m.Id == orderId, cancellationToken);
        }

        public async Task CancelOrder(
            ClaimsPrincipal principal,
            int id,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _userManager.GetUserAsync(principal) ?? throw new MixException(MixErrorStatus.Badrequest, "User not found");
            var order = await OrderViewModel.GetRepository(_uow, CacheService).GetSingleAsync(m => m.Id == id && m.UserId == user!.Id, cancellationToken) ?? throw new MixException(MixErrorStatus.Badrequest, "Invalid Order");
            if (order.PaymentStatus == PaymentStatus.SUCCESS)
            {
                throw new MixException(MixErrorStatus.Badrequest, "Cannot cancel paid order");
            }

            order.OrderStatus = OrderStatus.CANCELED;
            order.ModifiedBy = user.ModifiedBy;
            await order.SaveAsync(cancellationToken);
        }
    }
}
