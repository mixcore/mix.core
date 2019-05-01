using System.Globalization;
using System.Linq;
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
using Newtonsoft.Json.Linq;

namespace Mix.Cms.Web.Controllers
{
    public class BaseController : Controller
    {
        protected string _domain;
        protected bool _forbidden = false;
        protected bool _forbiddenPortal
        {
            get
            {
                var allowedIps = MixService.GetIpConfig<JArray>("AllowedPortalIps") ?? new JArray();
                string remoteIp = Request.HttpContext?.Connection?.RemoteIpAddress?.ToString();
                return _forbidden || (
                    // allow localhost
                    //remoteIp != "::1" &&
                    (
                        allowedIps.Count > 0 &&
                        !allowedIps.Any(t => t["text"].Value<string>() == remoteIp)
                    )
                );
            }
        }
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
            get => RouteData?.Values["culture"]?.ToString().ToLower() ==null
                    || RouteData?.Values["culture"]?.ToString().ToLower() == "init" 
                ? MixService.GetConfig<string>("DefaultCulture")
                : RouteData?.Values["culture"].ToString();
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewBag.culture = _culture;
            if (!string.IsNullOrEmpty(_culture))
            {
                ViewBag.assetFolder = MixCmsHelper.GetAssetFolder(_culture);
            }
            _domain = string.Format("{0}://{1}", Request.Scheme, Request.Host);
            if (MixService.GetIpConfig<bool>("IsRetrictIp"))
            {
                var allowedIps = MixService.GetIpConfig<JArray>("AllowedIps") ?? new JArray();
                var exceptIps = MixService.GetIpConfig<JArray>("ExceptIps") ?? new JArray();
                string remoteIp = Request.HttpContext?.Connection?.RemoteIpAddress?.ToString();
                if (
                        // allow localhost
                        //remoteIp != "::1" &&                    
                        allowedIps.Count > 0 &&
                        !allowedIps.Any(t => t["text"].Value<string>() == remoteIp) ||
                        (
                            exceptIps.Count > 0 &&
                            exceptIps.Any(t => t["text"].Value<string>() == remoteIp)
                        )
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