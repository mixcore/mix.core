using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Caching.Memory;
using Mix.Cms.Lib;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Domain.Core.ViewModels;
using Mix.Identity.Models;
using System;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Mix.Cms.Lib.MixEnums;


namespace Mix.Cms.Web.Controllers
{
    public class HomeController : BaseController
    {        
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IApiDescriptionGroupCollectionProvider _apiExplorer;
        IApplicationLifetime _lifetime;

        #region contructor
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

        protected override void ValidateRequest()
        {
            base.ValidateRequest();

            // If this site has not been inited yet
            if (MixService.GetConfig<bool>("IsInit"))
            {
                isValid = false;
                if (string.IsNullOrEmpty(MixService.GetConnectionString(MixConstants.CONST_CMS_CONNECTION)))
                {
                    _redirectUrl = $"Init";
                }
                else
                {
                    var status = MixService.GetConfig<string>("InitStatus");
                    _redirectUrl = $"/init/step{status}";
                }
            }
        }
        #endregion

        #region Routes

        [Route("")]
        public async Task<IActionResult> Index()
        {
            if (isValid)
            {
                string seoName = Request.Query["alias"];
                HandleSeoName(ref seoName);
                ViewData["Layout"] = "Masters/_Layout";
                return await AliasAsync(seoName);
            }
            else
            {
                return Redirect(_redirectUrl);
            }            
        }

        private void HandleSeoName(ref string seoName)
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
            // Ex: en-us/page-name => culture = en-us , seoNam = page-name
            regex = @"^([A-Za-z]{1,8}|[A-Za-z]{1,8}(-[A-Za-z0-9]{1,8})|[A-Za-z]{1,8}(-[A-Za-z0-9]{1,8})(-[A-Za-z0-9]{1,8}))\/(.*)$";
            r = new System.Text.RegularExpressions.Regex(regex, RegexOptions.IgnoreCase);
            m = r.Match(seoName);
            if (m.Success)
            {
                if (MixService.Instance.CheckValidCulture(m.Groups[1].Value))
                {
                    culture = m.Groups[1].Value;
                    seoName = m.Groups[5].Value;
                }
            }

            if (MixService.Instance.CheckValidCulture(seoName))
            {
                culture = seoName;
                seoName = string.Empty;
            }
        }

        #endregion
    }
}