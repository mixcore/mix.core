using Microsoft.AspNetCore.Mvc;
using Mix.Lib.Interfaces;
using Mix.Mq.Lib.Models;
using Mix.SignalR.Interfaces;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/mix-page-content")]
    [ApiController]
    [MixAuthorize("SyperAdmin, Owner")]
    public class MixPageContentController
        : MixBaseContentController<MixPageContentViewModel, MixPageContent, int>
    {
        public MixPageContentController(
            MixIdentityService identityService,
            TenantUserManager userManager,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixCacheService cacheService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<MixCmsContext> cmsUow,
            IMemoryQueueService<MessageQueueModel> queueService,
            IPortalHubClientService portalHub,
            IMixTenantService mixTenantService)
            : base(MixContentType.Page, identityService, userManager, 
                  httpContextAccessor, configuration, cacheService, translator, 
                  mixIdentityService, cmsUow, queueService, portalHub, mixTenantService)
        {

        }

        #region Overrides


        #endregion
    }
}
