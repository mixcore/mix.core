using Microsoft.AspNetCore.Mvc;
using Mix.Lib.Controllers;
using Mix.Shared.Services;
using Mix.Lib.Services;

namespace Mixcore.Controllers
{
    public class SecurityController : MixControllerBase
    {
        public SecurityController(MixAppSettingService appSettingService, MixService mixService) 
            : base(appSettingService, mixService)
        {
        }

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
    }
}
