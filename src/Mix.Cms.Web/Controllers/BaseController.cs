using AutoMapper.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Helpers;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Heart.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mix.Cms.Web.Controllers
{
    public class BaseController : Controller
    {
        protected string domain;
        protected bool forbidden = false;
        protected bool isValid = true;
        protected string _redirectUrl;

        protected bool _forbiddenPortal
        {
            get
            {
                var allowedIps = MixService.GetIpConfig<JArray>("AllowedPortalIps") ?? new JArray();
                string remoteIp = Request.HttpContext?.Connection?.RemoteIpAddress?.ToString();
                return forbidden || (
                        // add in allowedIps "::1" to allow localhost
                        allowedIps.Count > 0 &&
                        !allowedIps.Any(t => t["text"].Value<string>() == remoteIp)
                );
            }
        }

        protected IConfiguration _configuration;

        public BaseController()
        {
            
        }

        private void LoadCulture()
        {
            if (RouteData?.Values["culture"]?.ToString().ToLower() is not null)
            {
                culture = RouteData?.Values["culture"]?.ToString().ToLower();
            }
            if (!MixService.Instance.CheckValidCulture(culture))
            {
                culture = MixService.GetAppSetting<string>(MixAppSettingKeywords.DefaultCulture);
            }

            // Set CultureInfo
            var cultureInfo = new CultureInfo(culture);
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
        }

        public ViewContext ViewContext { get; set; }
        private string _culture;

        public string culture
        {
            get
            {
                return _culture;
            }
            set { _culture = value; }
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ValidateRequest();

            ViewBag.culture = culture;
            if (!string.IsNullOrEmpty(culture))
            {
                ViewBag.assetFolder = MixCmsHelper.GetAssetFolder(culture);
            }
            domain = string.Format("{0}://{1}", Request.Scheme, Request.Host);
            if (MixService.GetIpConfig<bool>("IsRetrictIp"))
            {
                var allowedIps = MixService.GetIpConfig<JArray>("AllowedIps") ?? new JArray();
                var exceptIps = MixService.GetIpConfig<JArray>("ExceptIps") ?? new JArray();
                string remoteIp = Request.HttpContext?.Connection?.RemoteIpAddress?.ToString();
                if (
                        // To allow localhost remove below comment
                        //remoteIp != "::1" &&
                        allowedIps.Count > 0 &&
                        !allowedIps.Any(t => t["text"].Value<string>() == remoteIp) ||
                        (
                            exceptIps.Count > 0 &&
                            exceptIps.Any(t => t["text"].Value<string>() == remoteIp)
                        )
                    )
                {
                    forbidden = true;
                }
            }
        }

        protected virtual void ValidateRequest()
        {
            if (!MixService.GetAppSetting<bool>(MixAppSettingKeywords.IsInit))
            {
                LoadCulture();
            }

            // If IP retricted in appsettings
            if (forbidden)
            {
                isValid = false;
                _redirectUrl = $"/403";
            }

            // If mode Maintenance enabled in appsettings
            if (MixService.GetAppSetting<bool>("IsMaintenance") 
                && Request.RouteValues["seoName"]?.ToString() != "maintenance"
                && Request.RouteValues["controller"]?.ToString().ToLower() != "portal"
                && Request.RouteValues["controller"]?.ToString().ToLower() != "security")
            {
                isValid = false;
                _redirectUrl = $"/maintenance";
            }
        }

        #region Helper

        protected async Task<IActionResult> AliasAsync(string seoName, string keyword = null)
        {
            // Home Page

            // If page name is null => return home page
            if (string.IsNullOrEmpty(seoName))
            {
                return await Page(seoName);
            }
            else
            {
                RepositoryResponse<Lib.ViewModels.MixUrlAliases.UpdateViewModel> getAlias = null;

                Expression<Func<MixUrlAlias, bool>> predicate;

                predicate = p =>
                p.Alias == seoName
                && p.Status == MixContentStatus.Published && p.Specificulture == culture;

                getAlias = await Lib.ViewModels.MixUrlAliases.UpdateViewModel.Repository.GetFirstModelAsync(predicate);
                if (getAlias.IsSucceed)// && getPage.Data.View != null
                {
                    switch (getAlias.Data.Type)
                    {
                        case MixUrlAliasType.Page:
                            return await Page(int.Parse(getAlias.Data.SourceId), keyword);

                        case MixUrlAliasType.Post:
                            return await Post(int.Parse(getAlias.Data.SourceId));

                        case MixUrlAliasType.Module: // TODO: Create view for module
                        case MixUrlAliasType.ModuleData: // TODO: Create view for module data
                        default:
                            return await Page(0);
                    }
                }
                else
                {
                    return await Page(seoName, keyword);
                }
            }
        }

        protected async System.Threading.Tasks.Task<IActionResult> Page(string seoName, string keyword = null)
        {
            // Home Page
            int maxPageSize = MixService.GetAppSetting<int>("MaxPageSize");
            string orderBy = MixService.GetAppSetting<string>("OrderBy");
            int orderDirection = MixService.GetAppSetting<int>("OrderDirection");
            int.TryParse(Request.Query["page"], out int page);
            int.TryParse(Request.Query["pageSize"], out int pageSize);
            ViewData["keyword"] = keyword;
            RepositoryResponse<Lib.ViewModels.MixPages.ReadMvcViewModel> getPage = null;
            Expression<Func<MixPage, bool>> predicate;

            if (string.IsNullOrEmpty(seoName))
            {
                predicate = p =>
                p.Type == MixPageType.Home
                && p.Status == MixContentStatus.Published && p.Specificulture == culture;
            }
            else
            {
                predicate = p =>
                p.SeoName == seoName
                && p.Status == MixContentStatus.Published && p.Specificulture == culture;
            }

            getPage = await Lib.ViewModels.MixPages.ReadMvcViewModel.Repository.GetFirstModelAsync(predicate);

            if (getPage.IsSucceed)
            {
                if (getPage.Data != null)
                {
                    maxPageSize = getPage.Data.PageSize.HasValue ? getPage.Data.PageSize.Value : maxPageSize;
                    pageSize = (pageSize > 0 && pageSize < maxPageSize) ? pageSize : maxPageSize;
                    getPage.Data.LoadData(pageSize: pageSize, pageIndex: page - 1);
                }
                GeneratePageDetailsUrls(getPage.Data);
            }

            if (getPage.IsSucceed)
            {
                ViewData["Title"] = getPage.Data.SeoTitle;
                ViewData["Name"] = getPage.Data.SeoName;
                ViewData["Description"] = getPage.Data.SeoDescription;
                ViewData["Keywords"] = getPage.Data.SeoKeywords;
                ViewData["Image"] = getPage.Data.ImageUrl;
                ViewData["BodyClass"] = getPage.Data.CssClass;
                ViewData["ViewMode"] = MixMvcViewMode.Page;
                ViewData["Keyword"] = keyword;
                getPage.LastUpdateConfiguration = MixService.GetAppSetting<DateTime?>("LastUpdateConfiguration");
                return View(getPage.Data);
            }
            else
            {
                if (seoName != "404")
                {
                    return Redirect($"/{culture}/404");
                }
                else
                {
                    return NotFound();
                }
            }
        }

        protected async System.Threading.Tasks.Task<IActionResult> Page(int pageId, string keyword = null)
        {
            // Home Page
            int? pageSize = MixService.GetAppSetting<int?>("TagPageSize");
            string orderBy = MixService.GetAppSetting<string>("OrderBy");
            int orderDirection = MixService.GetAppSetting<int>("OrderDirection");
            int.TryParse(Request.Query["page"], out int page);
            RepositoryResponse<Lib.ViewModels.MixPages.ReadMvcViewModel> getPage = null;

            Expression<Func<MixPage, bool>> predicate;

            predicate = p =>
            p.Id == pageId
            && p.Status == MixContentStatus.Published && p.Specificulture == culture;

            getPage = await Lib.ViewModels.MixPages.ReadMvcViewModel.Repository.GetFirstModelAsync(predicate);
            if (getPage.IsSucceed)
            {
                getPage.Data.LoadData(pageIndex: page - 1, pageSize: pageSize);
                GeneratePageDetailsUrls(getPage.Data);
            }

            if (getPage.IsSucceed)
            {
                ViewData["Title"] = getPage.Data.SeoTitle;
                ViewData["Description"] = getPage.Data.SeoDescription;
                ViewData["Keywords"] = getPage.Data.SeoKeywords;
                ViewData["Image"] = getPage.Data.ImageUrl;
                ViewData["Layout"] = getPage.Data.Layout ?? "Masters/_Layout";
                ViewData["BodyClass"] = getPage.Data.CssClass;
                ViewData["ViewMode"] = MixMvcViewMode.Page;
                ViewData["Keyword"] = keyword;

                ViewBag.viewMode = MixMvcViewMode.Page;
                getPage.LastUpdateConfiguration = MixService.GetAppSetting<DateTime?>("LastUpdateConfiguration");
                return View(getPage.Data);
            }
            else
            {
                return Redirect($"/{culture}/404");
            }
        }

        protected async System.Threading.Tasks.Task<IActionResult> Post(int id)
        {
            Expression<Func<MixPost, bool>> predicate;
            predicate = p =>
            p.Id == id
            && p.Status == MixContentStatus.Published
            && p.Specificulture == culture;

            RepositoryResponse<Lib.ViewModels.MixPosts.ReadMvcViewModel> getPost =
                await Lib.ViewModels.MixPosts.ReadMvcViewModel.Repository.GetFirstModelAsync(predicate);

            if (getPost.IsSucceed)
            {
                ViewData["Title"] = getPost.Data.SeoTitle;
                ViewData["Description"] = getPost.Data.SeoDescription;
                ViewData["Keywords"] = getPost.Data.SeoKeywords;
                ViewData["Image"] = getPost.Data.ImageUrl;
                ViewData["BodyClass"] = getPost.Data.BodyClass;
                ViewData["ViewMode"] = MixMvcViewMode.Post;

                ViewBag.viewMode = MixMvcViewMode.Post;
                getPost.LastUpdateConfiguration = MixService.GetAppSetting<DateTime?>("LastUpdateConfiguration");
                return View(getPost.Data);
            }
            else
            {
                return Redirect($"/{culture}/404");
            }
        }

        protected async System.Threading.Tasks.Task<IActionResult> Data(string mixDatabaseName, string seoName)
        {
            var getData = await Lib.ViewModels.MixDatabaseDatas.Helper.FilterByKeywordAsync<Lib.ViewModels.MixDatabaseDatas.ReadMvcViewModel>(
                culture, mixDatabaseName, "equal", "seo_url", seoName);

            if (getData.IsSucceed && getData.Data.Count > 0)
            {
                getData.LastUpdateConfiguration = MixService.GetAppSetting<DateTime?>("LastUpdateConfiguration");
                return View(getData.Data.FirstOrDefault());
            }
            else
            {
                return Redirect($"/{culture}/404");
            }
        }

        protected async System.Threading.Tasks.Task<IActionResult> Module(int id)
        {
            // Home Page
            int? pageSize = MixService.GetAppSetting<int?>("TagPageSize");
            string orderBy = MixService.GetAppSetting<string>("OrderBy");
            int orderDirection = MixService.GetAppSetting<int>("OrderDirection");
            int.TryParse(Request.Query["page"], out int page);
            RepositoryResponse<Lib.ViewModels.MixModules.ReadMvcViewModel> getData = null;

            Expression<Func<MixModule, bool>> predicate;

            predicate = p =>
            p.Id == id
            && p.Status == MixContentStatus.Published && p.Specificulture == culture;

            getData = await Lib.ViewModels.MixModules.ReadMvcViewModel.Repository.GetFirstModelAsync(predicate);
            if (getData.IsSucceed)
            {
                getData.Data.LoadData(pageIndex: page - 1, pageSize: pageSize);
                getData.Data.DetailsUrl = GenerateDetailsUrl(
                    new { culture = culture, seoName = getData.Data.Name }
                    );
                GenerateDetailsUrls(getData.Data);
                //_ = MixCacheService.SetAsync(cacheKey, getPage);
            }

            if (getData.IsSucceed)
            {
                ViewData["Title"] = getData.Data.Title;
                ViewData["Description"] = getData.Data.Description;
                ViewData["Keywords"] = getData.Data.Title;
                ViewData["Image"] = getData.Data.ImageUrl;
                getData.LastUpdateConfiguration = MixService.GetAppSetting<DateTime?>("LastUpdateConfiguration");
                return View(getData.Data);
            }
            else
            {
                return Redirect($"/{culture}/404");
            }
        }

        protected void GeneratePageDetailsUrls(Lib.ViewModels.MixPages.ReadMvcViewModel page)
        {
            if (page.Modules != null)
            {
                foreach (var nav in page.Modules)
                {
                    GenerateDetailsUrls(nav.Module);
                }
            }
        }

        protected void GenerateDetailsUrls(Lib.ViewModels.MixModules.ReadMvcViewModel module)
        {
            module.DetailsUrl = GenerateDetailsUrl(
                            new { action = "module", culture = culture, id = module.Id, seoName = module.Name }
                            );
        }

        protected string GenerateDetailsUrl(object routeValues)
        {
            return MixCmsHelper.GetRouterUrl(routeValues, Request, Url);
        }

        public async System.Threading.Tasks.Task<IActionResult> Error(string page = "404")
        {
            return await Page(page);
        }

        #endregion Helper
    }
}