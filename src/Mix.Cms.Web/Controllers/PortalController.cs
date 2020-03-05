using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Mix.Cms.Lib;
using Mix.Cms.Lib.Services;
using Mix.Identity.Models;

namespace Mix.Cms.Web.Controllers
{
    public class PortalController : BaseController
    {

        #region overrides

        protected override void ValidateRequest()
        {
            base.ValidateRequest();

            // If this site has not been inited yet
            if (MixService.GetConfig<bool>("IsInit"))
            {
                isValid = false;
                if (string.IsNullOrEmpty(MixService.GetConnectionString(MixConstants.CONST_CMS_CONNECTION)))
                {
                    _redirectUrl = $"Init";
                }
                else
                {
                    var status = MixService.GetConfig<string>("InitStatus");
                    _redirectUrl = $"/init/step{status}";
                }
            }
        }

        #endregion overrides

        #region Routes

        [HttpGet]
        [Authorize]
        [Route("portal")]
        [Route("admin")]
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

        #endregion Routes
    }
}