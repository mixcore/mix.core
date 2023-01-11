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
using Mix.Lib.Models.Common;
using Mix.Heart.Models;
using Mix.Heart.Extensions;
using Mix.Shared.Dtos;

namespace Mix.Services.Ecommerce.Lib.Services
{
    public class OrderService : TenantServiceBase
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly TenantUserManager _userManager;
        private readonly UnitOfWorkInfo<EcommerceDbContext> _uow;
        public OrderService(
            IHttpContextAccessor httpContextAccessor,
            UnitOfWorkInfo<EcommerceDbContext> uow,
            TenantUserManager userManager,
            IServiceProvider serviceProvider) : base(httpContextAccessor)
        {
            _uow = uow;
            _userManager = userManager;
            _serviceProvider = serviceProvider;
        }

        public async Task<PagingResponseModel<OrderViewModel>> GetUserOrders(
            ClaimsPrincipal principal,
            FilterOrderDto request,
            CancellationToken cancellationToken = default)
        {
            var user = await _userManager.GetUserAsync(principal);

            if (user == null)
            {
                throw new MixException(MixErrorStatus.UnAuthorized);
            }
            var searchRequest = new SearchQueryModel<OrderDetail, int>(
                    HttpContextAccessor.HttpContext!.Request,
                    request,
                    CurrentTenant.Id);

            searchRequest.Predicate = searchRequest.Predicate.AndAlsoIf(request.Statuses.Count == 0, m => m.UserId == user.Id && m.OrderStatus != OrderStatus.NEW);
            searchRequest.Predicate = searchRequest.Predicate.AndAlsoIf(request.Statuses.Count > 0, m => m.UserId == user.Id && request.Statuses.Contains(m.OrderStatus));
            return await OrderViewModel.GetRepository(_uow, false)
                            .GetPagingAsync(
                                searchRequest.Predicate,
                                searchRequest.PagingData,
                                cancellationToken);
        }

        public async Task<OrderViewModel> GetUserOrder(ClaimsPrincipal principal, int orderId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _userManager.GetUserAsync(principal);

            if (user == null)
            {
                throw new MixException(MixErrorStatus.UnAuthorized);
            }

            return await OrderViewModel.GetRepository(_uow).GetSingleAsync(m => m.Id == orderId && m.UserId == user.Id, cancellationToken);
        }

        public async Task CancelOrder(
            ClaimsPrincipal principal,
            int id,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _userManager.GetUserAsync(principal);

            var order = await OrderViewModel.GetRepository(_uow).GetSingleAsync(m => m.Id == id && m.UserId == user!.Id);
            if (order == null)
            {
                throw new MixException(MixErrorStatus.Badrequest, "Invalid Order");
            }

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
