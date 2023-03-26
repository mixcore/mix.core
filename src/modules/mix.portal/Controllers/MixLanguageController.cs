using Microsoft.AspNetCore.Mvc;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/language")]
    [ApiController]
    [MixAuthorize(MixRoles.Owner)]
    public class MixLanguageController
        : MixRestfulApiControllerBase<MixLanguageContentViewModel, MixCmsContext, MixLanguageContent, int>
    {
        public MixLanguageController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, MixService mixService, TranslatorService translator, MixIdentityService mixIdentityService, UnitOfWorkInfo<MixCmsContext> cmsUow, IQueueService<MessageQueueModel> queueService, MixCacheService cacheService) : base(httpContextAccessor, configuration, mixService, translator, mixIdentityService, cmsUow, queueService, cacheService)
        {

        }

        #region Overrides


        #endregion
    }
}
