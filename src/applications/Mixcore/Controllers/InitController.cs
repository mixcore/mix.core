using Microsoft.AspNetCore.Mvc;
using Mix.Shared.Models.Configurations;
using Mix.Shared.Services;
using Mixcore.Domain.Constants;

namespace Mixcore.Controllers
{
    public class InitController : MixControllerBase
    {
        private readonly MixEndpointService _mixEndpointService;
        private readonly GlobalSettingsModel _globalConfig;
        public InitController(
            IHttpContextAccessor httpContextAccessor,
            IMixCmsService mixCmsService,
            IPSecurityConfigService ipSecurityConfigService,
            MixEndpointService mixEndpointService,
            IMixTenantService tenantService,
             IConfiguration configuration)
            : base(httpContextAccessor, mixCmsService, ipSecurityConfigService, tenantService, configuration)
        {
            _mixEndpointService = mixEndpointService;
            _globalConfig = configuration.GetSection(MixAppSettingsSection.GlobalSettings).Get<GlobalSettingsModel>()!;
        }

        [HttpGet]
        [Route("init")]
        [Route("init/{page}")]
        public IActionResult Index(string page)
        {
            if (!_globalConfig.IsInit)
            {
                return Redirect("/");
            }
            else
            {
                page ??= "";
                var initStatus = _globalConfig.InitStatus;

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
