using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mix.Shared.Constants;
using Mix.Shared.Enums;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Linq;
using Mix.Shared.Services;
using Mix.Lib.Services;
using Microsoft.Extensions.Configuration;

namespace Mix.Lib.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class MixControllerBase : Controller
    {
        protected string domain;
        protected bool forbidden = false;
        protected bool isValid = true;
        protected string _redirectUrl;
        protected readonly MixAppSettingService _appSettingService;
        protected readonly MixService _mixService;
        protected bool ForbiddenPortal
        {
            get
            {
                var allowedIps = _appSettingService.GetConfig<JArray>(MixAppSettingsSection.IpSecuritySettings, "AllowedPortalIps") ?? new JArray();
                string remoteIp = Request.HttpContext?.Connection?.RemoteIpAddress?.ToString();
                return forbidden || (
                        // add in allowedIps "::1" to allow localhost
                        allowedIps.Count > 0 &&
                        !allowedIps.Any(t => t["text"].Value<string>() == remoteIp)
                );
            }
        }

        protected IConfiguration _configuration;

        public MixControllerBase([FromServices] MixAppSettingService appSettingService, [FromServices] MixService mixService)
        {
            _appSettingService = appSettingService;
            _mixService = mixService;

            if (!_appSettingService.GetConfig<bool>(
                            MixAppSettingsSection.GlobalSettings, MixAppSettingKeywords.IsInit))
            {
                LoadCulture();
            }
        }

        private void LoadCulture()
        {
            if (RouteData?.Values["culture"]?.ToString().ToLower() is not null)
            {
                Culture = RouteData?.Values["culture"]?.ToString().ToLower();
            }
            //if (!_appSettingService.Instance.CheckValidCulture(Culture))
            //{
            //    Culture = _appSettingService.GetConfig<string>(MixAppSettingKeywords.DefaultCulture);
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
                ViewBag.assetFolder = _mixService.GetAssetFolder(Culture);
            }
            domain = string.Format("{0}://{1}", Request.Scheme, Request.Host);
            if (_appSettingService.GetConfig<bool>(MixAppSettingsSection.IpSecuritySettings, "IsRetrictIp"))
            {
                var allowedIps = _appSettingService.GetConfig<JArray>(MixAppSettingsSection.IpSecuritySettings, "AllowedIps") ?? new JArray();
                var exceptIps = _appSettingService.GetConfig<JArray>(MixAppSettingsSection.IpSecuritySettings, "ExceptIps") ?? new JArray();
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
            if (_appSettingService.GetConfig<bool>(MixAppSettingsSection.GlobalSettings, "IsMaintenance")
                    && Request.RouteValues["seoName"].ToString() != "maintenance")
            {
                isValid = false;
                _redirectUrl = $"/maintenance";
            }
        }


    }
}
