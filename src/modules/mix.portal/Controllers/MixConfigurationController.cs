using Microsoft.AspNetCore.Mvc;
using Mix.Auth.Constants;
using Mix.Lib.Interfaces;
using Mix.Mq.Lib.Models;
using Mix.SignalR.Interfaces;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/configuration")]
    [ApiController]
    [MixAuthorize(MixRoles.Owner)]
    public class MixConfigurationController
        : MixRestfulApiControllerBase<MixConfigurationContentViewModel, MixCmsContext, MixConfigurationContent, int>
    {
        private readonly MixConfigurationService _configService;
        private readonly UnitOfWorkInfo<MixCmsContext> _cmsUow;
        public MixConfigurationController(
            MixConfigurationService configService,
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
            _cmsUow = uow;
            _configService = configService;
        }

        #region Overrides

        protected override async Task<int> CreateHandlerAsync(MixConfigurationContentViewModel data, CancellationToken cancellationToken = default)
        {
            var result = await base.CreateHandlerAsync(data, cancellationToken);
            await _configService.Reload(_cmsUow);
            return result;
        }
        protected override async Task UpdateHandler(int id, MixConfigurationContentViewModel data, CancellationToken cancellationToken = default)
        {
            await base.UpdateHandler(id, data, cancellationToken);
            await _configService.Reload(_cmsUow);
        }

        protected override async Task DeleteHandler(MixConfigurationContentViewModel data, CancellationToken cancellationToken = default)
        {
            await base.DeleteHandler(data, cancellationToken);
            await _configService.Reload(_cmsUow);
        }
        #endregion
    }
}
