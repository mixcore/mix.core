using Microsoft.AspNetCore.Mvc;
using Mix.Lib.Constants;
using Mix.Lib.Controllers;
using Mix.Lib.Services;

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
                var initStatus = MixAppSettingService.GetConfig<int>("InitStatus");
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
        }
    }
}
