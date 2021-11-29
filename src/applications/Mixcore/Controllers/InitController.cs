using Microsoft.AspNetCore.Mvc;
using Mix.Shared.Services;
using Mixcore.Domain.Constants;
using Mix.Lib.Services;

namespace Mixcore.Controllers
{
    public class InitController : MixControllerBase
    {
        public InitController(
            MixService mixService,
            IPSecurityConfigService ipSecurityConfigService) 
            : base(mixService, ipSecurityConfigService)
        {
        }

        [HttpGet]
        [Route("init")]
        [Route("init/{page}")]
        public IActionResult Index(string page)
        {
            if (!GlobalConfigService.Instance.AppSettings.IsInit)
            {
                return Redirect("/");
            }
            else
            {
                page ??= "";
                var initStatus = GlobalConfigService.Instance.AppSettings.InitStatus;
                switch (initStatus)
                {
                    case InitStep.Blank:
                        if (page.ToLower() != "")
                        {
                            return Redirect(InitRoutePath.Default);
                        }
                        break;

                    case InitStep.InitTenant:
                        if (page.ToLower() != "step2")
                        {
                            return Redirect(InitRoutePath.Step2);
                        }
                        break;

                    case InitStep.InitAccount:
                        if (page.ToLower() != "step3")
                        {
                            return Redirect(InitRoutePath.Step3);
                        }
                        break;
                    case InitStep.SelectTheme:
                        if (page.ToLower() != "step4")
                        {
                            return Redirect(InitRoutePath.Step4);
                        }
                        break;
                }
                return View();
            }
        }
    }
}
