using Microsoft.AspNetCore.Mvc;
using Mix.Database.Entities.AuditLog;
using Mix.Lib.Interfaces;
using Mix.Portal.Domain.Interfaces;
using Mix.SignalR.Interfaces;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/mix-application")]
    [ApiController]
    [MixAuthorize("Owner")]
    public class MixApplicationController
        : MixRestfulApiControllerBase<MixApplicationViewModel, MixCmsContext, MixApplication, int>
    {
        private readonly MixCmsContext _cmsContext;
        private readonly IMixApplicationService _applicationService;
        public MixApplicationController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixCacheService cacheService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<MixCmsContext> uow,
            IQueueService<MessageQueueModel> queueService,
            IPortalHubClientService portalHub,
            IMixTenantService mixTenantService,
            IMixApplicationService applicationService)
            : base(httpContextAccessor, configuration,
                  cacheService, translator, mixIdentityService, uow, queueService, portalHub, mixTenantService)
        {
            _cmsContext = uow.DbContext;
            _applicationService = applicationService;
        }

        #region Routes

        [HttpPost]
        [Route("install")]
        public async Task<ActionResult<MixApplicationViewModel>> Install([FromBody] MixApplicationViewModel app, CancellationToken cancellationToken = default)
        {
            if (_cmsContext.MixApplication.Any(m => m.MixTenantId == CurrentTenant.Id && m.BaseHref == app.BaseHref && m.Id != app.Id))
            {
                return BadRequest($"BaseHref: \"{app.BaseHref}\" existed");
            }

            await _applicationService.Install(app, cancellationToken);
            return base.Ok(app);
        }

        [HttpPost]
        [Route("restore-package")]
        public async Task<ActionResult<MixApplicationViewModel>> Restore([FromBody] RestoreMixApplicationPackageDto dto, CancellationToken cancellationToken = default)
        {
            MixApplicationViewModel app = await _applicationService.RestorePackage(dto, cancellationToken);
            return base.Ok(app);
        }

        #endregion

        #region Overrides
        protected override async Task UpdateHandler(int id, MixApplicationViewModel data, CancellationToken cancellationToken = default)
        {
            if (!string.IsNullOrEmpty(data.PackageFilePath))
            {
                await _applicationService.UpdatePackage(data, data.PackageFilePath, cancellationToken);
            }
            await base.UpdateHandler(id, data, cancellationToken);

        }
        protected override async Task<PagingResponseModel<MixApplicationViewModel>> SearchHandler(SearchRequestDto req, CancellationToken cancellationToken = default)
        {
            var result = await base.SearchHandler(req, cancellationToken);
            foreach (var item in result.Items)
            {
                item.DetailUrl = $"{CurrentTenant.PrimaryDomain}{item.BaseHref}";
            }
            return result;
        }

        protected override async Task DeleteHandler(MixApplicationViewModel data, CancellationToken cancellationToken = default)
        {
            await base.DeleteHandler(data, cancellationToken);
            if (data.TemplateId.HasValue)
            {
                await MixViewTemplateViewModel.GetRepository(Uow, CacheService).DeleteAsync(data.TemplateId.Value);
            }
            MixFileHelper.DeleteFolder(data.DeployUrl);
        }

        #endregion
    }
}
