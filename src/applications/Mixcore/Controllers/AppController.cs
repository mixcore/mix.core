using Microsoft.AspNetCore.Mvc;
using Mix.Database.Services;
using Mix.Heart.Helpers;
using Mix.Lib.Services;
using Mix.Portal.Domain.ViewModels;
using Mix.RepoDb.Repositories;
using Mix.Shared.Services;

namespace Mixcore.Controllers
{
    [Route("app")]
    public class AppController : MixControllerBase
    {
        protected UnitOfWorkInfo _uow;
        protected readonly MixCmsContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly TranslatorService _translator;
        private readonly DatabaseService _databaseService;
        private readonly MixRepoDbRepository _repoDbRepository;
        public AppController(
            IHttpContextAccessor httpContextAccessor,
            ILogger<HomeController> logger,
            IPSecurityConfigService ipSecurityConfigService,
            MixService mixService,
            TranslatorService translator,
            DatabaseService databaseService,
            MixCmsContext context,
            MixRepoDbRepository repoDbRepository)
            : base(httpContextAccessor, mixService, ipSecurityConfigService)
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
        [Route("{baseRoute}/{param1?}/{param2?}/{param3?}/{param4?}/{param5?}/{param6?}")]
        public async Task<IActionResult> Index(string baseRoute)
        {
            if (isValid)
            {
                return await App(baseRoute);
            }
            else
            {
                return Redirect(_redirectUrl);
            }
        }

        #endregion Routes

        #region Helper
        protected async Task<IActionResult> App(string baseRoute)
        {
            // Home App
            var pageRepo = ApplicationViewModel.GetRepository(_uow);
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
