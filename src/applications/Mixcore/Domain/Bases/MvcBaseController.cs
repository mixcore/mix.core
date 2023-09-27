using Microsoft.AspNetCore.Mvc;
using Mix.Database.Services;
using Mix.Lib.Services;
using Mix.Shared.Services;

namespace Mixcore.Domain.Bases
{
    [ResponseCache(CacheProfileName = "Default")]
    public class MvcBaseController : MixControllerBase
    {
        protected UnitOfWorkInfo<MixCmsContext> Uow;
        protected readonly MixCacheService CacheService;
        protected readonly TranslatorService Translator;
        protected readonly DatabaseService DatabaseService;
        public MvcBaseController(
            IHttpContextAccessor httpContextAccessor,
            IPSecurityConfigService ipSecurityConfigService,
            IMixCmsService mixCmsService,
            TranslatorService translator,
            DatabaseService databaseService,
            UnitOfWorkInfo<MixCmsContext> uow,
            MixCacheService cacheService,
            IMixTenantService tenantService,
             IConfiguration configuration) : 
            base(httpContextAccessor, mixCmsService, ipSecurityConfigService, tenantService, configuration)
        {
            Translator = translator;
            DatabaseService = databaseService;
            Uow = uow;
            CacheService = cacheService;
        }

        protected override void ValidateRequest()
        {
            base.ValidateRequest();

            // If this site has not been inited yet
            if (GlobalConfigService.Instance.AppSettings.IsInit)
            {
                IsValid = false;
                if (string.IsNullOrEmpty(DatabaseService.GetConnectionString(MixConstants.CONST_CMS_CONNECTION)))
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

        #region Helper
        protected async Task<IActionResult> Page(int pageId, string keyword = null)
        {
            // Home Page
            var pageRepo = PageContentViewModel.GetRepository(Uow, CacheService);
            var page = await pageRepo.GetSingleAsync(pageId);
            ViewData["Title"] = page.SeoTitle;
            ViewData["Description"] = page.SeoDescription;
            ViewData["Keywords"] = page.SeoKeywords;
            ViewData["Image"] = page.Image;
            ViewData["Layout"] = page.Layout.FilePath;
            ViewData["BodyClass"] = page.ClassName;
            ViewData["ViewMode"] = MixMvcViewMode.Page;
            ViewData["Keyword"] = keyword;

            ViewData["ViewMode"] = MixMvcViewMode.Page;
            return View(page);
        }
        #endregion
    }
}
