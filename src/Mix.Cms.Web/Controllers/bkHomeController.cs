using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Caching.Memory;
using Mix.Cms.Lib;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Cms.Web.Models;
using Mix.Domain.Core.ViewModels;
using Mix.Identity.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static Mix.Cms.Lib.MixEnums;

namespace Mix.Cms.Web.Controllers
{
    public class bkHomeController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IApiDescriptionGroupCollectionProvider _apiExplorer;
        IApplicationLifetime _lifetime;
        public bkHomeController(IHostingEnvironment env,
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
        [Route("bk")]
        public IActionResult Index()
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

        [Route("bk/{culture}")]
        [Route("bk/{culture}/{alias}")]
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

        [Route("bk/{culture}/page")]
        [Route("bk/{culture}/page/{seoName}")]
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

        [Route("bk/{culture}/tag/{tagName}")]
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
        [Route("bk/{culture}/search/")]
        public async System.Threading.Tasks.Task<IActionResult> Search()
        {
            string keyword = Request.Query["keyword"].ToString();
            if (_forbidden)
            {
                return Redirect($"/error/403");
            }
            return await SearchAsync(keyword);
        }

        [Route("bk/post/{id}/{seoName}")]
        [Route("bk/{culture}/post/{id}/{seoName}")]
        public async System.Threading.Tasks.Task<IActionResult> Post(int id, string culture, string seoName)
        {
            if (_forbidden)
            {
                return Redirect($"/error/403");
            }
            return await Post(id, seoName);
        }

        [Route("bk/md/{id}/{seoName}")]
        [Route("bk/{culture}/md/{id}/{seoName}")]
        public async System.Threading.Tasks.Task<IActionResult> Module(int id, string culture, string seoName)
        {
            if (_forbidden)
            {
                return Redirect($"/error/403");
            }
            return await Module(id, seoName);
        }

        [HttpGet]
        [Authorize]
        [Route("bk/portal")]
        [Route("bk/admin")]
        [Route("bk/portal/page/{type}")]
        [Route("bk/portal/post/{type}")]
        [Route("bk/portal/{pageName}")]
        [Route("bk/portal/{pageName}/{type}")]
        [Route("bk/portal/{pageName}/{type}/{param}")]
        [Route("bk/portal/{pageName}/{type}/{param}/{param1}")]
        [Route("bk/portal/{pageName}/{type}/{param}/{param1}/{param2}")]
        [Route("bk/portal/{pageName}/{type}/{param}/{param1}/{param2}/{param3}")]
        [Route("bk/portal/{pageName}/{type}/{param}/{param1}/{param2}/{param3}/{param4}")]
        public IActionResult Portal()
        {
            if (_forbiddenPortal)
            {
                return Redirect($"/error/403");
            }
            return View();
        }

        [HttpGet]
        [Route("bk/init")]
        [Route("bk/init/{page}")]
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
        [Route("bk/security/{page}")]
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
        [Route("bk/error/{page}")]
        public async System.Threading.Tasks.Task<IActionResult> PageError(string page = "404")
        {
            return await PageAsync(page);
        }

        [HttpGet]
        [Route("bk/maintenance")]
        public async System.Threading.Tasks.Task<IActionResult> Maintenance()
        {
            return await PageAsync("Maintenance");
        }

        #endregion

        #region Helper

        async Task<IActionResult> AliasAsync(string seoName)
        {
            // Home Page
            RepositoryResponse<Lib.ViewModels.MixUrlAliases.UpdateViewModel> getAlias = null;

            if (getAlias == null)
            {
                Expression<Func<MixUrlAlias, bool>> predicate;

                predicate = p =>
                p.Alias == seoName
                && p.Status == (int)MixContentStatus.Published && p.Specificulture == _culture;

                getAlias = await Lib.ViewModels.MixUrlAliases.UpdateViewModel.Repository.GetSingleModelAsync(predicate);
                //if (getAlias.IsSucceed)
                //{
                //    _ = Task.FromResult(MixCacheService.SetAsync(cacheKey, getAlias));
                //}

            }

            if (getAlias.IsSucceed)// && getPage.Data.View != null
            {
                switch (getAlias.Data.Type)
                {
                    case UrlAliasType.Page:
                        return await PageAsync(int.Parse(getAlias.Data.SourceId));
                    case UrlAliasType.Post:
                        return await Post(int.Parse(getAlias.Data.SourceId));
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
            int maxPageSize = MixService.GetConfig<int>("MaxPageSize");
            string orderBy = MixService.GetConfig<string>("OrderBy");
            int orderDirection = MixService.GetConfig<int>("OrderDirection");
            int.TryParse(Request.Query["page"], out int page);
            int.TryParse(Request.Query["pageSize"], out int pageSize);
            pageSize = (pageSize < maxPageSize) ? pageSize : maxPageSize;

            RepositoryResponse<Lib.ViewModels.MixPages.ReadMvcViewModel> getPage = null;
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
                    getPage.Data.LoadData(pageSize: pageSize, pageIndex: page - 1);
                }
                GeneratePageDetailsUrls(getPage.Data);
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
        async System.Threading.Tasks.Task<IActionResult> PageAsync(int pageId)
        {
            // Home Page
            int? pageSize = MixService.GetConfig<int?>("TagPageSize");
            string orderBy = MixService.GetConfig<string>("OrderBy");
            int orderDirection = MixService.GetConfig<int>("OrderDirection");
            int.TryParse(Request.Query["page"], out int page);
            RepositoryResponse<Lib.ViewModels.MixPages.ReadMvcViewModel> getPage = null;

            Expression<Func<MixPage, bool>> predicate;

            predicate = p =>
            p.Id == pageId
            && p.Status == (int)MixContentStatus.Published && p.Specificulture == _culture;

            getPage = await Lib.ViewModels.MixPages.ReadMvcViewModel.Repository.GetSingleModelAsync(predicate);
            if (getPage.IsSucceed)
            {
                getPage.Data.LoadData(pageIndex: page - 1, pageSize: pageSize);
                getPage.Data.DetailsUrl = GenerateDetailsUrl(
                    new { culture = _culture, seoName = getPage.Data.SeoName }
                    );
                GeneratePageDetailsUrls(getPage.Data);
                //_ = MixCacheService.SetAsync(cacheKey, getPage);
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

        async System.Threading.Tasks.Task<IActionResult> TagAsync(string tagName)
        {
            string seoName = "tag";
            ViewData["TagName"] = tagName;

            int? pageSize = MixService.GetConfig<int?>("TagPageSize");
            string orderBy = MixService.GetConfig<string>("OrderBy");
            int orderDirection = MixService.GetConfig<int>("OrderDirection");
            int.TryParse(Request.Query["page"], out int page);
            Expression<Func<MixPage, bool>> predicate = p =>
            p.SeoName == seoName
            && p.Status == (int)MixContentStatus.Published && p.Specificulture == _culture;

            var getPage = await Lib.ViewModels.MixPages.ReadMvcViewModel.Repository.GetSingleModelAsync(predicate);
            var getPosts = await Lib.ViewModels.MixPosts.Helper.GetModelistByMeta<Lib.ViewModels.MixPosts.ReadListItemViewModel>(
                _culture, MixConstants.AttributeSetName.SYSTEM_TAG, tagName, orderBy, orderDirection, pageSize, page-1);
            if (getPage.IsSucceed)// && getPage.Data.View != null
            {
                ViewData["Title"] = $"Tag: {tagName}";
                ViewData["Description"] = $"Tag: {tagName}";
                ViewData["Keywords"] = $"Tag: {tagName}";
                ViewData["Image"] = getPage.Data.ImageUrl;
                ViewData["PageClass"] = getPage.Data.CssClass;
                ViewData["Posts"] = getPosts.Data;
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

            int? pageSize = MixService.GetConfig<int?>("SearchPageSize");
            string orderBy = MixService.GetConfig<string>("OrderBy");
            int orderDirection = MixService.GetConfig<int>("OrderDirection");
            int.TryParse(Request.Query["page"], out int page);
            RepositoryResponse<Lib.ViewModels.MixPages.ReadMvcViewModel> getPage = null;
            
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
                    getPage.Data.LoadDataByKeyword(keyword, orderBy, orderDirection, pageIndex: page-1, pageSize: pageSize);

                    //_ = MixCacheService.SetAsync(cacheKey, getPage);
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

        async System.Threading.Tasks.Task<IActionResult> Post(int id)
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
                        //_ = MixCacheService.SetAsync(cacheKey, getPost);
                    }
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
        async System.Threading.Tasks.Task<IActionResult> Post(int id, string seoName)
        {
            RepositoryResponse<Lib.ViewModels.MixPosts.ReadMvcViewModel> getPost = null;

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
        
        async System.Threading.Tasks.Task<IActionResult> Module(int id, string seoName)
        {
            RepositoryResponse<Lib.ViewModels.MixModules.ReadMvcViewModel> getPost = null;

            Expression<Func<MixModule, bool>> predicate;
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

            getPost = await Lib.ViewModels.MixModules.ReadMvcViewModel.Repository.GetSingleModelAsync(predicate);
            if (getPost.IsSucceed)
            {
                getPost.Data.DetailsUrl = GenerateDetailsUrl(
                    new { culture = _culture, action = "post", id = getPost.Data.Id, seoName = getPost.Data.Name });
                //Generate details url for related posts
                if (getPost.Data.Posts != null && getPost.Data.Posts.TotalItems > 0)
                {
                    getPost.Data.Posts.Items.ForEach(n => n.Post.DetailsUrl = GenerateDetailsUrl(
                            new { culture = _culture, action = "post", id = n.Post.Id, seoName = n.Post.SeoName }));
                }
            }

            if (getPost.IsSucceed)
            {
                ViewData["Title"] = getPost.Data.Title;
                ViewData["Description"] = getPost.Data.Title;
                ViewData["Keywords"] = getPost.Data.Title;
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
            page.DetailsUrl = MixCmsHelper.GetRouterUrl(
                                   new { culture = _culture, seoName = page.SeoName }, Request, Url);
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
            module.DetailsUrl = GenerateDetailsUrl(
                            new { culture = _culture, action = "md", id = module.Id, seoName = module.Name }
                            );
            if (module.Posts != null)
            {

                foreach (var postNav in module.Posts.Items)
                {
                    if (postNav.Post != null)
                    {
                        postNav.Post.DetailsUrl = GenerateDetailsUrl(
                            new { culture = _culture, action = "post", id = postNav.PostId, seoName = postNav.Post.SeoName }
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
