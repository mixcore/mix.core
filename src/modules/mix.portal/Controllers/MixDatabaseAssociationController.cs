using Microsoft.AspNetCore.Mvc;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/mix-database-association")]
    [ApiController]
    [MixAuthorize($"{MixRoles.SuperAdmin}, {MixRoles.Owner}")]
    public class MixDatabaseAssociationController
        : MixRestApiControllerBase<MixDatabaseAssociationViewModel, MixCmsContext, MixDatabaseAssociation, Guid>
    {
        public MixDatabaseAssociationController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixService mixService,
            TranslatorService translator,
            EntityRepository<MixCmsContext, MixCulture, int> cultureRepository,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<MixCacheDbContext> cacheUOW,
            UnitOfWorkInfo<MixCmsContext> cmsUOW,
            IQueueService<MessageQueueModel> queueService)
            : base(httpContextAccessor, configuration, mixService, translator, cultureRepository, mixIdentityService, cacheUOW, cmsUOW, queueService)
        {

        }

        #region Overrides


        #endregion


    }
}
