using Microsoft.AspNetCore.Mvc;
using Mix.Shared.Services;
using Mix.Lib.Services;
using Mix.Lib.Abstracts;

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
