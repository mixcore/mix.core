using Microsoft.AspNetCore.Mvc;
using Mix.Heart.Services;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Attributes;
using Mix.Lib.Base;
using Mix.Lib.Interfaces;
using Mix.Lib.Services;
using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Mix.Services.Ecommerce.Lib.Dtos;
using Mix.Services.Ecommerce.Lib.Entities.Mix;
using Mix.Services.Ecommerce.Lib.Interfaces;
using Mix.Services.Ecommerce.Lib.ViewModels;
using Mix.SignalR.Interfaces;

namespace Mix.Services.Ecommerce.Controllers
{
    [Route("api/v2/rest/ecommerce/order-detail")]
    [ApiController]
    [MixDatabaseAuthorize("OrderDetail")]
    public class OrderDetailController
        : MixRestfulApiControllerBase<OrderViewModel, EcommerceDbContext, OrderDetail, int>
    {
        private readonly IEcommerceService _ecommerceService;
        public OrderDetailController(
            IEcommerceService ecommerceService,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixCacheService cacheService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<EcommerceDbContext> uow,
            IQueueService<MessageQueueModel> queueService,
            IPortalHubClientService portalHub,
            IMixTenantService mixTenantService)
            : base(httpContextAccessor, configuration, 
                  cacheService, translator, mixIdentityService, uow, queueService, portalHub, mixTenantService)
        {
            Repository.IsCache = false;
            _ecommerceService = ecommerceService;
        }

        #region Overrides


        [HttpPost("update-order-status/{id}")]
        public async Task<ActionResult> UpdateStatus(int id, [FromBody] UpdateOrderStatusDto data, CancellationToken cancellationToken = default)
        {
            await _ecommerceService.UpdateOrderStatus(id, data.OrderStatus, cancellationToken);
            return Ok();
        }

        #endregion
    }
}
