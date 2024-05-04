using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Mix.Lib.Interfaces;
using Mix.Shared.Models.Configurations;
using System.Globalization;

namespace Mix.Lib.Base
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public abstract class MixControllerBase : Controller
    {
        protected ISession Session;
        protected string Domain;
        protected bool Forbidden;
        protected bool IsValid = true;
        protected string RedirectUrl;
        protected readonly IPSecurityConfigService IpSecurityConfigService;
        protected readonly GlobalSettingsModel GlobalConfig;
        protected readonly IMixCmsService MixCmsService;
        protected IMixTenantService TenantService;
        protected MixTenantSystemModel CurrentTenant => Session.Get<MixTenantSystemModel>(MixRequestQueryKeywords.Tenant);
        protected bool ForbiddenPortal
        {
            get
            {
                var allowedIps = IpSecurityConfigService.GetConfig<JArray>(MixAppSettingKeywords.AllowedPortalIps) ?? new JArray();
                string remoteIp = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
                return Forbidden || (
                        // add in allowedIps "::1" to allow localhost
                        allowedIps.Count > 0 &&
                        !allowedIps.Any(t => t["text"].Value<string>() == remoteIp)
                );
            }
        }

        protected IConfiguration Configuration;

        protected MixControllerBase(
             IHttpContextAccessor httpContextAccessor,
             IMixCmsService mixCmsService,
             IPSecurityConfigService ipSecurityConfigService,
             IMixTenantService tenantService,
             IConfiguration configuration)
        {
            Configuration = configuration;
            Session = httpContextAccessor.HttpContext?.Session;
            IpSecurityConfigService = ipSecurityConfigService;
            ViewData[MixRequestQueryKeywords.Tenant] = CurrentTenant;
            MixCmsService = mixCmsService;
            TenantService = tenantService;
            GlobalConfig = configuration.GetSection(MixAppSettingsSection.GlobalSettings).Get<GlobalSettingsModel>()!;
        }

        private void LoadCulture()
        {
            if (RouteData.Values["culture"]?.ToString()?.ToLower() is not null)
            {
                Culture = RouteData.Values["culture"]?.ToString()?.ToLower();
            }
            if (RouteData.Values["seoName"] is not null)
            {
                string seoName = RouteData.Values["seoName"]?.ToString()?.ToLower();
                string culture = seoName.Split('/')[0];
                if (TenantService.AllCultures.Any(m => m.Specificulture == culture))
                {
                    RouteData.Values["culture"] = culture;
                    RouteData.Values["seoName"] = seoName.Replace(culture, string.Empty);
                    Culture = culture;
                }
            }
            //if (!_globalConfigService.Instance.CheckValidCulture(Culture))
            //{
            //    Culture = GlobalConfigService.Instance.AppSettings.DefaultCulture;
            //}
            if (CurrentTenant != null)
            {
                if (!CurrentTenant.Cultures.Any(m => m.Specificulture == Culture))
                {
                    Culture = CurrentTenant.Cultures.First().Specificulture;
                }

                // Set CultureInfo
                var cultureInfo = new CultureInfo(Culture);
                CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
                CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
            }
        }

        public ViewContext ViewContext { get; set; }

        public string Culture { get; set; }

        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!GlobalConfig.IsInit)
            {
                LoadCulture();
            }
            return base.OnActionExecutionAsync(context, next);
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ValidateRequest();

            if (!string.IsNullOrEmpty(Culture))
            {
                ViewData["Culture"] = Culture;
                ViewData["AssetFolder"] = MixCmsService.GetAssetFolder(Culture, CurrentTenant.PrimaryDomain);
            }
            Domain = $"{Request.Scheme}://{Request.Host}";
            if (IpSecurityConfigService.GetConfig<bool>(MixAppSettingKeywords.IsRetrictIp))
            {
                var allowedIps = IpSecurityConfigService.GetConfig<JArray>(MixAppSettingKeywords.AllowedIps) ?? new JArray();
                var exceptIps = IpSecurityConfigService.GetConfig<JArray>(MixAppSettingKeywords.ExceptIps) ?? new JArray();
                string remoteIp = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
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
                    Forbidden = true;
                }
            }
        }

        protected virtual void ValidateRequest()
        {
            // If IP retricted in appsettings
            if (Forbidden)
            {
                IsValid = false;
                RedirectUrl = "/403";
            }

            // If mode Maintenance enabled in appsettings
            if (!GlobalConfig.IsInit
                && CurrentTenant != null
                && CurrentTenant.Configurations != null
                && CurrentTenant.Configurations.IsMaintenance
                && Request.RouteValues["seoName"]?.ToString() != "maintenance")
            {
                IsValid = false;
                RedirectUrl = "/maintenance";
            }
        }
    }
}
