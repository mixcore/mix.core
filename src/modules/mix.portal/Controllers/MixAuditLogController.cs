using Microsoft.AspNetCore.Mvc;
using Mix.Database.Entities.AuditLog;

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
            MixService mixService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<AuditLogDbContext> uow,
            IQueueService<MessageQueueModel> queueService)
            : base(httpContextAccessor, configuration, mixService, translator, mixIdentityService, uow, queueService)
        {

        }

        #region Overrides

        #endregion
    }
}
