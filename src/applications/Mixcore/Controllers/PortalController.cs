using Microsoft.AspNetCore.Mvc;
using Mix.Database.Services;
using Mix.Shared.Services;

namespace Mixcore.Controllers
{
    public class PortalController : MixControllerBase
    {
        private readonly DatabaseService _databaseService;

        public PortalController(
            IHttpContextAccessor httpContextAccessor,
            IMixCmsService mixCmsService,
            DatabaseService databaseService, IPSecurityConfigService ipSecurityConfigService)
            : base(httpContextAccessor, mixCmsService, ipSecurityConfigService)
        {
            _databaseService = databaseService;
        }

        [HttpGet]
        [Route("admin/{appFolder?}/{param1?}/{param2?}/{param3?}/{param4?}")]
        public IActionResult Index()
        {
            if (IsValid)
            {
                return View();
            }
            else
            {
                return Redirect(RedirectUrl);
            }
        }

        [Route("portal/{appFolder?}/{param1?}/{param2?}/{param3?}/{param4?}")]
        //[Route("portal-apps/{appFolder?}/{param1?}/{param2?}/{param3?}/{param4?}")]
        public IActionResult Spa(string appFolder, string param1, string param2, string param3, string param4)
        {
            string folder = $"portal-apps/{appFolder}";
            var baseHref = Request.Query["baseHref"].ToString();
            if (string.IsNullOrEmpty(baseHref))
            {
                string subPath = string.Join(
                "/",
                Request.RouteValues
                .Where(m => m.Key.Contains("param"))
                .Select(m => m.Value).ToArray());
                appFolder ??= "mix-portal";

                string url = $"/portal-apps/{appFolder}/{subPath}?baseHref=portal-apps/{appFolder}";
                return Redirect(url);
            }

            return View();

        }
        #region overrides

        protected override void ValidateRequest()
        {
            // If IP retricted in appsettings
            if (ForbiddenPortal)
            {
                IsValid = false;
                RedirectUrl = "/403";
            }

            base.ValidateRequest();

            // If this site has not been inited yet
            if (GlobalConfigService.Instance.AppSettings.IsInit)
            {
                IsValid = false;
                if (string.IsNullOrEmpty(_databaseService.GetConnectionString(MixConstants.CONST_CMS_CONNECTION)))
                {
                    RedirectUrl = "Init";
                }
                else
                {
                    var status = GlobalConfigService.Instance.AppSettings.InitStatus;
                    RedirectUrl = $"/init/step{status}";
                }
            }
        }

        #endregion overrides
    }
}
