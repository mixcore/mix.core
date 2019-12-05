using AutoMapper.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mix.Cms.Lib;
using Mix.Cms.Lib.Services;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Linq;

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
        public BaseController()
        {
            // Set CultureInfo
            var cultureInfo = new CultureInfo(_culture);
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
        }

        public ViewContext ViewContext { get; set; }

        protected string _culture
        {
            get => RouteData?.Values["culture"]?.ToString().ToLower() == null
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

            if (_forbidden)
            {
                Redirect($"/error/403");
            }
            if (MixService.GetConfig<bool>("IsMaintenance"))
            {
                Redirect($"/{_culture}/maintenance");
            }

            base.OnActionExecuting(context);
        }

    }
}