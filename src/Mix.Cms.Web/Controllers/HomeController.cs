using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Mix.Cms.Lib;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Cms.Web.Models;
using Mix.Domain.Core.ViewModels;
using Mix.Identity.Models;
using static Mix.Cms.Lib.MixEnums;

namespace Mix.Cms.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IApiDescriptionGroupCollectionProvider _apiExplorer;
        IApplicationLifetime _lifetime;
        public HomeController(IHostingEnvironment env,
            IMemoryCache memoryCache,
             UserManager<ApplicationUser> userManager,
             IApiDescriptionGroupCollectionProvider apiExplorer,
            IHttpContextAccessor accessor,
            IApplicationLifetime lifetime
            ) : base(env, memoryCache, accessor)
        {

            this._userManager = userManager;
            _apiExplorer = apiExplorer;
            _lifetime = lifetime;
        }

        #region Routes
        [Route("")]
        public async System.Threading.Tasks.Task<IActionResult> Index()
        {            
            if (_forbidden)
            {
                return Redirect($"/error/403");
            }
            if (MixService.GetConfig<bool>("IsMaintenance"))
            {
                return Redirect($"/{_culture}/maintenance");
            }

            if (MixService.GetConfig<bool>("IsInit"))
            {
                //Go to landing page
                return Redirect($"/{_culture}");
            }
            else
            {
                if (string.IsNullOrEmpty(MixService.GetConnectionString(MixConstants.CONST_CMS_CONNECTION)))
                {
                    return Redirect("Init");
                }
                else
                {
                    return Redirect($"/init/step2");
                }
            }
        }

        [Route("{culture}")]
        [Route("{culture}/{alias}")]
        public async System.Threading.Tasks.Task<IActionResult> Alias(string culture,
            string alias, int pageIndex, int pageSize = 10)
        {
            string seoName = Request.Query["alias"];
            seoName = seoName ?? alias;
            if (_forbidden)
            {
                return Redirect($"/error/403");
            }
            if (MixService.GetConfig<bool>("IsMaintenance"))
            {
                return await PageAsync("maintenance");
            }

            if (MixService.GetConfig<bool>("IsInit"))
            {
                //Go to landing page
                return await AliasAsync(seoName);
            }
            else
            {
                if (string.IsNullOrEmpty(MixService.GetConnectionString(MixConstants.CONST_CMS_CONNECTION)))
                {
                    return Redirect("Init");
                }
                else
                {
                    return Redirect($"/init/step2");
                }
            }
        }

        [Route("{culture}/page")]
        [Route("{culture}/page/{seoName}")]
        public async System.Threading.Tasks.Task<IActionResult> Page(
            string culture, string seoName)
        {
            if (_forbidden)
            {
                return Redirect($"/error/403");
            }
            if (MixService.GetConfig<bool>("IsMaintenance"))
            {
                return await PageAsync("maintenance");
            }

            if (MixService.GetConfig<bool>("IsInit"))
            {
                //Go to landing page
                return await PageAsync(seoName);
            }
            else
            {
                var initStatus = MixService.GetConfig<int>("InitStatus");
                switch (initStatus)
                {
                    case 0:
                        return Redirect("Init");
                    case 1:
                        return Redirect($"/init/step2");
                    case 2:
                        return Redirect($"/init/step3");
                    case 3:
                        return Redirect($"/init/step4");
                    case 4:
                        return Redirect($"/init/step5");
                    default:
                        return Error();
                }
            }
        }

        [Route("{culture}/tag/{tagName}")]
        public async System.Threading.Tasks.Task<IActionResult> Tag(string culture, string tagName)
        {
            if (_forbidden)
            {
                return Redirect($"/error/403");
            }
            return await TagAsync(tagName);
        }

        /**
         * Form action must be "{culture}/search/"
         **/
        [HttpGet]
        [Route("{culture}/search/")]
        public async System.Threading.Tasks.Task<IActionResult> Search()
        {
            string keyword = Request.Query["keyword"].ToString();
            if (_forbidden)
            {
                return Redirect($"/error/403");
            }
            return await SearchAsync(keyword);
        }

        [Route("post/{id}/{seoName}")]
        [Route("{culture}/post/{id}/{seoName}")]
        public async System.Threading.Tasks.Task<IActionResult> Post(int id, string culture, string seoName)
        {
            if (_forbidden)
            {
                return Redirect($"/error/403");
            }
            return await PostViewAsync(id, seoName);
        }

        [HttpGet]
        [Authorize]
        [Route("portal")]
        [Route("admin")]
        [Route("portal/page/{type}")]
        [Route("portal/post/{type}")]
        [Route("portal/{pageName}")]
        [Route("portal/{pageName}/{type}")]
        [Route("portal/{pageName}/{type}/{param}")]
        [Route("portal/{pageName}/{type}/{param}/{param1}")]
        [Route("portal/{pageName}/{type}/{param}/{param1}/{param2}")]
        [Route("portal/{pageName}/{type}/{param}/{param1}/{param2}/{param3}")]
        [Route("portal/{pageName}/{type}/{param}/{param1}/{param2}/{param3}/{param4}")]
        public IActionResult Portal()
        {
            if (_forbiddenPortal)
            {
                return Redirect($"/error/403");
            }
            return View();
        }

        [HttpGet]
        [Route("init")]
        [Route("init/{page}")]
        public IActionResult Init(string page)
        {
            if (_forbidden)
            {
                return Redirect($"/error/403");
            }
            if (MixService.GetConfig<bool>("IsInit"))
            {
                return Redirect("/");
            }
            else
            {
                page = page ?? "";
                var initStatus = MixService.GetConfig<int>("InitStatus");
                switch (initStatus)
                {
                    case 0:
                        if (page.ToLower() != "")
                        {
                            return Redirect($"/init");
                        }
                        break;
                    case 1:
                        if (page.ToLower() != "step2")
                        {
                            return Redirect($"/init/step2");
                        }
                        break;
                    case 2:
                        if (page.ToLower() != "step3")
                        {
                            return Redirect($"/init/step3");
                        }
                        break;
                    case 3:
                        if (page.ToLower() != "step4")
                        {
                            return Redirect($"/init/step4");
                        }
                        break;
                    case 4:
                        if (page.ToLower() != "step5")
                        {
                            return Redirect($"/init/step5");
                        }
                        break;

                }
                return View();
            }

        }
        [HttpGet]
        [Route("security/{page}")]
        public IActionResult Security(string page)
        {
            if (_forbidden)
            {
                return Redirect($"/error/403");
            }
            if (string.IsNullOrEmpty(page) && MixService.GetConfig<bool>("IsInit"))
            {
                return Redirect($"/init/login");
            }
            else
            {
                return View();
            }

        }

        [HttpGet]
        [Route("error/{page}")]
        public async System.Threading.Tasks.Task<IActionResult> PageError(string page = "404")
        {
            return await PageAsync(page);
        }

        [HttpGet]
        [Route("maintenance")]
        public async System.Threading.Tasks.Task<IActionResult> Maintenance()
        {
            return await PageAsync("Maintenance");
        }

        #endregion
        #region Helper

        async System.Threading.Tasks.Task<IActionResult> AliasAsync(string seoName)
        {
            // Home Page
            int? pageSize = MixService.GetConfig<int?>("TagPageSize");
            string orderBy = MixService.GetConfig<string>("OrderBy");
            int orderDirection = MixService.GetConfig<int>("OrderDirection");
            int.TryParse(Request.Query["pageIndex"], out int pageIndex);
            var cacheKey = $"mvc_{_culture}_alias_{seoName}_{pageSize}_{pageIndex}_{orderBy}_{orderDirection}";

            RepositoryResponse<Lib.ViewModels.MixUrlAliases.UpdateViewModel> getAlias = null;

            if (MixService.GetConfig<bool>("IsCache"))
            {
                getAlias = await MixCacheService.GetAsync<RepositoryResponse<Lib.ViewModels.MixUrlAliases.UpdateViewModel>>(cacheKey);
            }
            if (getAlias == null)
            {
                Expression<Func<MixUrlAlias, bool>> predicate;

                predicate = p =>
                p.Alias == seoName
                && p.Status == (int)MixContentStatus.Published && p.Specificulture == _culture;

                getAlias = await Lib.ViewModels.MixUrlAliases.UpdateViewModel.Repository.GetSingleModelAsync(predicate);
                if (getAlias.IsSucceed)
                {
                    MixCacheService.SetAsync(cacheKey, getAlias);
                }

            }

            if (getAlias.IsSucceed)// && getPage.Data.View != null
            {
                switch (getAlias.Data.Type)
                {
                    case UrlAliasType.Page:
                        return await PageAsync(int.Parse(getAlias.Data.SourceId));
                    case UrlAliasType.Post:
                        return await PostViewAsync(int.Parse(getAlias.Data.SourceId));
                    case UrlAliasType.Module: // TODO: Create view for module
                    case UrlAliasType.ModuleData: // TODO: Create view for module data
                    default:
                        return await PageError();
                }
            }
            else
            {
                return await PageAsync(seoName);
            }
        }

        async System.Threading.Tasks.Task<IActionResult> PageAsync(string seoName)
        {

            // Home Page
            int? pageSize = MixService.GetConfig<int?>("TagPageSize");
            string orderBy = MixService.GetConfig<string>("OrderBy");
            int orderDirection = MixService.GetConfig<int>("OrderDirection");
            int.TryParse(Request.Query["pageIndex"], out int pageIndex);
            var cacheKey = $"mvc_{_culture}_page_{seoName}_{pageSize}_{pageIndex}_{orderBy}_{orderDirection}";
            RepositoryResponse<Lib.ViewModels.MixPages.ReadMvcViewModel> getPage = null;

            if (MixService.GetConfig<bool>("IsCache"))
            {
                getPage = await MixCacheService.GetAsync<RepositoryResponse<Lib.ViewModels.MixPages.ReadMvcViewModel>>(cacheKey);
            }

            // Try to get page if not cached yet
            if (getPage == null)
            {
                Expression<Func<MixPage, bool>> predicate;
                if (string.IsNullOrEmpty(seoName))
                {
                    predicate = p =>
                    p.Type == (int)MixPageType.Home
                    && p.Status == (int)MixContentStatus.Published && p.Specificulture == _culture;
                }
                else
                {
                    predicate = p =>
                    p.SeoName == seoName
                    && p.Status == (int)MixContentStatus.Published && p.Specificulture == _culture;
                }

                getPage = await Lib.ViewModels.MixPages.ReadMvcViewModel.Repository.GetSingleModelAsync(predicate);
                if (getPage.IsSucceed)
                {
                    if (getPage.Data != null)
                    {
                        getPage.Data.LoadData(pageIndex: pageIndex, pageSize: pageSize);
                    }
                    GeneratePageDetailsUrls(getPage.Data);
                    MixCacheService.SetAsync(cacheKey, getPage);
                }
            }

            if (getPage.IsSucceed)
            {
                ViewData["TopPages"] = await GetCategoryAsync(CatePosition.Nav, seoName);
                ViewData["HeaderPages"] = await GetCategoryAsync(CatePosition.Top, seoName);
                ViewData["FooterPages"] = await GetCategoryAsync(CatePosition.Footer, seoName);
                ViewData["LeftPages"] = await GetCategoryAsync(CatePosition.Left, seoName);

                ViewData["Title"] = getPage.Data.SeoTitle;
                ViewData["Description"] = getPage.Data.SeoDescription;
                ViewData["Keywords"] = getPage.Data.SeoKeywords;
                ViewData["Image"] = getPage.Data.ImageUrl;
                ViewData["PageClass"] = getPage.Data.CssClass;
                getPage.LastUpdateConfiguration = MixService.GetConfig<DateTime?>("LastUpdateConfiguration");
                return View(getPage.Data);
            }
            else
            {
                return Redirect($"/error/404");
            }
        }
        async System.Threading.Tasks.Task<IActionResult> PageAsync(int pageId)
        {
            // Home Page
            int? pageSize = MixService.GetConfig<int?>("TagPageSize");
            string orderBy = MixService.GetConfig<string>("OrderBy");
            int orderDirection = MixService.GetConfig<int>("OrderDirection");
            int.TryParse(Request.Query["pageIndex"], out int pageIndex);
            var cacheKey = $"mvc_{_culture}_page_{pageId}_{pageSize}_{pageIndex}_{orderBy}_{orderDirection}";
            RepositoryResponse<Lib.ViewModels.MixPages.ReadMvcViewModel> getPage = null;

            if (MixService.GetConfig<bool>("IsCache"))
            {
                getPage = await MixCacheService.GetAsync<RepositoryResponse<Lib.ViewModels.MixPages.ReadMvcViewModel>>(cacheKey);
            }
            if (getPage == null)
            {
                Expression<Func<MixPage, bool>> predicate;

                predicate = p =>
                p.Id == pageId
                && p.Status == (int)MixContentStatus.Published && p.Specificulture == _culture;

                getPage = await Lib.ViewModels.MixPages.ReadMvcViewModel.Repository.GetSingleModelAsync(predicate);
                if (getPage.IsSucceed)
                {
                    getPage.Data.LoadData(pageIndex: pageIndex, pageSize: pageSize);
                    getPage.Data.DetailsUrl = GenerateDetailsUrl(
                        new { culture = _culture, seoName = getPage.Data.SeoName }
                        );
                    GeneratePageDetailsUrls(getPage.Data);
                    MixCacheService.SetAsync(cacheKey, getPage);
                }
            }

            if (getPage.IsSucceed)
            {
                ViewData["TopPages"] = await GetCategoryAsync(CatePosition.Nav, getPage.Data.SeoName);
                ViewData["HeaderPages"] = await GetCategoryAsync(CatePosition.Top, getPage.Data.SeoName);
                ViewData["FooterPages"] = await GetCategoryAsync(CatePosition.Footer, getPage.Data.SeoName);
                ViewData["LeftPages"] = await GetCategoryAsync(CatePosition.Left, getPage.Data.SeoName);

                ViewData["Title"] = getPage.Data.SeoTitle;
                ViewData["Description"] = getPage.Data.SeoDescription;
                ViewData["Keywords"] = getPage.Data.SeoKeywords;
                ViewData["Image"] = getPage.Data.ImageUrl;
                ViewData["PageClass"] = getPage.Data.CssClass;

                getPage.LastUpdateConfiguration = MixService.GetConfig<DateTime?>("LastUpdateConfiguration");
                return View(getPage.Data);
            }
            else
            {
                return Redirect($"/error/404");
            }
        }

        async System.Threading.Tasks.Task<IActionResult> TagAsync(string tagName)
        {
            string seoName = "tag";
            ViewData["TopPages"] = await GetCategoryAsync(CatePosition.Nav, seoName);
            ViewData["HeaderPages"] = await GetCategoryAsync(CatePosition.Top, seoName);
            ViewData["FooterPages"] = await GetCategoryAsync(CatePosition.Footer, seoName);
            ViewData["LeftPages"] = await GetCategoryAsync(CatePosition.Left, seoName);
            ViewData["TagName"] = tagName;

            int? pageSize = MixService.GetConfig<int?>("TagPageSize");
            string orderBy = MixService.GetConfig<string>("OrderBy");
            int orderDirection = MixService.GetConfig<int>("OrderDirection");
            int.TryParse(Request.Query["pageIndex"], out int pageIndex);
            var cacheKey = $"mvc_{_culture}_tag_{tagName}_{pageSize}_{pageIndex}_{orderBy}_{orderDirection}";
            RepositoryResponse<Lib.ViewModels.MixPages.ReadMvcViewModel> getPage = null;
            

            if (MixService.GetConfig<bool>("IsCache"))
            {
                getPage = await MixCacheService.GetAsync<RepositoryResponse<Lib.ViewModels.MixPages.ReadMvcViewModel>>(cacheKey);
            }
            if (getPage == null)
            {
                Expression<Func<MixPage, bool>> predicate;

                predicate = p =>
                p.SeoName == "tag"
                && p.Status == (int)MixContentStatus.Published && p.Specificulture == _culture;

                getPage = await Lib.ViewModels.MixPages.ReadMvcViewModel.Repository.GetSingleModelAsync(predicate);
                if (getPage.IsSucceed)
                {
                    getPage.Data.LoadDataByTag(tagName, orderBy, orderDirection, pageIndex: pageIndex, pageSize: pageSize);
                    GeneratePageDetailsUrls(getPage.Data);
                    MixCacheService.SetAsync(cacheKey, getPage);
                }
            }

            if (getPage.IsSucceed)// && getPage.Data.View != null
            {
                ViewData["Title"] = getPage.Data.SeoTitle;
                ViewData["Description"] = getPage.Data.SeoDescription;
                ViewData["Keywords"] = getPage.Data.SeoKeywords;
                ViewData["Image"] = getPage.Data.ImageUrl;
                ViewData["PageClass"] = getPage.Data.CssClass;
                getPage.LastUpdateConfiguration = MixService.GetConfig<DateTime?>("LastUpdateConfiguration");
                return View(getPage.Data);
            }
            else
            {
                return Redirect($"/error/404");
            }
        }
        async System.Threading.Tasks.Task<IActionResult> SearchAsync(string keyword)
        {
            string seoName = "search";
            ViewData["TopPages"] = await GetCategoryAsync(CatePosition.Nav, seoName);
            ViewData["HeaderPages"] = await GetCategoryAsync(CatePosition.Top, seoName);
            ViewData["FooterPages"] = await GetCategoryAsync(CatePosition.Footer, seoName);
            ViewData["LeftPages"] = await GetCategoryAsync(CatePosition.Left, seoName);

            int? pageSize = MixService.GetConfig<int?>("SearchPageSize");
            string orderBy = MixService.GetConfig<string>("OrderBy");
            int orderDirection = MixService.GetConfig<int>("OrderDirection");
            int.TryParse(Request.Query["pageIndex"], out int pageIndex);
            RepositoryResponse<Lib.ViewModels.MixPages.ReadMvcViewModel> getPage = null;
            var cacheKey = $"mvc_{_culture}_search_{keyword}_{pageSize}_{pageIndex}_{orderBy}_{orderDirection}";

            // Try load data from cache
            if (MixService.GetConfig<bool>("IsCache"))
            {
                getPage = await MixCacheService.GetAsync<RepositoryResponse<Lib.ViewModels.MixPages.ReadMvcViewModel>>(cacheKey);
            }

            // If cannot load from cache => try query new data
            if (getPage == null)
            {
                Expression<Func<MixPage, bool>> predicate;

                predicate = p =>
                p.SeoName == "search"
                && p.Status == (int)MixContentStatus.Published && p.Specificulture == _culture;

                getPage = await Lib.ViewModels.MixPages.ReadMvcViewModel.Repository.GetSingleModelAsync(predicate);
                if (getPage.IsSucceed)
                {
                    GeneratePageDetailsUrls(getPage.Data);
                    getPage.Data.LoadDataByKeyword(keyword, orderBy, orderDirection, pageIndex: pageIndex, pageSize: pageSize);

                    MixCacheService.SetAsync(cacheKey, getPage);
                }
            }

            if (getPage.IsSucceed)
            {

                ViewData["Title"] = getPage.Data.SeoTitle;
                ViewData["Description"] = getPage.Data.SeoDescription;
                ViewData["Keywords"] = getPage.Data.SeoKeywords;
                ViewData["Image"] = getPage.Data.ImageUrl;
                ViewData["PageClass"] = getPage.Data.CssClass;
                getPage.LastUpdateConfiguration = MixService.GetConfig<DateTime?>("LastUpdateConfiguration");
                return View(getPage.Data);
            }
            else
            {
                return Redirect($"/error/404");
            }
        }

        async System.Threading.Tasks.Task<IActionResult> PostViewAsync(int id)
        {

            RepositoryResponse<Lib.ViewModels.MixPosts.ReadMvcViewModel> getPost = null;
            var cacheKey = $"mvc_{_culture}_post_{id}";
            if (MixService.GetConfig<bool>("IsCache"))
            {
                getPost = await MixCacheService.GetAsync<RepositoryResponse<Lib.ViewModels.MixPosts.ReadMvcViewModel>>(cacheKey);
            }
            if (getPost == null)
            {
                Expression<Func<MixPost, bool>> predicate;
                predicate = p =>
                p.Id == id
                && p.Status == (int)MixContentStatus.Published
                && p.Specificulture == _culture;

                getPost = await Lib.ViewModels.MixPosts.ReadMvcViewModel.Repository.GetSingleModelAsync(predicate);
                if (getPost.IsSucceed)
                {
                    getPost.Data.DetailsUrl = GenerateDetailsUrl(                        
                        new { culture = _culture, action = "post", id = getPost.Data.Id, seoName = getPost.Data.SeoName }
                        );
                    //Generate details url for related posts
                    if (getPost.IsSucceed)
                    {
                        if (getPost.Data.PostNavs != null && getPost.Data.PostNavs.Count > 0)
                        {
                            getPost.Data.PostNavs.ForEach(n => n.RelatedPost.DetailsUrl = GenerateDetailsUrl(
                                new { culture = _culture, action = "post", id = n.RelatedPost.Id, seoName = n.RelatedPost.SeoName }));
                        }
                        MixCacheService.SetAsync(cacheKey, getPost);
                    }
                }

            }

            if (getPost.IsSucceed)
            {
                ViewData["TopPages"] = await GetCategoryAsync(CatePosition.Nav, getPost.Data.SeoName);
                ViewData["HeaderPages"] = await GetCategoryAsync(CatePosition.Top, getPost.Data.SeoName);
                ViewData["FooterPages"] = await GetCategoryAsync(CatePosition.Footer, getPost.Data.SeoName);
                ViewData["LeftPages"] = await GetCategoryAsync(CatePosition.Left, getPost.Data.SeoName);

                ViewData["Title"] = getPost.Data.SeoTitle;
                ViewData["Description"] = getPost.Data.SeoDescription;
                ViewData["Keywords"] = getPost.Data.SeoKeywords;
                ViewData["Image"] = getPost.Data.ImageUrl;
                getPost.LastUpdateConfiguration = MixService.GetConfig<DateTime?>("LastUpdateConfiguration");
                return View(getPost.Data);
            }
            else
            {
                return Redirect($"/error/404");
            }
        }
        async System.Threading.Tasks.Task<IActionResult> PostViewAsync(int id, string seoName)
        {
            ViewData["TopPages"] = await GetCategoryAsync(CatePosition.Nav, seoName);
            ViewData["HeaderPages"] = await GetCategoryAsync(CatePosition.Top, seoName);
            ViewData["FooterPages"] = await GetCategoryAsync(CatePosition.Footer, seoName);
            ViewData["LeftPages"] = await GetCategoryAsync(CatePosition.Left, seoName);

            RepositoryResponse<Lib.ViewModels.MixPosts.ReadMvcViewModel> getPost = null;

            var cacheKey = $"mvc_{_culture}_post_{seoName}";

            if (MixService.GetConfig<bool>("IsCache"))
            {
                getPost = await MixCacheService.GetAsync<RepositoryResponse<Lib.ViewModels.MixPosts.ReadMvcViewModel>>(cacheKey);

            }

            if (getPost == null)
            {
                Expression<Func<MixPost, bool>> predicate;
                if (string.IsNullOrEmpty(seoName))
                {
                    predicate = p =>
                    p.Type == (int)MixPageType.Home
                    && p.Status == (int)MixContentStatus.Published && p.Specificulture == _culture;
                }
                else
                {
                    predicate = p =>
                    p.Id == id
                    && p.Status == (int)MixContentStatus.Published
                    && p.Specificulture == _culture;
                }

                getPost = await Lib.ViewModels.MixPosts.ReadMvcViewModel.Repository.GetSingleModelAsync(predicate);
                if (getPost.IsSucceed)
                {
                    getPost.Data.DetailsUrl = GenerateDetailsUrl(
                        new { culture = _culture, action = "post", id = getPost.Data.Id, seoName = getPost.Data.SeoName });
                    //Generate details url for related posts
                    if (getPost.Data.PostNavs != null && getPost.Data.PostNavs.Count > 0)
                    {
                        getPost.Data.PostNavs.ForEach(n => n.RelatedPost.DetailsUrl = GenerateDetailsUrl(
                                new { culture = _culture, action = "post", id = n.RelatedPost.Id, seoName = n.RelatedPost.SeoName }));
                    }
                    MixCacheService.SetAsync(cacheKey, getPost);
                }
            }

            if (getPost.IsSucceed)
            {
                ViewData["Title"] = getPost.Data.SeoTitle;
                ViewData["Description"] = getPost.Data.SeoDescription;
                ViewData["Keywords"] = getPost.Data.SeoKeywords;
                ViewData["Image"] = getPost.Data.ImageUrl;
                getPost.LastUpdateConfiguration = MixService.GetConfig<DateTime?>("LastUpdateConfiguration");
                return View(getPost.Data);
            }
            else
            {
                return Redirect($"/error/404");
            }
        }

        void GeneratePageDetailsUrls(Lib.ViewModels.MixPages.ReadMvcViewModel page)
        {
            page.DetailsUrl = GenerateDetailsUrl(
                new { culture = _culture, action = "alias", seoName = page.SeoName });
            if (page.Posts != null)
            {
                foreach (var postNav in page.Posts.Items)
                {
                    if (postNav.Post != null)
                    {
                        postNav.Post.DetailsUrl = GenerateDetailsUrl(new { culture = _culture, action = "post", id = postNav.PostId, seoName = postNav.Post.SeoName });
                    }
                }
            }

            if (page.Modules != null)
            {
                foreach (var nav in page.Modules)
                {
                    GeneratePageDetailsUrls(nav.Module);
                }
            }
        }

        void GeneratePageDetailsUrls(Lib.ViewModels.MixModules.ReadMvcViewModel module)
        {
            if (module.Posts != null)
            {

                foreach (var postNav in module.Posts.Items)
                {
                    if (postNav.Post != null)
                    {
                        postNav.Post.DetailsUrl = GenerateDetailsUrl(
                            new {culture = _culture, action = "post", id = postNav.PostId, seoName = postNav.Post.SeoName }
                            );
                    }
                }
            }
        }

        string GenerateDetailsUrl(object routeValues)
        {
            return MixCmsHelper.GetRouterUrl(routeValues, Request, Url);
        }

        #endregion
        async System.Threading.Tasks.Task<List<Lib.ViewModels.MixPages.ReadListItemViewModel>> GetCategoryAsync(MixEnums.CatePosition position, string seoName)
        {
            List<Lib.ViewModels.MixPages.ReadListItemViewModel> result = null;
            var cacheKey = $"mvc_menus_{position}";

            if (MixService.GetConfig<bool>("IsCache"))
            {
                result = await MixCacheService.GetAsync<List<Lib.ViewModels.MixPages.ReadListItemViewModel>>(cacheKey);
            }
            if (result == null)
            {
                var getTopCates = Lib.ViewModels.MixPages.ReadListItemViewModel.Repository.GetModelListBy
            (c => c.Specificulture == _culture && c.MixPagePosition.Any(
              p => p.PositionId == (int)position)
            );

                result = getTopCates.Data ?? new List<Lib.ViewModels.MixPages.ReadListItemViewModel>();
                foreach (var cate in result)
                {
                    switch (cate.Type)
                    {
                        case MixPageType.Blank:
                            foreach (var child in cate.Childs)
                            {
                                child.Page.DetailsUrl = GenerateDetailsUrl(
                                    new { culture = _culture, seoName = child.Page.SeoName });
                            }
                            break;

                        case MixPageType.StaticUrl:
                            cate.DetailsUrl = cate.StaticUrl;
                            break;

                        case MixPageType.Home:
                        case MixPageType.ListPost:
                        case MixPageType.Post:
                        case MixPageType.Modules:
                        default:
                            cate.DetailsUrl = GenerateDetailsUrl(
                                new { culture = _culture, seoName = cate.SeoName }
                                );
                            break;
                    }
                }

            }

            foreach (var cate in result)
            {
                cate.IsActived = (cate.SeoName == seoName
                    || (cate.Type == MixPageType.Home && string.IsNullOrEmpty(seoName)));
                cate.Childs.ForEach((Action<Lib.ViewModels.MixPagePages.ReadViewModel>)(c =>
                {
                    c.IsActived = (
                    c.Page.SeoName == seoName);
                    cate.IsActived = cate.IsActived || c.IsActived;
                }));
            }
            return result;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
