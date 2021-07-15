using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Mix.Database.Entities.Cms.v2;
using Mix.Heart.Repository;
using Mix.Lib.Services;
using Mix.Shared.Constants;
using Mix.Shared.Enums;
using Mix.Shared.Services;

namespace Mix.Lib.Abstracts
{
    public abstract class MixApiControllerBase : Controller
    {
        protected string _lang;
        protected MixCulture _culture;
        protected readonly ILogger<MixApiControllerBase> _logger;
        protected readonly MixAppSettingService _appSettingService;
        protected readonly MixService _mixService;
        protected readonly TranslatorService _translator;
        protected readonly Repository<MixCmsContext, MixCulture, int> _cultureRepository;
        public MixApiControllerBase(
            ILogger<MixApiControllerBase> logger,
            MixAppSettingService appSettingService,
            MixService mixService,
            TranslatorService translator,
            Repository<MixCmsContext, MixCulture, int> cultureRepository) : base()
        {
            _logger = logger;
            _appSettingService = appSettingService;
            _mixService = mixService;
            _translator = translator;
            _cultureRepository = cultureRepository;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            if (!_appSettingService.GetConfig<bool>(MixAppSettingsSection.GlobalSettings, MixAppSettingKeywords.IsInit))
            {
                _lang = RouteData?.Values["lang"] != null
                    ? RouteData.Values["lang"].ToString()
                    : _appSettingService.GetConfig<string>(MixAppSettingsSection.GlobalSettings, MixAppSettingKeywords.DefaultCulture);
                _culture = _cultureRepository.GetSingleAsync(c => c.Specificulture == _lang).GetAwaiter().GetResult();
            }
        }
    }
}
