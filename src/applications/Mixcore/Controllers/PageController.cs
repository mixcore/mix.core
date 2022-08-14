using Microsoft.AspNetCore.Mvc;
using Mix.Database.Entities.Runtime;
using Mix.Database.Services;
using Mix.Lib.Services;
using Mix.Shared.Services;

namespace Mixcore.Controllers
{
    [Route("{controller}")]
    public class PageController : MixControllerBase
    {
        protected UnitOfWorkInfo _uow;
        protected readonly MixCmsContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly TranslatorService _translator;
        private readonly DatabaseService _databaseService;
        private readonly RuntimeDbContextService _runtimeDbContextService;
        public PageController(
            IHttpContextAccessor httpContextAccessor,
            ILogger<HomeController> logger,
            IPSecurityConfigService ipSecurityConfigService,
            MixService mixService,
            TranslatorService translator,
            DatabaseService databaseService,
            MixCmsContext context,
            MixCacheService cacheService,
            RuntimeDbContextService runtimeDbContextService)
            : base(httpContextAccessor, mixService, ipSecurityConfigService)
        {
            _context = context;
            _uow = new(_context);
            _logger = logger;
            _translator = translator;
            _databaseService = databaseService;
            _context = context;
            _runtimeDbContextService = runtimeDbContextService;
        }

        protected override void ValidateRequest()
        {
            base.ValidateRequest();

            // If this site has not been inited yet
            if (GlobalConfigService.Instance.AppSettings.IsInit)
            {
                isValid = false;
                if (string.IsNullOrEmpty(_databaseService.GetConnectionString(MixConstants.CONST_CMS_CONNECTION)))
                {
                    _redirectUrl = "Init";
                }
                else
                {
                    var status = GlobalConfigService.Instance.AppSettings.InitStatus;
                    _redirectUrl = $"/init/step{status}";
                }
            }
        }

        #region Routes
        [Route("{id}")]
        public async Task<IActionResult> Index(int id, string keyword)
        {
            if (isValid)
            {
                return await Page(id, keyword);
            }
            else
            {
                return Redirect(_redirectUrl);
            }
        }

        #endregion Routes

        #region Helper
        protected async Task<IActionResult> Page(int pageId, string keyword = null)
        {
            // Home Page
            var pageRepo = PageContentViewModel.GetRepository(_uow);
            var page = await pageRepo.GetSingleAsync(m => m.Id == pageId && m.MixTenantId == MixTenantId);
            if (page == null)
                return NotFound();
            if (page.AdditionalData == null)
            {
                await page.LoadAdditionalDataAsync(_runtimeDbContextService);
                await pageRepo.CacheService.SetAsync($"{page.Id}/{typeof(PostContentViewModel).FullName}", pageRepo, typeof(MixPostContent), pageRepo.CacheFilename);
            }

            ViewData["Title"] = page.SeoTitle;
            ViewData["Description"] = page.SeoDescription;
            ViewData["Keywords"] = page.SeoKeywords;
            ViewData["Image"] = page.Image;
            ViewData["Layout"] = page.Layout?.FilePath;
            ViewData["BodyClass"] = page.ClassName;
            ViewData["ViewMode"] = MixMvcViewMode.Page;
            ViewData["Keyword"] = keyword;

            ViewBag.viewMode = MixMvcViewMode.Page;
            return View(page);
        }
    }


    #endregion
}
