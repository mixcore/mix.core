using Microsoft.AspNetCore.Mvc;
using Mix.Database.Services;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Heart.Helpers;
using Mix.Lib.Extensions;
using Mix.Shared.Models.Configurations;
using Mixcore.Domain.Constants;

namespace Mixcore.Controllers
{
    public class InitController : MixControllerBase
    {
        private readonly AppSettingsService _appSettingsService;
        private readonly MixEndpointService _mixEndpointService;
        private readonly GlobalSettingsModel _globalConfig;
        public InitController(
            IHttpContextAccessor httpContextAccessor,
            IMixCmsService mixCmsService,
            IPSecurityConfigService ipSecurityConfigService,
            MixEndpointService mixEndpointService,
            IMixTenantService tenantService,
             IConfiguration configuration,
             AppSettingsService appSettingsService)
            : base(httpContextAccessor, mixCmsService, ipSecurityConfigService, tenantService, configuration)
        {
            _mixEndpointService = mixEndpointService;
            _appSettingsService = appSettingsService;
        }

        [HttpGet]
        [Route("init")]
        [Route("init/{page}")]
        public IActionResult Index(string page)
        {
            if (!Configuration.IsInit())
            {
                return Redirect("/");
            }
            else
            {
                if (string.IsNullOrEmpty(Configuration.AesKey()))
                {
                    var newKey = AesEncryptionHelper.GenerateCombinedKeys();
                    _appSettingsService.SetConfig("AesKey", newKey);
                    Configuration["AesKey"] = newKey;
                    _appSettingsService.SaveSettings();
                }
                page ??= "";
                switch (Configuration.InitStep())
                {
                    case InitStep.Blank:
                    case InitStep.InitTenant:
                        InitEndpoints();
                        if (!string.IsNullOrEmpty(page.ToLower()))
                        {
                            return Redirect(InitRoutePath.Default);
                        }
                        break;
                    case InitStep.InitAccount:
                        if (page.ToLower() != "step2")
                        {
                            return Redirect(InitRoutePath.Step2);
                        }
                        break;
                    case InitStep.SelectTheme:
                        if (page.ToLower() != "step3")
                        {
                            return Redirect(InitRoutePath.Step3);
                        }
                        break;
                    case InitStep.InitTheme:
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
