using Microsoft.AspNetCore.Mvc;
using Mix.Lib.Services;
using Mix.Shared.Services;
using Mixcore.Domain.Constants;

namespace Mixcore.Controllers
{
    public class InitController : MixControllerBase
    {
        private readonly MixEndpointService _mixEndpointService;
        public InitController(
            IHttpContextAccessor httpContextAccessor,
            MixService mixService,
            IPSecurityConfigService ipSecurityConfigService,
            MixEndpointService mixEndpointService)
            : base(httpContextAccessor, mixService, ipSecurityConfigService)
        {
            _mixEndpointService = mixEndpointService;
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
            if (string.IsNullOrEmpty(_mixEndpointService.Messenger))
            {
                string endpoint = $"{Request.Scheme}://{Request.Host}";
                _mixEndpointService.Messenger = endpoint;
                _mixEndpointService.Portal = endpoint;
                _mixEndpointService.Grpc = endpoint;
                _mixEndpointService.Scheduler = endpoint;
                _mixEndpointService.Theme = endpoint;
                _mixEndpointService.Account = endpoint;
                _mixEndpointService.Common = endpoint;
                _mixEndpointService.Mixcore = endpoint;
                _mixEndpointService.SaveSettings();
            }
        }
    }
}
