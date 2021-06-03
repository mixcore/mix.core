using AutoMapper.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mix.Shared.Constants;
using Mix.Shared.Enums;
using Mix.Lib.Helpers;
using Mix.Lib.Services;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Linq;
using Mix.Shared.Services;

namespace Mix.Lib.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
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
            //if (!MixAppSettingService.Instance.CheckValidCulture(Culture))
            //{
            //    Culture = MixAppSettingService.GetConfig<string>(MixAppSettingKeywords.DefaultCulture);
            //}

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

       
    }
}
