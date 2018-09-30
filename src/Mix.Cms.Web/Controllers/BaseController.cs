using System.Globalization;
using AutoMapper.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mix.Cms.Lib;
using Mix.Cms.Lib.Services;

namespace Mix.Cms.Web.Controllers
{
    public class BaseController : Controller
    {
        protected string _domain;
        protected IConfiguration _configuration;
        protected IHostingEnvironment _env;
        public BaseController(IHostingEnvironment env)
        {
            _env = env;
            // Set CultureInfo
            var cultureInfo = new CultureInfo(_culture);
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
        }

        public BaseController(IHostingEnvironment env, IConfiguration configuration)
        {
            _configuration = configuration;
            _env = env;
            // Set CultureInfo
            var cultureInfo = new CultureInfo(_culture);
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
        }
      
        public ViewContext ViewContext { get; set; }

        protected string _culture
        {
            get => RouteData?.Values["culture"]?.ToString().ToLower() ?? MixService.GetConfig<string>("DefaultCulture");
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewBag.culture = _culture;
            _domain = string.Format("{0}://{1}", Request.Scheme, Request.Host);
            base.OnActionExecuting(context);
        }

        protected void GetLanguage()
        {
            
        }

    }
}