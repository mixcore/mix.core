using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mix.Database.Entities.AuditLog;
using Mix.Database.Entities.Queue;
using Mix.Lib.Interfaces;
using Mix.SignalR.Interfaces;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/queue-log")]
    [ApiController]
    [MixAuthorize(MixRoles.Owner)]
    public class MixQueueLogController
        : MixRestfulApiControllerBase<MixQueueMessageLogViewModel, MixQueueDbContext, MixQueueMessageLog, Guid>
    {
        public MixQueueLogController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixCacheService cacheService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<MixQueueDbContext> uow,
            IQueueService<MessageQueueModel> queueService,
            IPortalHubClientService portalHub,
            IMixTenantService mixTenantService)
            : base(httpContextAccessor, configuration,
                  cacheService, translator, mixIdentityService, uow, queueService, portalHub, mixTenantService)
        {

        }

        #region Overrides
        protected override async Task<PagingResponseModel<MixQueueMessageLogViewModel>> SearchHandler(SearchRequestDto req, CancellationToken cancellationToken = default)
        {
            if (req.FromDate.HasValue)
            {
                Uow = new(new MixQueueDbContext(req.FromDate.Value));
                if (Uow.DbContext.Database.GetPendingMigrations().Any())
                {
                    await Uow.DbContext.Database.MigrateAsync();
                }
                RestApiService.Repository.SetUowInfo(Uow);
            }
            return await base.SearchHandler(req, cancellationToken);
        }
        #endregion
    }
}
