﻿using Microsoft.AspNetCore.Mvc;
using Mix.Cms.Lib;
using Mix.Cms.Lib.Services;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Constants;
namespace Mix.Cms.Web.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class SecurityController : BaseController
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
        [Route("security/{page}")]
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