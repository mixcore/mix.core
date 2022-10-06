using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Mix.Heart.Constants;
using Mix.Portal.Domain.Services;
using Mix.Shared.Services;
using Mix.SignalR.Constants;
using Mix.SignalR.Hubs;
using System.Text.RegularExpressions;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/mix-application")]
    [ApiController]
    [MixAuthorize("SyperAdmin, Owner")]
    public class MixApplicationController
        : MixRestfulApiControllerBase<MixApplicationViewModel, MixCmsContext, MixApplication, int>
    {
        public MixApplicationController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixService mixService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<MixCacheDbContext> cacheUOW,
            UnitOfWorkInfo<MixCmsContext> uow,
            IQueueService<MessageQueueModel> queueService            )
            : base(httpContextAccessor, configuration, mixService, translator, mixIdentityService, cacheUOW, uow, queueService)
        {
        }

        #region Routes

        [HttpPost]
        [Route("install")]
        public async Task<ActionResult<MixApplicationViewModel>> Install([FromBody] MixApplicationViewModel app, [FromServices] MixApplicationService applicationService)
        {
            await applicationService.Install(app);
            return base.Ok(app);
        }

        #endregion

        #region Overrides


        #endregion

        
    }
}
