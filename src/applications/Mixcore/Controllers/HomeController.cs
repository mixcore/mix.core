using Microsoft.AspNetCore.Mvc;
using Mix.Database.Services;
using Mix.Heart.Extensions;
using Mix.Lib.Interfaces;
using Mix.Lib.Services;
using Mix.RepoDb.Repositories;
using Mix.Services.Databases.Lib.Interfaces;
using Mix.Shared.Services;
using Mixcore.Domain.Bases;
using System.Linq.Expressions;

namespace Mixcore.Controllers
{
    public class HomeController : MvcBaseController
    {
        private readonly IMixMetadataService _metadataService;
        private readonly MixRepoDbRepository _repoDbRepository;
        private readonly MixCacheService _cacheService;

        public HomeController(
            IHttpContextAccessor httpContextAccessor,
            IPSecurityConfigService ipSecurityConfigService,
            MixService mixService,
            IMixCmsService mixCmsService,
            TranslatorService translator,
            DatabaseService databaseService,
            UnitOfWorkInfo<MixCmsContext> uow,
            MixRepoDbRepository repoDbRepository,
            IMixMetadataService metadataService,
            MixCacheService cacheService)
            : base(httpContextAccessor, ipSecurityConfigService, mixService, mixCmsService, translator, databaseService, uow)
        {
            _repoDbRepository = repoDbRepository;
            _metadataService = metadataService;
            _cacheService = cacheService;
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

        [Route("{seoName?}")]
        public async Task<IActionResult> Index([FromRoute] string seoName)
        {

            if (!IsValid)
            {
                return Redirect(RedirectUrl);
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
            var pageRepo = PageContentViewModel.GetRepository(Uow);
            pageRepo.CacheService = _cacheService;
            Expression<Func<MixPageContent, bool>> predicate = p => p.MixTenantId == CurrentTenant.Id
                    && p.Specificulture == Culture;
            predicate = predicate.AndAlsoIf(string.IsNullOrEmpty(seoName), m => m.Type == MixPageType.Home);
            predicate = predicate.AndAlsoIf(!string.IsNullOrEmpty(seoName), m => m.SeoName == seoName);
            var page = await pageRepo.GetFirstAsync(predicate);

            if (page != null)
            {
                await page.LoadDataAsync(_repoDbRepository, _metadataService, new(Request)
                {
                    SortBy = MixQueryColumnName.Priority
                }, _cacheService);

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
            var alias = await MixUrlAliasViewModel.GetRepository(Uow).GetSingleAsync(m => m.MixTenantId == CurrentTenant.Id && m.Alias == seoName);
            if (alias != null)
            {
                switch (alias.Type)
                {
                    case MixUrlAliasType.Page:
                        var pageRepo = PageContentViewModel.GetRepository(Uow);
                        pageRepo.CacheService = _cacheService;
                        var page = await pageRepo.GetSingleAsync(m => m.Id == alias.SourceContentId);
                        if (page != null)
                        {
                            await page.LoadDataAsync(_repoDbRepository, _metadataService, new(Request)
                            {
                                SortBy = MixQueryColumnName.Priority
                            }, _cacheService);
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
                        var postRepo = PostContentViewModel.GetRepository(Uow);
                        postRepo.CacheService = _cacheService;
                        var post = await postRepo.GetSingleAsync(m => m.Id == alias.SourceContentId);
                        if (post != null)
                            return View("Post", post);
                        break;
                    case MixUrlAliasType.Module:
                        break;
                    case MixUrlAliasType.ModuleData:
                        break;
                    case MixUrlAliasType.MixApplication:
                        var appRepo = ApplicationViewModel.GetRepository(Uow);
                        appRepo.CacheService = _cacheService;
                        var app = await appRepo.GetSingleAsync(m => m.Id == alias.SourceContentId);
                        if (app != null)
                            return View("App", app);
                        break;
                }
            }
            return NotFound();
        }
    }
}
