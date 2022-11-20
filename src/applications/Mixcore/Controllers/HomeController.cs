using Microsoft.AspNetCore.Mvc;
using Mix.Database.Services;
using Mix.Heart.Extensions;
using Mix.Lib.Services;
using Mix.RepoDb.Repositories;
using Mix.Shared.Services;
using Mixcore.Domain.Bases;
using System.Linq.Expressions;

namespace Mixcore.Controllers
{
    public class HomeController : MvcBaseController
    {
        private readonly MixRepoDbRepository _repoDbRepository;
        private readonly ILogger<HomeController> _logger;
        public HomeController(
            IHttpContextAccessor httpContextAccessor,
            ILogger<HomeController> logger,
            IPSecurityConfigService ipSecurityConfigService,
            MixService mixService,
            MixCmsService mixCmsService,
            TranslatorService translator,
            DatabaseService databaseService,
            UnitOfWorkInfo<MixCmsContext> uow,
            MixRepoDbRepository repoDbRepository)
            : base(httpContextAccessor, ipSecurityConfigService, mixService, mixCmsService, translator, databaseService, uow)
        {
            _logger = logger;
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

        [Route("{seoName?}")]
        public async Task<IActionResult> Index([FromRoute] string seoName)
        {

            if (!isValid)
            {
                return Redirect(_redirectUrl);
            }
            var page = await LoadPage(seoName);
            if (page != null)
            {
                return View(page);
            }
            return await LoadAlias(seoName);
        }


        private async Task<PageContentViewModel> LoadPage(string seoName = null)
        {
            var pageRepo = PageContentViewModel.GetRepository(_uow);
            Expression<Func<MixPageContent, bool>> predicate = p => p.MixTenantId == CurrentTenant.Id
                    && p.Specificulture == Culture;
            predicate = predicate.AndAlsoIf(string.IsNullOrEmpty(seoName), m => m.Type == MixPageType.Home);
            predicate = predicate.AndAlsoIf(!string.IsNullOrEmpty(seoName), m => m.SeoName == seoName);
            var page = await pageRepo.GetFirstAsync(predicate);

            if (page != null)
            {
                await page.LoadDataAsync(_repoDbRepository, new(Request)
                {
                    SortBy = MixQueryColumnName.Priority
                });

                ViewData["Tenant"] = CurrentTenant;
                ViewData["Title"] = page.SeoTitle;
                ViewData["Description"] = page.SeoDescription;
                ViewData["Keywords"] = page.SeoKeywords;
                ViewData["Image"] = page.Image;
                ViewData["Layout"] = page.Layout?.FilePath;
                ViewData["BodyClass"] = page.ClassName;
                ViewData["ViewMode"] = MixMvcViewMode.Page;
                ViewData["Keyword"] = page.SeoKeywords;

                ViewData["ViewMode"] = MixMvcViewMode.Page;
            }
            return page;
        }
        private async Task<IActionResult> LoadAlias(string seoName)
        {
            var alias = await MixUrlAliasViewModel.GetRepository(_uow).GetSingleAsync(m => m.Alias == seoName);
            if (alias != null)
            {
                switch (alias.Type)
                {
                    case MixUrlAliasType.Page:
                        var page = await PageContentViewModel.GetRepository(_uow).GetSingleAsync(m => m.Id == alias.SourceContentId);
                        if (page != null)
                        {
                            await page.ExpandView();
                            ViewData["Tenant"] = CurrentTenant;
                            ViewData["Title"] = page.SeoTitle;
                            ViewData["Description"] = page.SeoDescription;
                            ViewData["Keywords"] = page.SeoKeywords;
                            ViewData["Image"] = page.Image;
                            ViewData["Layout"] = page.Layout?.FilePath;
                            ViewData["BodyClass"] = page.ClassName;
                            ViewData["ViewMode"] = MixMvcViewMode.Page;
                            ViewData["Keyword"] = page.SeoKeywords;

                            ViewData["ViewMode"] = MixMvcViewMode.Page;
                            return View("Page", page);
                        }
                        break;
                    case MixUrlAliasType.Post:
                        var post = await PostContentViewModel.GetRepository(_uow).GetSingleAsync(m => m.Id == alias.SourceContentId);
                        if (post != null)
                            return View("Post", post);
                        break;
                    case MixUrlAliasType.Module:
                        break;
                    case MixUrlAliasType.ModuleData:
                        break;
                    case MixUrlAliasType.MixApplication:
                        var app = await ApplicationViewModel.GetRepository(_uow).GetSingleAsync(m => m.Id == alias.SourceContentId);
                        if (app != null)
                            return View("App", app);
                        break;
                }
            }
            return NotFound();
        }
    }
}
