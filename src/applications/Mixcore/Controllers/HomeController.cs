using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mix.Shared.Constants;
using Mix.Lib.Controllers;
using Mix.Lib.Services;
using System.Threading.Tasks;
using Mix.Shared.Services;

namespace Mixcore.Controllers
{
    public class HomeController : MixControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TranslatorService _translator;
        public HomeController(
            ILogger<HomeController> logger,
            MixService mixService,
            TranslatorService translator) : base(mixService)
        {
            _logger = logger;
            _translator = translator;
        }
        protected override void ValidateRequest()
        {
            base.ValidateRequest();

            // If this site has not been inited yet
            if (MixAppSettingService.GetConfig<bool>(MixAppSettingKeywords.IsInit))
            {
                isValid = false;
                if (string.IsNullOrEmpty(MixService.GetConnectionString(MixConstants.CONST_CMS_CONNECTION)))
                {
                    _redirectUrl = "Init";
                }
                else
                {
                    var status = MixAppSettingService.GetConfig<string>("InitStatus");
                    _redirectUrl = $"/init/step{status}";
                }
            }
        }

        [Route("")]
        [Route("{seoName}")]
        [Route("{seoName}/{keyword}")]
        [Route("{culture}/{seoName}/{keyword}")]
        public async Task<IActionResult> Index(string seoName, string keyword)
        {
            if (!isValid)
            {
                return Redirect(_redirectUrl);
            }
            return View();
        }
    }
}
