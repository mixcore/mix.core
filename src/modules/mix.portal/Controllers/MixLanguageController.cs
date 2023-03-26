using Microsoft.AspNetCore.Mvc;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/language")]
    [ApiController]
    [MixAuthorize(MixRoles.Owner)]
    public class MixLanguageController
        : MixRestfulApiControllerBase<MixLanguageContentViewModel, MixCmsContext, MixLanguageContent, int>
    {
        public MixLanguageController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, MixCacheService cacheService, TranslatorService translator, MixIdentityService mixIdentityService, UnitOfWorkInfo<MixCmsContext> uow, IQueueService<MessageQueueModel> queueService) : base(httpContextAccessor, configuration, cacheService, translator, mixIdentityService, uow, queueService)
        {

        }

        #region Overrides


        #endregion
    }
}
