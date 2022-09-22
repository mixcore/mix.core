using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Mix.Lib.Extensions;
using Mix.Lib.Services;
using System.Globalization;

namespace Mix.Lib.Base
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public abstract class MixControllerBase : Controller
    {
        protected ISession _session;
        protected MixTenantSystemViewModel _currentTenant;
        protected MixTenantSystemViewModel CurrentTenant
        {
            get
            {
                if (_currentTenant == null)
                {
                    _currentTenant = _session.Get<MixTenantSystemViewModel>(MixRequestQueryKeywords.Tenant);
                }
                return _currentTenant;
            }
        }
        protected string domain;
        protected bool forbidden = false;
        protected bool isValid = true;
        protected string _redirectUrl;
        protected readonly IPSecurityConfigService _ipSecurityConfigService;
        protected readonly MixService _mixService;
        protected bool ForbiddenPortal
        {
            get
            {
                var allowedIps = _ipSecurityConfigService.GetConfig<JArray>(MixAppSettingKeywords.AllowedPortalIps) ?? new JArray();
                string remoteIp = Request.HttpContext?.Connection?.RemoteIpAddress?.ToString();
                return forbidden || (
                        // add in allowedIps "::1" to allow localhost
                        allowedIps.Count > 0 &&
                        !allowedIps.Any(t => t["text"].Value<string>() == remoteIp)
                );
            }
        }

        protected IConfiguration _configuration;

        public MixControllerBase(
             IHttpContextAccessor httpContextAccessor,
            MixService mixService,
            IPSecurityConfigService ipSecurityConfigService)
        {
            _mixService = mixService;
            _session = httpContextAccessor.HttpContext.Session;
            _ipSecurityConfigService = ipSecurityConfigService;
            ViewData[MixRequestQueryKeywords.Tenant] = CurrentTenant;
        }

        private void LoadCulture()
        {
            if (RouteData?.Values["culture"]?.ToString().ToLower() is not null)
            {
                Culture = RouteData?.Values["culture"]?.ToString().ToLower();
            }
            //if (!_globalConfigService.Instance.CheckValidCulture(Culture))
            //{
            //    Culture = GlobalConfigService.Instance.AppSettings.DefaultCulture;
            //}
            Culture ??= CurrentTenant.Configurations.DefaultCulture;

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
            if (!GlobalConfigService.Instance.AppSettings.IsInit)
            {
                LoadCulture();
            }
            ViewBag.culture = Culture;
            if (!string.IsNullOrEmpty(Culture))
            {
                ViewBag.assetFolder = _mixService.GetAssetFolder(Culture, CurrentTenant.PrimaryDomain);
            }
            domain = string.Format("{0}://{1}", Request.Scheme, Request.Host);
            if (_ipSecurityConfigService.GetConfig<bool>(MixAppSettingKeywords.IsRetrictIp))
            {
                var allowedIps = _ipSecurityConfigService.GetConfig<JArray>(MixAppSettingKeywords.AllowedIps) ?? new JArray();
                var exceptIps = _ipSecurityConfigService.GetConfig<JArray>(MixAppSettingKeywords.ExceptIps) ?? new JArray();
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
                _redirectUrl = "/403";
            }

            // If mode Maintenance enabled in appsettings
            if (!GlobalConfigService.Instance.IsInit && CurrentTenant.Configurations.IsMaintenance
                    && Request.RouteValues["seoName"].ToString() != "maintenance")
            {
                isValid = false;
                _redirectUrl = "/maintenance";
            }
        }


    }
}
