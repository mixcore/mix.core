using Microsoft.AspNetCore.Mvc;
using Mix.Auth.Constants;
using Mix.Lib.Interfaces;
using Mix.Mq.Lib.Models;
using Mix.Portal.Domain.Interfaces;
using Mix.SignalR.Interfaces;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/culture")]
    [ApiController]
    [MixAuthorize(MixRoles.Owner)]
    public class MixCultureController
        : MixRestfulApiControllerBase<MixCultureViewModel, MixCmsContext, MixCulture, int>
    {
        private readonly ICloneCultureService _cloneCultureService;
        public MixCultureController(
            ICloneCultureService cloneCultureService,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixCacheService cacheService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<MixCmsContext> uow,
            IMemoryQueueService<MessageQueueModel> queueService,
            IPortalHubClientService portalHub,
            IMixTenantService mixTenantService)
            : base(httpContextAccessor, configuration, 
                  cacheService, translator, mixIdentityService, uow, queueService, portalHub, mixTenantService)
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
                await _cloneCultureService.CloneDefaultCulture(CurrentTenant.Cultures[0]?.Specificulture, data.Specificulture, cancellationToken);
            }
            return result;
        }

        #endregion
    }
}
