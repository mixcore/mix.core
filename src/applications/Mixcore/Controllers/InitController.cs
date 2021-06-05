using Microsoft.AspNetCore.Mvc;
using Mix.Shared.Constants;
using Mix.Lib.Controllers;
using Mix.Shared.Services;
using Mixcore.Domain.Constants;
using Mix.Shared.Enums;
using Mix.Lib.Services;

namespace Mixcore.Controllers
{
    public class InitController : MixControllerBase
    {
        public InitController(MixAppSettingService appSettingService, MixService mixService) 
            : base(appSettingService, mixService)
        {
        }

        [HttpGet]
        [Route("init")]
        [Route("init/{page}")]
        public IActionResult Index(string page)
        {
            if (!MixAppSettingService.Instance.GetConfig<bool>(MixAppSettingsSection.GlobalSettings, MixAppSettingKeywords.IsInit))
            {
                return Redirect("/");
            }
            else
            {
                page = page ?? "";
                var initStatus = _appSettingService.GetConfig<int>(
                    MixAppSettingsSection.GlobalSettings, MixAppSettingKeywords.InitStatus);
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
