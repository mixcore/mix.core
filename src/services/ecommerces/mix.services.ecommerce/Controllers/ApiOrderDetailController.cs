using Microsoft.AspNetCore.Mvc;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Attributes;
using Mix.Lib.Base;
using Mix.Lib.Services;
using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Mix.Service.Services;
using Mix.Services.Ecommerce.Lib.Dtos;
using Mix.Services.Ecommerce.Lib.Entities.Mix;
using Mix.Services.Ecommerce.Lib.Services;
using Mix.Services.Ecommerce.Lib.ViewModels;

namespace Mix.Services.Ecommerce.Controllers
{
    [Route("api/v2/rest/ecommerce/order-detail")]
    [ApiController]
    [MixDatabaseAuthorize("OrderDetail")]
    public class OrderDetailController
        : MixRestfulApiControllerBase<OrderViewModel, EcommerceDbContext, OrderDetail, int>
    {
        private readonly EcommerceService _ecommerceService;
        public OrderDetailController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixService mixService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<EcommerceDbContext> uow,
            IQueueService<MessageQueueModel> queueService,
            EcommerceService ecommerceService)
            : base(httpContextAccessor, configuration, mixService, translator, mixIdentityService, uow, queueService)
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
