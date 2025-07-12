using DocumentFormat.OpenXml.Office2010.Excel;
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
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<MixCmsContext> cmsUow,
            IMemoryQueueService<MessageQueueModel> queueService,
            IPortalHubClientService portalHub,
            IMixTenantService mixTenantService)
            : base(MixContentType.Page, identityService, userManager,
                  httpContextAccessor, configuration, cacheService, mixIdentityService, cmsUow, queueService, portalHub, mixTenantService)
        {

        }

        #region Routes

        [HttpGet("get-by-seo-name/{seoName}")]
        public async Task<ActionResult<MixPageContentViewModel>> GetBySeoName(string seoName, CancellationToken cancellationToken = default)
        {
            var data = await RestApiService.Repository.GetFirstAsync(m => m.SeoName == seoName);
            if (data != null)
            {
                return Ok(data);
            }
            throw new MixException(MixErrorStatus.NotFound, seoName);
        }


        #endregion

        #region Overrides


        #endregion
    }
}
