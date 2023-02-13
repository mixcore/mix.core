using Microsoft.AspNetCore.Mvc;
using Mix.Portal.Domain.Services;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/culture")]
    [ApiController]
    [MixAuthorize($"{MixRoles.SuperAdmin}, {MixRoles.Owner}")]
    public class MixCultureController
        : MixRestfulApiControllerBase<MixCultureViewModel, MixCmsContext, MixCulture, int>
    {
        private readonly CloneCultureService _cloneCultureService;
        public MixCultureController(
            CloneCultureService cloneCultureService,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixService mixService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<MixCmsContext> cmsUow,
            IQueueService<MessageQueueModel> queueService)
            : base(httpContextAccessor, configuration, mixService, translator, mixIdentityService, cmsUow, queueService)
        {
            _cloneCultureService = cloneCultureService;
        }

        #region Routes

        #endregion

        #region Overrides

        protected override async Task<int> CreateHandlerAsync(MixCultureViewModel data, CancellationToken cancellationToken = default)
        {
            var result = await base.CreateHandlerAsync(data, cancellationToken);
            if (result > 0)
            {
                await _cloneCultureService.CloneDefaultCulture(CurrentTenant.Configurations.DefaultCulture, data.Specificulture, cancellationToken);
            }
            return result;
        }

        #endregion
    }
}
