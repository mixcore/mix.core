using Microsoft.AspNetCore.Mvc;
using Mix.Auth.Constants;
using Mix.Lib.Interfaces;
using Mix.SignalR.Interfaces;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/mix-database-relationship")]
    [ApiController]
    [MixAuthorize(MixRoles.Owner)]
    public class MixDatabaseRelationshipController
        : MixRestfulApiControllerBase<MixDatabaseRelationshipViewModel, MixCmsContext, MixDatabaseRelationship, int>
    {
        public MixDatabaseRelationshipController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixCacheService cacheService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<MixCmsContext> cmsUow,
            IQueueService<MessageQueueModel> queueService,
            IPortalHubClientService portalHub,
            IMixTenantService mixTenantService)
            : base(httpContextAccessor, configuration,
                  cacheService, translator, mixIdentityService, cmsUow, queueService, portalHub, mixTenantService)
        {

        }

        #region Overrides


        #endregion
    }
}
