using Microsoft.AspNetCore.Mvc;
using Mix.Auth.Constants;
using Mix.Lib.Interfaces;
using Mix.Mq.Lib.Models;
using Mix.SignalR.Interfaces;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/mix-module-content")]
    [ApiController]
    [MixAuthorize(MixRoles.Owner)]
    public class MixModuleController : MixRestfulApiControllerBase<MixModuleContentViewModel, MixCmsContext, MixModuleContent, int>
    {
        public MixModuleController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixCacheService cacheService,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<MixCmsContext> uow,
            IMemoryQueueService<MessageQueueModel> queueService,
            IPortalHubClientService portalHub,
            IMixTenantService mixTenantService)
            : base(httpContextAccessor, configuration, cacheService, mixIdentityService, uow, queueService, portalHub, mixTenantService)
        {
        }
    }
}
