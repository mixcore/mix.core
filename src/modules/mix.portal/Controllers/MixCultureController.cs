using Microsoft.AspNetCore.Mvc;
using Mix.Portal.Domain.Services;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/culture")]
    [ApiController]
    [MixAuthorize($"{MixRoles.SuperAdmin}, {MixRoles.Owner}")]
    public class MixCultureController
        : MixRestApiControllerBase<MixCultureViewModel, MixCmsContext, MixCulture, int>
    {
        private CloneCultureService _cloneCultureService;
        public MixCultureController(
            CloneCultureService cloneCultureService,
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
            _cloneCultureService = cloneCultureService;
        }

        #region Routes

        #endregion
        #region Overrides

        protected override async Task<int> CreateHandlerAsync(MixCultureViewModel data)
        {
            var result = await base.CreateHandlerAsync(data);
            if (result > 0)
            {
                await _cloneCultureService.CloneDefaultCulture(data.Specificulture);
            }
            return result;
        }

        #endregion


    }
}
