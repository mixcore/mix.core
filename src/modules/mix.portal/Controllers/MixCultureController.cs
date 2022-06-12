using Microsoft.AspNetCore.Mvc;
using Mix.Portal.Domain.Services;
using Mix.Shared.Services;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/culture")]
    [ApiController]
    [MixAuthorize($"{MixRoles.SuperAdmin}, {MixRoles.Owner}")]
    public class MixCultureController
        : MixRestApiControllerBase<MixCultureViewModel, MixCmsContext, MixCulture, int>
    {
        private CultureService _cultureService;
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
            IQueueService<MessageQueueModel> queueService, CultureService cultureService)
            : base(httpContextAccessor, configuration, mixService, translator, cultureRepository, mixIdentityService, cacheUOW, cmsUOW, queueService)
        {
            _cloneCultureService = cloneCultureService;
            _cultureService = cultureService;
        }

        #region Routes

        #endregion
        #region Overrides

        protected override async Task<int> CreateHandlerAsync(MixCultureViewModel data)
        {
            var result = await base.CreateHandlerAsync(data);
            if (result > 0)
            {
                await _cloneCultureService.CloneDefaultCulture(GlobalConfigService.Instance.DefaultCulture, data.Specificulture);
                _cultureService.LoadCultures((MixCmsContext)_uow.ActiveDbContext);
            }
            return result;
        }

        protected override async Task DeleteHandler(MixCultureViewModel data)
        {
            await base.DeleteHandler(data);
            _cultureService.LoadCultures((MixCmsContext)_uow.ActiveDbContext);
        }

        #endregion


    }
}
