﻿using Microsoft.AspNetCore.Mvc;
using Mix.Shared.Constants;
using Mix.Lib.Controllers;
using Mix.Shared.Services;
using Mix.Shared.Enums;
using Mix.Lib.Services;
using Mix.Database.Services;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace Mixcore.Controllers
{
    public class PortalController : MixControllerBase
    {
        private readonly MixDatabaseService _databaseService;
        protected bool _forbiddenPortal
        {
            get
            {
                var allowedIps = _appSettingService.GetConfig(MixAppSettingsSection.IpSecuritySettings, MixAppSettingKeywords.AllowedPortalIps, new JArray());
                string remoteIp = Request.HttpContext?.Connection?.RemoteIpAddress?.ToString();
                return forbidden || (
                        // add in allowedIps "::1" to allow localhost
                        allowedIps.Count > 0 &&
                        !allowedIps.Any(t => t["text"].Value<string>() == remoteIp)
                );
            }
        }

        public PortalController(
            MixAppSettingService appSettingService, 
            MixService mixService, 
            MixDatabaseService databaseService)
            : base(appSettingService, mixService)
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
            if (_forbiddenPortal)
            {
                isValid = false;
                _redirectUrl = $"/403";
            }

            base.ValidateRequest();

            // If this site has not been inited yet
            if (_appSettingService.GetConfig<bool>(
                   MixAppSettingsSection.GlobalSettings, MixAppSettingKeywords.IsInit))
            {
                isValid = false;
                if (string.IsNullOrEmpty(_databaseService.GetConnectionString(MixConstants.CONST_CMS_CONNECTION)))
                {
                    _redirectUrl = "Init";
                }
                else
                {
                    var status = _appSettingService.GetConfig<string>(
                        MixAppSettingsSection.GlobalSettings, MixAppSettingKeywords.InitStatus);
                    _redirectUrl = $"/init/step{status}";
                }
            }
        }

        #endregion overrides
    }
}