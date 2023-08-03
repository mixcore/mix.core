using Microsoft.AspNetCore.Mvc;
using Mix.Database.Services;
using Mix.Lib.Interfaces;
using Mix.Lib.Services;
using Mix.Shared.Services;

namespace Mixcore.Controllers
{
    [Route("app")]
    public class AppController : MixControllerBase
    {
        protected UnitOfWorkInfo Uow;
        protected readonly MixCmsContext CmsContext;
        private readonly MixCacheService _cacheService;
        private readonly DatabaseService _databaseService;

        public AppController(
            IHttpContextAccessor httpContextAccessor,
            IPSecurityConfigService ipSecurityConfigService,
            IMixCmsService mixCmsService,
            DatabaseService databaseService,
            MixCmsContext cmsContext,
            MixCacheService cacheService,
            IMixTenantService tenantService)
            : base(httpContextAccessor, mixCmsService, ipSecurityConfigService, tenantService)
        {
            CmsContext = cmsContext;
            Uow = new(CmsContext);
            _databaseService = databaseService;
            CmsContext = cmsContext;
            _cacheService = cacheService;
        }

        protected override void ValidateRequest()
        {
            base.ValidateRequest();

            // If this site has not been inited yet
            if (GlobalConfigService.Instance.AppSettings.IsInit)
            {
                IsValid = false;
                if (string.IsNullOrEmpty(_databaseService.GetConnectionString(MixConstants.CONST_CMS_CONNECTION)))
                {
                    RedirectUrl = "Init";
                }
                else
                {
                    var status = GlobalConfigService.Instance.AppSettings.InitStatus;
                    RedirectUrl = $"/init/step{status}";
                }
            }
        }

        #region Routes
        [Route("{baseRoute}/{param1?}/{param2?}/{param3?}/{param4?}/{param5?}/{param6?}")]
        public async Task<IActionResult> Index(string baseRoute)
        {
            if (IsValid)
            {
                return await App(baseRoute);
            }
            else
            {
                return Redirect(RedirectUrl);
            }
        }

        #endregion Routes

        #region Helper
        protected async Task<IActionResult> App(string baseRoute)
        {
            // Home App
            var pageRepo = ApplicationViewModel.GetRepository(Uow, _cacheService);
            var page = await pageRepo.GetSingleAsync(m => m.BaseRoute == baseRoute && m.MixTenantId == CurrentTenant.Id);
            if (page == null)
                return NotFound();

            ViewData["Image"] = page.Image;
            ViewData["ViewMode"] = MixMvcViewMode.Application;

            ViewData["ViewMode"] = MixMvcViewMode.Application;
            return View(page);
        }
    }

    #endregion
}
