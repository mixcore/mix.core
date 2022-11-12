using Microsoft.AspNetCore.Mvc;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/language")]
    [ApiController]
    [MixAuthorize(MixRoles.Owner)]
    public class MixLanguageController
        : MixRestfulApiControllerBase<MixLanguageContentViewModel, MixCmsContext, MixLanguageContent, int>
    {
        public MixLanguageController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixService mixService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<MixCacheDbContext> cacheUOW,
            UnitOfWorkInfo<MixCmsContext> cmsUOW,
            IQueueService<MessageQueueModel> queueService)
            : base(httpContextAccessor, configuration, mixService, translator, mixIdentityService, cacheUOW, cmsUOW, queueService)
        {

        }

        #region Overrides


        #endregion


    }
}
