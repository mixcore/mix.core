using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mix.Cms.Lib;
using Mix.Cms.Lib.Services;
using Mix.Cms.Web.Models;
using Mix.Identity.Models;

namespace Mix.Cms.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        public HomeController(ILogger<HomeController> logger,
            UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            if (MixService.GetConfig<bool>("IsInit"))
            {
                //Go to landing page
                return Redirect($"/{_culture}");
            }
            else
            {
                if (string.IsNullOrEmpty(MixService.GetConnectionString(MixConstants.CONST_CMS_CONNECTION)))
                {
                    return Redirect("Init");
                }
                else
                {
                    return Redirect($"/init/step2");
                }
            }
        }

        #region Security Routes
        [HttpGet]
        [Route("security/{page}")]
        public IActionResult Security(string page)
        {
            if (_forbidden)
            {
                return Redirect($"/error/403");
            }
            if (string.IsNullOrEmpty(page) && MixService.GetConfig<bool>("IsInit"))
            {
                return Redirect($"/init/login");
            }
            else
            {
                return View();
            }

        }
        #endregion

        #region portal routes

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
        public IActionResult Portal()
        {
            if (_forbiddenPortal)
            {
                return Redirect($"/error/403");
            }
            return View();
        }
        #endregion

        #region Init Routes
        [HttpGet]
        [Route("init")]
        [Route("init/{page}")]
        public IActionResult Init(string page)
        {
            page = page ?? "";
            var initStatus = MixService.GetConfig<int>("InitStatus");
            switch (initStatus)
            {
                case 0:
                    if (page.ToLower() != "")
                    {
                        return Redirect($"/init");
                    }
                    break;
                case 1:
                    if (page.ToLower() != "step2")
                    {
                        return Redirect($"/init/step2");
                    }
                    break;
                case 2:
                    if (page.ToLower() != "step3")
                    {
                        return Redirect($"/init/step3");
                    }
                    break;
                case 3:
                    if (page.ToLower() != "step4")
                    {
                        return Redirect($"/init/step4");
                    }
                    break;
                case 4:
                    if (page.ToLower() != "step5")
                    {
                        return Redirect($"/init/step5");
                    }
                    break;

            }
            return View();

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #endregion

    }
}
