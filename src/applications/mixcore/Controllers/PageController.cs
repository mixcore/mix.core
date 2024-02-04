using Microsoft.AspNetCore.Mvc;
using Mix.Database.Services;
using Mix.RepoDb.Interfaces;
using Mix.RepoDb.Repositories;
using Mix.Services.Databases.Lib.Interfaces;
using Mix.Shared.Services;

namespace Mixcore.Controllers
{
    [Route("{controller}")]
    public class PageController : MixControllerBase
    {
        protected UnitOfWorkInfo Uow;
        protected readonly MixCmsContext CmsContext;
        private readonly DatabaseService _databaseService;
        private readonly MixCacheService _cacheService;
        private readonly IMixMetadataService _metadataService;
        private readonly IMixDbDataService _mixDbDataService;

        public PageController(IHttpContextAccessor httpContextAccessor, IPSecurityConfigService ipSecurityConfigService, IMixCmsService mixCmsService, DatabaseService databaseService, MixCmsContext cmsContext, IMixMetadataService metadataService, MixCacheService cacheService, IMixTenantService tenantService,
             IConfiguration configuration, IMixDbDataService mixDbDataService) :
            base(httpContextAccessor, mixCmsService, ipSecurityConfigService, tenantService, configuration)
        {
            CmsContext = cmsContext;
            Uow = new(CmsContext);
            _databaseService = databaseService;
            CmsContext = cmsContext;
            _metadataService = metadataService;
            _cacheService = cacheService;
            _mixDbDataService = mixDbDataService;
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
        [Route("{id}/{keyword?}")]
        public async Task<IActionResult> Index(int id, string keyword)
        {
            if (IsValid)
            {
                return await Page(id, keyword);
            }
            else
            {
                return Redirect(RedirectUrl);
            }
        }

        #endregion Routes

        #region Helper
        protected async Task<IActionResult> Page(int pageId, string keyword = null)
        {
            // Home Page
            var pageRepo = PageContentViewModel.GetRepository(Uow, _cacheService);
            var page = await pageRepo.GetSingleAsync(m => m.Id == pageId && m.MixTenantId == CurrentTenant.Id);
            if (page == null)
                return NotFound();

            await page.LoadDataAsync(_mixDbDataService, _metadataService, new(Request)
            {
                SortBy = MixQueryColumnName.Priority
            }, _cacheService);


            ViewData["Title"] = page.SeoTitle;
            ViewData["Description"] = page.SeoDescription;
            ViewData["Keywords"] = page.SeoKeywords;
            ViewData["Image"] = page.Image;
            ViewData["Layout"] = page.Layout?.FilePath;
            ViewData["BodyClass"] = page.ClassName;
            ViewData["ViewMode"] = MixMvcViewMode.Page;
            ViewData["Keyword"] = keyword;

            ViewData["ViewMode"] = MixMvcViewMode.Page;
            return View(page);
        }
    }


    #endregion
}
