using Microsoft.AspNetCore.Mvc;
using Mix.Shared.Constants;
using Mix.Lib.Abstracts;
using Mix.Shared.Services;
using Mixcore.Domain.Constants;
using Mix.Shared.Enums;
using Mix.Lib.Services;

namespace Mixcore.Controllers
{
    public class InitController : MixControllerBase
    {
        public InitController(
            GlobalConfigService globalConfigService, 
            MixService mixService,
            IPSecurityConfigService ipSecurityConfigService) 
            : base(globalConfigService, mixService, ipSecurityConfigService)
        {
        }

        [HttpGet]
        [Route("init")]
        [Route("init/{page}")]
        public IActionResult Index(string page)
        {
            if (!_globalConfigService.GetConfig<bool>(MixAppSettingKeywords.IsInit))
            {
                return Redirect("/");
            }
            else
            {
                page ??= "";
                var initStatus = _globalConfigService.GetConfig<int>(MixAppSettingKeywords.InitStatus);
                switch (initStatus)
                {
                    case 0:
                        if (page.ToLower() != "")
                        {
                            return Redirect(InitRoutePath.Default);
                        }
                        break;

                    case 1:
                        if (page.ToLower() != "step2")
                        {
                            return Redirect(InitRoutePath.Step2);
                        }
                        break;

                    case 2:
                        if (page.ToLower() != "step3")
                        {
                            return Redirect(InitRoutePath.Step3);
                        }
                        break;
                }
                return View();
            }
        }
    }
}
