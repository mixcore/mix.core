using System;
using System.Diagnostics;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        public HomeController(IHostingEnvironment env,
            IMemoryCache memoryCache,
             UserManager<ApplicationUser> userManager
            ) : base(env, memoryCache)
        {
            this._userManager = userManager;
        }

        [Route("")]
        [Route("{culture}")]
        [Route("{culture}/{seoName}")]
        public async System.Threading.Tasks.Task<IActionResult> Index(string culture, string seoName)
        {
            if (MixService.GetConfig<bool>("IsInit"))
            {
                //Go to landing page
                return await PageAsync(seoName);


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

        [HttpGet]
        [Route("portal")]
        [Route("admin")]
        [Route("portal/{pageName}")]
        [Route("portal/{pageName}/{type}")]
        [Route("portal/{pageName}/{type}/{param}")]
        [Route("portal/{pageName}/{type}/{param}/{param1}")]
        [Route("portal/{pageName}/{type}/{param}/{param1}/{param2}")]
        [Route("portal/{pageName}/{type}/{param}/{param1}/{param2}/{param3}")]
        [Route("portal/{pageName}/{type}/{param}/{param1}/{param2}/{param3}/{param4}")]
        public IActionResult Portal()
        {
            return View();
        }

        [HttpGet]
        [Route("init")]
        [Route("init/{page}")]
        public IActionResult Init(string page)
        {
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
        [Route("404")]
        public async System.Threading.Tasks.Task<IActionResult> PageNotFound()
        {
            return await PageAsync("404");
        }


        async System.Threading.Tasks.Task<IActionResult> PageAsync(string seoName)//Expression<Func<MixPage, bool>> predicate, int? pageIndex = null, int pageSize = 10)
        {
            // Home Page

            var getPage = new RepositoryResponse<Lib.ViewModels.MixPages.ReadMvcViewModel>();

            var cacheKey = $"Page_{_culture}_{seoName}";
            var data = _memoryCache.Get<Lib.ViewModels.MixPages.ReadMvcViewModel>(cacheKey);
            if (data != null)
            {
                getPage.IsSucceed = true;
                getPage.Data = data;
            }
            else
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
                _memoryCache.Set(cacheKey, getPage.Data);
            }

            if (getPage.IsSucceed && getPage.Data.View != null)
            {
                GeneratePageDetailsUrls(getPage.Data);
                ViewData["Title"] = getPage.Data.SeoTitle;
                ViewData["Description"] = getPage.Data.SeoDescription;
                ViewData["Keywords"] = getPage.Data.SeoKeywords;
                ViewData["Image"] = getPage.Data.ImageUrl;
                ViewData["PageClass"] = getPage.Data.CssClass;
                return View(getPage.Data);
            }
            else
            {
                return NotFound();
            }
        }

        IActionResult ArticleView(Expression<Func<MixArticle, bool>> predicate)
        {
            var getArticle = Lib.ViewModels.MixArticles.ReadMvcViewModel.Repository.GetSingleModel(predicate);
            if (getArticle.IsSucceed)
            {
                ViewData["Title"] = getArticle.Data.SeoTitle;
                ViewData["Description"] = getArticle.Data.SeoDescription;
                ViewData["Keywords"] = getArticle.Data.SeoKeywords;
                ViewData["Image"] = getArticle.Data.ImageUrl;
                return View(getArticle.Data);
            }
            else
            {
                return RedirectToAction("PageNotFound", "Home");
            }
        }

        IActionResult ProductView(Expression<Func<MixProduct, bool>> predicate)
        {
            var getProduct = Lib.ViewModels.MixProducts.ReadMvcViewModel.Repository.GetSingleModel(predicate);
            if (getProduct.IsSucceed)
            {
                getProduct.Data.ProductNavs.ForEach(p =>
                {
                    p.RelatedProduct.DetailsUrl = GenerateDetailsUrl("Product", new { seoName = p.RelatedProduct.SeoName });
                });

                ViewData["Title"] = getProduct.Data.SeoTitle;
                ViewData["Description"] = getProduct.Data.SeoDescription;
                ViewData["Keywords"] = getProduct.Data.SeoKeywords;
                ViewData["Image"] = getProduct.Data.ImageUrl;

                return View(getProduct.Data);
            }
            else
            {
                return RedirectToAction("PageNotFound", "Home");
            }
        }

        void GeneratePageDetailsUrls(Lib.ViewModels.MixPages.ReadMvcViewModel page)
        {
            foreach (var articleNav in page.Articles.Items)
            {
                if (articleNav.Article != null)
                {
                    articleNav.Article.DetailsUrl = GenerateDetailsUrl("Article", new { seoName = articleNav.Article.SeoName });
                }
            }

            foreach (var productNav in page.Products.Items)
            {
                if (productNav.Product != null)
                {
                    productNav.Product.DetailsUrl = GenerateDetailsUrl("Product", new { seoName = productNav.Product.SeoName });
                }
            }
        }

        void GeneratePageDetailsUrls(Lib.ViewModels.MixModules.ReadMvcViewModel module)
        {
            foreach (var articleNav in module.Articles.Items)
            {
                if (articleNav.Article != null)
                {
                    articleNav.Article.DetailsUrl = GenerateDetailsUrl("Article", new { seoName = articleNav.Article.SeoName });
                }
            }

            foreach (var productNav in module.Products.Items)
            {
                if (productNav.Product != null)
                {
                    productNav.Product.DetailsUrl = GenerateDetailsUrl("Product", new { seoName = productNav.Product.SeoName });
                }
            }
        }

        string GenerateDetailsUrl(string type, object routeValues)
        {
            return MixCmsHelper.GetRouterUrl(type, routeValues, Request, Url);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
