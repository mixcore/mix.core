using Microsoft.AspNetCore.Mvc;
using Mix.Database.Entities.AuditLog;
using Mix.SignalR.Interfaces;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/audit-log")]
    [ApiController]
    [MixAuthorize(MixRoles.Owner)]
    public class AuditLogController
        : MixRestfulApiControllerBase<AuditLogViewModel, AuditLogDbContext, AuditLog, Guid>
    {
        public AuditLogController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixCacheService cacheService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<AuditLogDbContext> uow,
            IQueueService<MessageQueueModel> queueService,
            IPortalHubClientService portalHub)
            : base(httpContextAccessor, configuration, cacheService, translator, mixIdentityService, uow, queueService, portalHub)
        {

        }

        #region Overrides

        #endregion
    }
}
