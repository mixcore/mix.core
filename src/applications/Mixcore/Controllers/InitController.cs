using Microsoft.AspNetCore.Mvc;
using Mix.Shared.Constants;
using Mix.Lib.Controllers;
using Mix.Shared.Services;
using Mixcore.Domain.Constants;

namespace Mixcore.Controllers
{
    public class InitController : MixControllerBase
    {
        public InitController(MixService mixService) : base(mixService)
        {
        }

        [HttpGet]
        [Route("init")]
        [Route("init/{page}")]
        public IActionResult Index(string page)
        {
            if (!MixAppSettingService.GetConfig<bool>(MixAppSettingKeywords.IsInit))
            {
                return Redirect("/");
            }
            else
            {
                page = page ?? "";
                var initStatus = MixAppSettingService.GetConfig<int>(MixAppSettingKeywords.InitStatus);
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
