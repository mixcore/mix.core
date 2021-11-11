using Microsoft.AspNetCore.Mvc;
using Mix.Shared.Constants;
using Mix.Lib.Base;
using Mix.Shared.Services;
using Mix.Lib.Services;
using Mix.Database.Services;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace Mixcore.Controllers
{
    public class PortalController : MixControllerBase
    {
        private readonly MixDatabaseService _databaseService;
       
        public PortalController(
            GlobalConfigService globalConfigService,
            MixService mixService,
            MixDatabaseService databaseService, IPSecurityConfigService ipSecurityConfigService)
            : base(globalConfigService, mixService, ipSecurityConfigService)
        {
            _databaseService = databaseService;
        }

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
        public IActionResult Index()
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

        #region overrides

        protected override void ValidateRequest()
        {
            // If IP retricted in appsettings
            if (ForbiddenPortal)
            {
                isValid = false;
                _redirectUrl = $"/403";
            }

            base.ValidateRequest();

            // If this site has not been inited yet
            if (_globalConfigService.AppSettings.IsInit)
            {
                isValid = false;
                if (string.IsNullOrEmpty(_databaseService.GetConnectionString(MixConstants.CONST_CMS_CONNECTION)))
                {
                    _redirectUrl = "Init";
                }
                else
                {
                    var status = _globalConfigService.AppSettings.InitStatus;
                    _redirectUrl = $"/init/step{status}";
                }
            }
        }

        #endregion overrides
    }
}
