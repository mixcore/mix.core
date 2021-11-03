using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mix.Shared.Constants;
using Mix.Lib.Services;
using Mix.Shared.Services;
using Mix.Shared.Enums;
using Mix.Database.Services;
using Mix.Lib.Abstracts;
using Mixcore.Domain.ViewModels;

namespace Mixcore.Controllers
{
    public class HomeController : MixControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TranslatorService _translator;
        private readonly MixDatabaseService _databaseService;
        public HomeController(
            ILogger<HomeController> logger,
            GlobalConfigService globalConfigService,
            IPSecurityConfigService ipSecurityConfigService,
            MixService mixService,
            TranslatorService translator, 
            MixDatabaseService databaseService) : base(globalConfigService, mixService, ipSecurityConfigService)
        {
            _logger = logger;
            _translator = translator;
            _databaseService = databaseService;
        }
        
        protected override void ValidateRequest()
        {
            base.ValidateRequest();

            // If this site has not been inited yet
            if (_globalConfigService.AppSettings.IsInit)
            {
                isValid = false;
                if (string.IsNullOrEmpty(_databaseService.GetConnectionString(MixConstants.CONST_CMS_CONNECTION)))
                {
                    _redirectUrl = "Init";
                }
                else
                {
                    var status = _globalConfigService.AppSettings.InitStatus;
                    _redirectUrl = $"/init/step{status}";
                }
            }
        }

        [Route("")]
        [Route("{seoName}")]
        [Route("{seoName}/{keyword}")]
        public IActionResult Index(string seoName, string keyword)
        {
            if (!isValid)
            {
                return Redirect(_redirectUrl);
            }
            return View(new PageContentViewModel() { 
                SeoName = seoName
            });
        }
    }
}
