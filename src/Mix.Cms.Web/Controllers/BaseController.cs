using System.Globalization;
using AutoMapper.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Mix.Cms.Lib;
using Mix.Cms.Lib.Services;

namespace Mix.Cms.Web.Controllers
{
    public class BaseController : Controller
    {
        protected string _domain;
        protected bool _forbidden = false;
        protected IConfiguration _configuration;
        protected IHostingEnvironment _env;
        protected readonly IMemoryCache _memoryCache;
        protected readonly IHttpContextAccessor _accessor;
        public BaseController(IHostingEnvironment env, IMemoryCache memoryCache, IHttpContextAccessor accessor)
        {
            _env = env;
            _memoryCache = memoryCache;
            _accessor = accessor;
            // Set CultureInfo
            var cultureInfo = new CultureInfo(_culture);
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
        }

        public BaseController(IHostingEnvironment env, IConfiguration configuration, IHttpContextAccessor accessor)
        {
            _configuration = configuration;
            _accessor = accessor;
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
            if (MixService.GetConfig<bool>("IsRetrictIp"))
            {
                string allowedIps = MixService.GetConfig<string>("AllowedIps");
                string exceptIps = MixService.GetConfig<string>("ExceptIps");
                string remoteIp = _accessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
                if (
                    // allow localhost
                    remoteIp != "::1" &&
                    (allowedIps != "*" && !allowedIps.Contains(remoteIp))
                    || (exceptIps.Contains(remoteIp))
                    )
                {
                    _forbidden = true;
                }
            }
            base.OnActionExecuting(context);
        }

        protected void GetLanguage()
        {
            
        }

    }
}