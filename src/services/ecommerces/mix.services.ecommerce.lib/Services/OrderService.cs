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
            SearchRequestDto request, 
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

            searchRequest.Predicate = searchRequest.Predicate.AndAlso(m => m.UserId == user.Id);
            return await OrderViewModel.GetRepository(_uow)
                            .GetPagingAsync(
                                searchRequest.Predicate,
                                searchRequest.PagingData,
                                cancellationToken);
        }
    }
}
