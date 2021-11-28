using Microsoft.AspNetCore.Mvc;
using Mix.Lib.Services;
using Mix.Shared.Services;
using Mix.Database.Services;
using Mixcore.Domain.Bases;

namespace Mixcore.Controllers
{
    public class HomeController : MvcBaseController
    {
        private readonly ILogger<HomeController> _logger;
        public HomeController(
            ILogger<HomeController> logger,
            IPSecurityConfigService ipSecurityConfigService,
            MixService mixService,
            TranslatorService translator, 
            MixDatabaseService databaseService,
            MixCmsContext context) : base(ipSecurityConfigService, mixService, translator, databaseService, context)
        {
            _logger = logger;
        }
        
        protected override void ValidateRequest()
        {
            base.ValidateRequest();

            // If this site has not been inited yet
            if (GlobalConfigService.Instance.AppSettings.IsInit)
            {
                isValid = false;
                if (string.IsNullOrEmpty(_databaseService.GetConnectionString(MixConstants.CONST_CMS_CONNECTION)))
                {
                    _redirectUrl = "Init";
                }
                else
                {
                    var status = GlobalConfigService.Instance.AppSettings.InitStatus;
                    _redirectUrl = $"/init/step{status}";
                }
            }
        }

        [Route("")]
        public async Task<IActionResult> Index(string seoName, string keyword)
        {
            if (!isValid)
            {
                return Redirect(_redirectUrl);
            }
            return await Page(1, keyword);
        }
    }
}
