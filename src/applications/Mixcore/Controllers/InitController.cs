using Microsoft.AspNetCore.Mvc;
using Mix.Lib.Services;
using Mix.Shared.Services;
using Mixcore.Domain.Constants;

namespace Mixcore.Controllers
{
    public class InitController : MixControllerBase
    {
        public InitController(
            IHttpContextAccessor httpContextAccessor,
            MixService mixService,
            IPSecurityConfigService ipSecurityConfigService)
            : base(httpContextAccessor, mixService, ipSecurityConfigService)
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
                        InitEndpoints();
                        if (!string.IsNullOrEmpty(page.ToLower()))
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

        private void InitEndpoints()
        {
            if (string.IsNullOrEmpty(MixEndpointService.Instance.Messenger))
            {
                string endpoint = $"{Request.Scheme}://{Request.Host}";
                MixEndpointService.Instance.Messenger = endpoint;
                MixEndpointService.Instance.Portal = endpoint;
                MixEndpointService.Instance.Grpc = endpoint;
                MixEndpointService.Instance.Scheduler = endpoint;
                MixEndpointService.Instance.Theme = endpoint;
                MixEndpointService.Instance.Account = endpoint;
                MixEndpointService.Instance.Common = endpoint;
                MixEndpointService.Instance.Mixcore = endpoint;
                MixEndpointService.Instance.SaveSettings();
            }
        }
    }
}
