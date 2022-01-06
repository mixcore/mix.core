using Microsoft.AspNetCore.Mvc;
using Mix.Shared.Services;
using Mix.Lib.Services;
using Mix.Lib.Base;

namespace Mixcore.Controllers
{
    public class SecurityController : MixControllerBase
    {
        public SecurityController(
            MixService mixService,
            IPSecurityConfigService ipSecurityConfigService,
            MixCacheService cacheService)
            : base(mixService, ipSecurityConfigService)
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
