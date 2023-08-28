using Microsoft.AspNetCore.Mvc;
using Mix.Database.Entities.AuditLog;
using Mix.Heart.Services;
using Mix.Heart.UnitOfWork;
using Mix.Identity.Constants;
using Mix.Lib.Attributes;
using Mix.Lib.Base;
using Mix.Lib.Interfaces;
using Mix.Lib.Services;
using Mix.Log.Lib.ViewModels;
using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Mix.SignalR.Interfaces;

namespace Mix.Log.Controllers
{
    [Route("api/v2/rest/mix-log/audit-log")]
    [ApiController]
    [MixAuthorize(MixRoles.Owner)]
    public class AuditLogController
        : MixQueryApiControllerBase<AuditLogViewModel, AuditLogDbContext, AuditLog, Guid>
    {
        public AuditLogController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixCacheService cacheService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<AuditLogDbContext> uow,
            IQueueService<MessageQueueModel> queueService,
            IPortalHubClientService portalHub,
            IMixTenantService mixTenantService)
            : base(httpContextAccessor, configuration,
                  cacheService, translator, mixIdentityService, uow, queueService, portalHub, mixTenantService)
        {

        }

        #region Overrides

        #endregion
    }
}
