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
        private readonly MixCmsContext _cmsContext;
        public MixApplicationController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixService mixService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<MixCacheDbContext> cacheUOW,
            UnitOfWorkInfo<MixCmsContext> uow,
            IQueueService<MessageQueueModel> queueService)
            : base(httpContextAccessor, configuration, mixService, translator, mixIdentityService, cacheUOW, uow, queueService)
        {
            _cmsContext = uow.DbContext;
        }

        #region Routes

        [HttpPost]
        [Route("install")]
        public async Task<ActionResult<MixApplicationViewModel>> Install([FromBody] MixApplicationViewModel app, [FromServices] MixApplicationService applicationService)
        {
            if (_cmsContext.MixApplication.Any(m => m.MixTenantId == CurrentTenant.Id && m.BaseRoute == app.BaseRoute && m.Id != app.Id))
            {
                return BadRequest($"BaseRoute: \"{app.BaseRoute}\" existed");
            }

            await applicationService.Install(app);
            return base.Ok(app);
        }

        #endregion

        #region Overrides
        protected override async Task<PagingResponseModel<MixApplicationViewModel>> SearchHandler(SearchRequestDto req)
        {
            var result = await base.SearchHandler(req);
            foreach (var item in result.Items)
            {
                item.DetailUrl = $"{CurrentTenant.PrimaryDomain}/app/{item.BaseRoute}";
            }
            return result;
        }

        #endregion


    }
}
