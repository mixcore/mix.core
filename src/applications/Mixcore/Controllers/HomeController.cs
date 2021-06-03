using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Mix.Heart.Models;
using Mix.Infrastructure.Repositories;
using Mix.Shared.Constants;
using Mix.Lib.Controllers;
using Mix.Shared.Enums;
using Mix.Lib.Models.Common;
using Mix.Lib.Services;
using Mixcore.Domain.ViewModels.Mvc;
using Mixcore.Models;
using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Mix.Shared.Services;
using Mix.Database.Entities.Cms.v2;

namespace Mixcore.Controllers
{
    public class HomeController : MixControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TranslatorService _translator;
        public HomeController(
            ILogger<HomeController> logger,
            MixService mixService,
            TranslatorService translator) : base(mixService)
        {
            _logger = logger;
            _translator = translator;
        }
        protected override void ValidateRequest()
        {
            base.ValidateRequest();

            // If this site has not been inited yet
            if (MixAppSettingService.GetConfig<bool>(MixAppSettingKeywords.IsInit))
            {
                isValid = false;
                if (string.IsNullOrEmpty(MixService.GetConnectionString(MixConstants.CONST_CMS_CONNECTION)))
                {
                    _redirectUrl = "Init";
                }
                else
                {
                    var status = MixAppSettingService.GetConfig<string>("InitStatus");
                    _redirectUrl = $"/init/step{status}";
                }
            }
        }

        [Route("")]
        [Route("{seoName}")]
        [Route("{seoName}/{keyword}")]
        [Route("{culture}/{seoName}/{keyword}")]
        public async Task<IActionResult> Index(string seoName, string keyword)
        {
            if (!isValid)
            {
                return Redirect(_redirectUrl);
            }


            seoName = seoName ?? Request.Query["alias"];
            if (!string.IsNullOrEmpty(seoName))
            {
                if (CheckIsVueRoute(seoName))
                {
                    var staticFile = MixFileRepository.Instance.GetFile(seoName, MixFolders.WebRootPath);
                    if (staticFile != null)
                    {
                        return Ok(staticFile.Content);
                    }
                    else
                    {
                        var getModule = await MvcModuleViewModel.Repository.GetSingleModelAsync(
            m => m.Name == seoName && m.Specificulture == Culture);
                        if (getModule.IsSucceed)
                        {
                            var myViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(),
                            new ModelStateDictionary()) { { "ModuleViewModel",
                    getModule.Data} };
                            myViewData.Model = getModule.Data;

                            PartialViewResult result = new PartialViewResult()
                            {
                                ViewName = "VueComponent",
                                ViewData = myViewData,
                            };

                            return result;
                        }
                        else
                        {
                            return NotFound();
                        }
                    }
                }
                else
                {
                    HandleSeoName(ref seoName, ref keyword);
                }
            }
            return await AliasAsync(seoName, keyword);
        }

        [HttpPost]
        [Route("search")]
        [Route("{culture}/search")]
        public async Task<IActionResult> Search([FromBody] string keyword)
        {
            return await Page("search", keyword);
        }

        #region Views
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

        #endregion
       
        public async System.Threading.Tasks.Task<IActionResult> Error(string page = "404")
        {
            return await Page(page);
        }

        private void HandleSeoName(ref string seoName, ref string keyword)
        {
            using var ctx = new MixCmsContext();
            string temp = $"{seoName}/{keyword}";
            if (ctx.MixUrlAlias.Any(u => u.Alias == temp))
            {
                seoName = temp;
                keyword = string.Empty;
            }
            else
            {
                // Check url is end with '/' or '?'
                // Ex: en-us/page-name/ => seoName = en-us/page-name
                string regex = @"(.*)[(\/|\?|#)]$";
                System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex(regex, RegexOptions.IgnoreCase);
                Match m = r.Match(seoName);
                if (m.Success)
                {
                    seoName = m.Groups[1].Value;
                }

                // Check first group is culture
                // Ex: en-us/page-name => culture = en-us , seoName = page-name
                regex = @"^([A-Za-z]{1,8}|[A-Za-z]{1,8}(-[A-Za-z0-9]{1,8})|[A-Za-z]{1,8}(-[A-Za-z0-9]{1,8})(-[A-Za-z0-9]{1,8}))\/(.*)$";
                r = new System.Text.RegularExpressions.Regex(regex, RegexOptions.IgnoreCase);
                m = r.Match(seoName);
                if (m.Success)
                {
                    if (MixAppSettingService.Instance.CheckValidCulture(m.Groups[1].Value))
                    {
                        Culture = m.Groups[1].Value;
                        seoName = m.Groups[5].Value;
                    }
                }

                if (MixAppSettingService.Instance.CheckValidCulture(seoName))
                {
                    Culture = seoName;
                    seoName = keyword;
                    keyword = string.Empty;
                }
            }
        }

        protected bool CheckIsVueRoute(string seoName)
        {
            // Check if seoname is vue route
            var regex = @"^(.*)\.((vue)$)";
            var r = new System.Text.RegularExpressions.Regex(regex, RegexOptions.IgnoreCase);
            var m = r.Match(seoName);
            return m.Success;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
