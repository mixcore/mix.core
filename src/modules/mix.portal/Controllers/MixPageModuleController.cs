using Microsoft.AspNetCore.Mvc;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/mix-page-module")]
    [ApiController]
    public class MixPageModuleController
        : MixAssociationApiControllerBase<MixPageModuleViewModel, MixCmsContext, MixPageModuleAssociation>
    {
        public MixPageModuleController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixService mixService,
            TranslatorService translator,
            EntityRepository<MixCmsContext, MixCulture, int> cultureRepository,
            MixIdentityService mixIdentityService,
            MixCmsContext context,
            IQueueService<MessageQueueModel> queueService)
            : base(httpContextAccessor, configuration, mixService, translator, cultureRepository, mixIdentityService, context, queueService)
        {

        }

        #region Overrides


        #endregion


    }
}
