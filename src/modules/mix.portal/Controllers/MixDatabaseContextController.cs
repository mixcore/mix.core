using Microsoft.AspNetCore.Mvc;
using Mix.Lib.Interfaces;
using Mix.SignalR.Interfaces;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/mix-database-context")]
    [ApiController]
    [MixAuthorize(MixRoles.Owner)]
    public class MixDatabaseContextController : MixRestfulApiControllerBase<MixDatabaseContextViewModel, MixCmsContext, MixDatabaseContext, int>
    {
        public MixDatabaseContextController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixCacheService cacheService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<MixCmsContext> uow,
            IQueueService<MessageQueueModel> queueService,
            IPortalHubClientService portalHub,
            IMixTenantService mixTenantService)
            : base(httpContextAccessor, configuration, cacheService, translator, mixIdentityService, uow, queueService, portalHub, mixTenantService)
        {
        }
    }
}
