using Microsoft.AspNetCore.Mvc;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/mix-database-relationship")]
    [ApiController]
    [MixAuthorize($"{MixRoles.SuperAdmin}, {MixRoles.Owner}")]
    public class MixDatabaseRelationshipController
        : MixRestfulApiControllerBase<MixDatabaseRelationshipViewModel, MixCmsContext, MixDatabaseRelationship, int>
    {
        public MixDatabaseRelationshipController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixService mixService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<MixCmsContext> cmsUOW,
            IQueueService<MessageQueueModel> queueService)
            : base(httpContextAccessor, configuration, mixService, translator, mixIdentityService, cmsUOW, queueService)
        {

        }

        #region Overrides


        #endregion


    }
}
