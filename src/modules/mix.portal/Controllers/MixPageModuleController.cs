using Microsoft.AspNetCore.Mvc;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/mix-page-module")]
    [ApiController]
    public class MixPageModuleController
        : MixAssociationApiControllerBase<MixPageModuleViewModel, MixCmsContext, MixPageModuleAssociation>
    {
        public MixPageModuleController(
            IConfiguration configuration,
            MixService mixService,
            TranslatorService translator,
            EntityRepository<MixCmsContext, MixCulture, int> cultureRepository,
            MixIdentityService mixIdentityService,
            MixCmsContext context,
            MixCacheService cacheService,
            IQueueService<MessageQueueModel> queueService)
            : base(configuration, mixService, translator, cultureRepository, mixIdentityService, context, cacheService, queueService)
        {

        }

        #region Overrides


        #endregion


    }
}
