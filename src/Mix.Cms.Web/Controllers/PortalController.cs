using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mix.Cms.Lib;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Services;

namespace Mix.Cms.Web.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class PortalController : BaseController
    {
        #region overrides

        protected override void ValidateRequest()
        {
            // If IP retricted in appsettings
            if (_forbiddenPortal)
            {
                isValid = false;
                _redirectUrl = $"/error/403";
            }

            base.ValidateRequest();

            // If this site has not been inited yet
            if (MixService.GetAppSetting<bool>(MixAppSettingKeywords.IsInit))
            {
                isValid = false;
                if (string.IsNullOrEmpty(MixService.GetConnectionString(MixConstants.CONST_CMS_CONNECTION)))
                {
                    _redirectUrl = $"Init";
                }
                else
                {
                    var status = MixService.GetAppSetting<string>("InitStatus");
                    _redirectUrl = $"/init/step{status}";
                }
            }
        }

        #endregion overrides

        #region Routes

        [HttpGet]
        [Route("portal")]
        [Route("portal/page/{type}")]
        [Route("portal/post/{type}")]
        [Route("portal/{pageName}")]
        [Route("portal/{pageName}/{type}")]
        [Route("portal/{pageName}/{type}/{param}")]
        [Route("portal/{pageName}/{type}/{param}/{param1}")]
        [Route("portal/{pageName}/{type}/{param}/{param1}/{param2}")]
        [Route("portal/{pageName}/{type}/{param}/{param1}/{param2}/{param3}")]
        [Route("portal/{pageName}/{type}/{param}/{param1}/{param2}/{param3}/{param4}")]
        public IActionResult Index(string page)
        {
            if (isValid)
            {
                return View();
            }
            else
            {
                return Redirect(_redirectUrl);
            }
        }
        
        [HttpGet]
        [Route("admin")]
        [Route("admin/page/{type}")]
        [Route("admin/post/{type}")]
        [Route("admin/{pageName}")]
        [Route("admin/{pageName}/{type}")]
        [Route("admin/{pageName}/{type}/{param}")]
        [Route("admin/{pageName}/{type}/{param}/{param1}")]
        [Route("admin/{pageName}/{type}/{param}/{param1}/{param2}")]
        [Route("admin/{pageName}/{type}/{param}/{param1}/{param2}/{param3}")]
        [Route("admin/{pageName}/{type}/{param}/{param1}/{param2}/{param3}/{param4}")]
        public IActionResult Svelte(string page)
        {
            if (isValid)
            {
                return View();
            }
            else
            {
                return Redirect(_redirectUrl);
            }
        }
        
        [HttpGet]
        [Route("portal-v2")]
        [Route("portal-v2/page/{type}")]
        [Route("portal-v2/post/{type}")]
        [Route("portal-v2/{pageName}")]
        [Route("portal-v2/{pageName}/{type}")]
        [Route("portal-v2/{pageName}/{type}/{param}")]
        [Route("portal-v2/{pageName}/{type}/{param}/{param1}")]
        [Route("portal-v2/{pageName}/{type}/{param}/{param1}/{param2}")]
        [Route("portal-v2/{pageName}/{type}/{param}/{param1}/{param2}/{param3}")]
        [Route("portal-v2/{pageName}/{type}/{param}/{param1}/{param2}/{param3}/{param4}")]
        public IActionResult PortalV2(string page)
        {
            if (isValid)
            {
                return View();
            }
            else
            {
                return Redirect(_redirectUrl);
            }
        }

        #endregion Routes
    }
}