using Microsoft.AspNetCore.Mvc;
using Mix.Auth.Constants;
using Mix.Lib.Interfaces;
using Mix.Mq.Lib.Models;
using Mix.SignalR.Interfaces;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/mix-db-table-relationship")]
    [ApiController]
    [MixAuthorize(MixRoles.Owner)]
    public class MixDbTableRelationshipController
        : MixRestfulApiControllerBase<MixDbTableRelationshipViewModel, MixCmsContext, MixDbTableRelationship, int>
    {
        public MixDbTableRelationshipController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixCacheService cacheService,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<MixCmsContext> cmsUow,
            IMemoryQueueService<MessageQueueModel> queueService,
            IPortalHubClientService portalHub,
            IMixTenantService mixTenantService)
            : base(httpContextAccessor, configuration,
                  cacheService, mixIdentityService, cmsUow, queueService, portalHub, mixTenantService)
        {

        }

        #region Overrides


        #endregion
    }
}
