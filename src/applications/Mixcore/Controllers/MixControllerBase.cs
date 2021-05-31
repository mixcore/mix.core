using AutoMapper.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mix.Heart.Models;
using Mix.Lib.Constants;
using Mix.Lib.Entities.Cms;
using Mix.Lib.Enums;
using Mix.Lib.Helpers;
using Mix.Lib.Models.Common;
using Mix.Lib.Services;
using Mix.Lib.ViewModels.Cms;
using Mixcore.Domain.ViewModels.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mixcore.Controllers
{
    public class MixControllerBase : Controller
    {
        protected string domain;
        protected bool forbidden = false;
        protected bool isValid = true;
        protected string _redirectUrl;
        protected readonly MixService _mixService;
        protected bool ForbiddenPortal
        {
            get
            {
                var allowedIps = MixAppSettingService.GetConfig<JArray>(MixAppSettingsSection.IpSecuritySettings, "AllowedPortalIps") ?? new JArray();
                string remoteIp = Request.HttpContext?.Connection?.RemoteIpAddress?.ToString();
                return forbidden || (
                        // add in allowedIps "::1" to allow localhost
                        allowedIps.Count > 0 &&
                        !allowedIps.Any(t => t["text"].Value<string>() == remoteIp)
                );
            }
        }

        protected IConfiguration _configuration;

        public MixControllerBase(MixService mixService)
        {
            if (!MixAppSettingService.GetConfig<bool>(MixAppSettingKeywords.IsInit))
            {
                LoadCulture();
            }
            _mixService = mixService;
        }

        private void LoadCulture()
        {
            if (RouteData?.Values["culture"]?.ToString().ToLower() is not null)
            {
                Culture = RouteData?.Values["culture"]?.ToString().ToLower();
            }
            if (!MixAppSettingService.Instance.CheckValidCulture(Culture))
            {
                Culture = MixAppSettingService.GetConfig<string>(MixAppSettingKeywords.DefaultCulture);
            }

            // Set CultureInfo
            var cultureInfo = new CultureInfo(Culture);
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
        }

        public ViewContext ViewContext { get; set; }
        private string _culture;

        public string Culture
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

            ViewBag.culture = Culture;
            if (!string.IsNullOrEmpty(Culture))
            {
                ViewBag.assetFolder = MixCmsHelper.GetAssetFolder(Culture);
            }
            domain = string.Format("{0}://{1}", Request.Scheme, Request.Host);
            if (MixAppSettingService.GetConfig<bool>(MixAppSettingsSection.IpSecuritySettings, "IsRetrictIp"))
            {
                var allowedIps = MixAppSettingService.GetConfig<JArray>(MixAppSettingsSection.IpSecuritySettings, "AllowedIps") ?? new JArray();
                var exceptIps = MixAppSettingService.GetConfig<JArray>(MixAppSettingsSection.IpSecuritySettings, "ExceptIps") ?? new JArray();
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
            // If IP retricted in appsettings
            if (forbidden)
            {
                isValid = false;
                _redirectUrl = $"/403";
            }

            // If mode Maintenance enabled in appsettings
            if (MixAppSettingService.GetConfig<bool>("IsMaintenance") && Request.RouteValues["seoName"].ToString() != "maintenance")
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
                RepositoryResponse<MixUrlAliasViewModel> getAlias = null;

                Expression<Func<MixUrlAlias, bool>> predicate;

                predicate = p =>
                p.Alias == seoName
                && p.Status == MixContentStatus.Published && p.Specificulture == Culture;

                getAlias = await MixUrlAliasViewModel.Repository.GetFirstModelAsync(predicate);
                if (getAlias.IsSucceed)// && getPage.Data.View != null
                {
                    return getAlias.Data.Type switch
                    {
                        MixUrlAliasType.Page => await Page(int.Parse(getAlias.Data.SourceId), keyword),
                        MixUrlAliasType.Post => await Post(int.Parse(getAlias.Data.SourceId)),
                        // TODO: Create view for module
                        _ => await Page(0),
                    };
                }
                else
                {
                    return await Page(seoName, keyword);
                }
            }
        }

        protected async Task<IActionResult> Page(string seoName, string keyword = null)
        {
            // Home Page
            int maxPageSize = MixAppSettingService.GetConfig<int>("MaxPageSize");
            var searchRequest = new SearchQueryModel(Request);
            ViewData["keyword"] = keyword;
            RepositoryResponse<MvcPageViewModel> getPage = null;
            Expression<Func<MixPage, bool>> predicate;

            if (string.IsNullOrEmpty(seoName))
            {
                predicate = p =>
                p.Type == MixPageType.Home
                && p.Status == MixContentStatus.Published && p.Specificulture == Culture;
            }
            else
            {
                predicate = p =>
                p.SeoName == seoName
                && p.Status == MixContentStatus.Published && p.Specificulture == Culture;
            }

            getPage = await MvcPageViewModel.Repository.GetFirstModelAsync(predicate);

            if (getPage.IsSucceed)
            {
                if (getPage.Data != null)
                {
                    maxPageSize = getPage.Data.PageSize ?? maxPageSize;
                    await getPage.Data.LoadData(searchRequest.PagingData);
                }
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
                getPage.LastUpdateConfiguration = MixAppSettingService.GetConfig<DateTime?>("LastUpdateConfiguration");
                return View(getPage.Data);
            }
            else
            {
                if (seoName != "404")
                {
                    return Redirect($"/{Culture}/404");
                }
                else
                {
                    return NotFound();
                }
            }
        }

        protected async Task<IActionResult> Page(int pageId, string keyword = null)
        {
            // Home Page
            var searchRequest = new SearchQueryModel(Request);
            RepositoryResponse<MvcPageViewModel> getPage = null;

            Expression<Func<MixPage, bool>> predicate;

            predicate = p =>
            p.Id == pageId
            && p.Status == MixContentStatus.Published && p.Specificulture == Culture;

            getPage = await MvcPageViewModel.Repository.GetFirstModelAsync(predicate);
            if (getPage.IsSucceed)
            {
                await getPage.Data.LoadData(searchRequest.PagingData);
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
                getPage.LastUpdateConfiguration = MixAppSettingService.GetConfig<DateTime?>("LastUpdateConfiguration");
                return View(getPage.Data);
            }
            else
            {
                return Redirect($"/{Culture}/404");
            }
        }

        protected async Task<IActionResult> Post(int id)
        {
            Expression<Func<MixPost, bool>> predicate;
            predicate = p =>
            p.Id == id
            && p.Status == MixContentStatus.Published
            && p.Specificulture == Culture;

            RepositoryResponse<MvcPostViewModel> getPost =
                await MvcPostViewModel.Repository.GetFirstModelAsync(predicate);

            if (getPost.IsSucceed)
            {
                ViewData["Title"] = getPost.Data.SeoTitle;
                ViewData["Description"] = getPost.Data.SeoDescription;
                ViewData["Keywords"] = getPost.Data.SeoKeywords;
                ViewData["Image"] = getPost.Data.ImageUrl;
                ViewData["BodyClass"] = getPost.Data.BodyClass;
                ViewData["ViewMode"] = MixMvcViewMode.Post;

                ViewBag.viewMode = MixMvcViewMode.Post;
                getPost.LastUpdateConfiguration = MixAppSettingService.GetConfig<DateTime?>("LastUpdateConfiguration");
                return View(getPost.Data);
            }
            else
            {
                return Redirect($"/{Culture}/404");
            }
        }

        protected async Task<IActionResult> Module(int id)
        {
            // Home Page
            var searchRequest = new SearchQueryModel(Request);
            RepositoryResponse<MvcModuleViewModel> getData = null;

            Expression<Func<MixModule, bool>> predicate;

            predicate = p =>
            p.Id == id
            && p.Status == MixContentStatus.Published && p.Specificulture == Culture;

            getData = await MvcModuleViewModel.Repository.GetFirstModelAsync(predicate);
            if (getData.IsSucceed)
            {
                await getData.Data.LoadData(searchRequest.PagingData);
            }

            if (getData.IsSucceed)
            {
                ViewData["Title"] = getData.Data.Title;
                ViewData["Description"] = getData.Data.Description;
                ViewData["Keywords"] = getData.Data.Title;
                ViewData["Image"] = getData.Data.ImageUrl;
                getData.LastUpdateConfiguration = MixAppSettingService.GetConfig<DateTime?>("LastUpdateConfiguration");
                return View(getData.Data);
            }
            else
            {
                return Redirect($"/{Culture}/404");
            }
        }
        public async System.Threading.Tasks.Task<IActionResult> Error(string page = "404")
        {
            return await Page(page);
        }

        #endregion Helper
    }
}
