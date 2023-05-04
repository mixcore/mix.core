using Microsoft.AspNetCore.Mvc;

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
            IQueueService<MessageQueueModel> queueService)
            : base(httpContextAccessor, configuration, cacheService, translator, mixIdentityService, uow, queueService)
        {

        }

        #region Overrides


        #endregion
    }
}
