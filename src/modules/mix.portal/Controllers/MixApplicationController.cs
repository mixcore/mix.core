using Microsoft.AspNetCore.Mvc;
using Mix.Portal.Domain.Interfaces;

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
            UnitOfWorkInfo<MixCmsContext> uow,
            IQueueService<MessageQueueModel> queueService, MixCacheService cacheService)
            : base(httpContextAccessor, configuration, mixService, translator, mixIdentityService, uow, queueService, cacheService)
        {
            _cmsContext = uow.DbContext;
        }

        #region Routes

        [HttpPost]
        [Route("install")]
        public async Task<ActionResult<MixApplicationViewModel>> Install([FromBody] MixApplicationViewModel app, [FromServices] IMixApplicationService applicationService)
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
        protected override async Task<PagingResponseModel<MixApplicationViewModel>> SearchHandler(SearchRequestDto req, CancellationToken cancellationToken = default)
        {
            var result = await base.SearchHandler(req, cancellationToken);
            foreach (var item in result.Items)
            {
                item.DetailUrl = $"{CurrentTenant.PrimaryDomain}/app/{item.BaseRoute}";
            }
            return result;
        }

        #endregion
    }
}
