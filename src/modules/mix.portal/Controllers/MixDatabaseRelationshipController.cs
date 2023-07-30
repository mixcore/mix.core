using Microsoft.AspNetCore.Mvc;
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
            UnitOfWorkInfo<MixCmsContext> uow,
            IQueueService<MessageQueueModel> queueService,
            IPortalHubClientService portalHub)
            : base(httpContextAccessor, configuration, cacheService, translator, mixIdentityService, uow, queueService, portalHub)
        {

        }

        #region Overrides


        #endregion
    }
}
