using Microsoft.AspNetCore.Mvc;
using Mix.Database.Services;
using Mix.Heart.Helpers;
using Mix.Lib.Services;
using Mix.RepoDb.Repositories;
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
        private readonly MixRepoDbRepository _repoDbRepository;
        public PageController(
            IHttpContextAccessor httpContextAccessor,
            ILogger<HomeController> logger,
            IPSecurityConfigService ipSecurityConfigService,
            MixService mixService,
            MixCmsService mixCmsService,
            TranslatorService translator,
            DatabaseService databaseService,
            MixCmsContext context,
            MixRepoDbRepository repoDbRepository)
            : base(httpContextAccessor, mixService, mixCmsService, ipSecurityConfigService)
        {
            _context = context;
            _uow = new(_context);
            _logger = logger;
            _translator = translator;
            _databaseService = databaseService;
            _context = context;
            _repoDbRepository = repoDbRepository;
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
            var pageRepo = PageContentViewModel.GetRepository(_uow);
            var page = await pageRepo.GetSingleAsync(m => m.Id == pageId && m.MixTenantId == CurrentTenant.Id);
            if (page == null)
                return NotFound();

            await page.LoadDataAsync(_repoDbRepository, new(Request)
            {
                SortBy = MixQueryColumnName.Priority
            });

            

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
