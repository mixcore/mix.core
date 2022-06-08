using Microsoft.AspNetCore.Mvc;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/culture")]
    [ApiController]
    [MixAuthorize($"{MixRoles.SuperAdmin}, {MixRoles.Owner}")]
    public class MixCultureController
        : MixRestApiControllerBase<MixCultureViewModel, MixCmsContext, MixCulture, int>
    {
        public MixCultureController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixService mixService,
            TranslatorService translator,
            EntityRepository<MixCmsContext, MixCulture, int> cultureRepository,
            MixIdentityService mixIdentityService,
            GenericUnitOfWorkInfo<MixCacheDbContext> cacheUOW,
            GenericUnitOfWorkInfo<MixCmsContext> cmdUOW,
            IQueueService<MessageQueueModel> queueService)
            : base(httpContextAccessor, configuration, mixService, translator, cultureRepository, mixIdentityService, cacheUOW, cmdUOW, queueService)
        {

        }

        #region Routes

        #endregion
        #region Overrides

        #endregion


    }
}
