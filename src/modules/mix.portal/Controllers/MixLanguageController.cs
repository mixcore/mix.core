using Microsoft.AspNetCore.Mvc;
using Mix.Lib.Interfaces;
using Mix.SignalR.Interfaces;

namespace Mix.Portal.Controllers
{
    // TODO
    [Route("api/v2/rest/mix-portal/language")]
    [ApiController]
    [MixAuthorize(MixRoles.Owner)]
    public class MixLanguageController
        : MixRestfulApiControllerBase<MixLanguageContentViewModel, MixCmsContext, MixLanguageContent, int>
    {
        public MixLanguageController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, MixCacheService cacheService, TranslatorService translator, MixIdentityService mixIdentityService, UnitOfWorkInfo<MixCmsContext> uow, IQueueService<MessageQueueModel> queueService,
            IPortalHubClientService portalHub,
            IMixTenantService mixTenantService) 
            : base(httpContextAccessor, configuration, 
                  cacheService, translator, mixIdentityService, uow, queueService, portalHub, mixTenantService)
        {

        }

        #region Overrides


        #endregion
    }
}
