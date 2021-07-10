using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Mix.Lib.Services;
using Mix.Shared.Constants;
using Mix.Shared.Enums;
using Mix.Shared.Services;

namespace Mix.Lib.Abstracts
{
    [Produces("application/json")]
    [ApiController]
    public abstract class MixApiControllerBase: Controller
    {
        protected string _lang;
        protected readonly ILogger<MixApiControllerBase> _logger;
        protected readonly MixAppSettingService _appSettingService;
        protected readonly MixService _mixService;
        protected readonly TranslatorService _translator;
        public MixApiControllerBase(
            ILogger<MixApiControllerBase> logger,
            MixAppSettingService appSettingService,
            MixService mixService,
            TranslatorService translator) : base()
        {
            _logger = logger;
            _appSettingService = appSettingService;
            _mixService = mixService;
            _translator = translator;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            _lang = RouteData?.Values["culture"] != null 
                ? RouteData.Values["culture"].ToString() 
                : _appSettingService.GetConfig<string>(MixAppSettingsSection.GlobalSettings, MixAppSettingKeywords.DefaultCulture);
        }
    }
}
