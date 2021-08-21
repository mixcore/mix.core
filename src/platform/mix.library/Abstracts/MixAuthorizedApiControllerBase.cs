using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Mix.Database.Entities.Cms;
using Mix.Heart.Repository;
using Mix.Lib.Attributes;
using Mix.Lib.Services;
using Mix.Shared.Constants;
using Mix.Shared.Services;

namespace Mix.Lib.Abstracts
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [MixAuthorize]
    public abstract class MixAuthorizedApiControllerBase : Controller
    {
        protected string _lang;
        protected MixCulture _culture;
        protected readonly ILogger<MixApiControllerBase> _logger;
        protected readonly GlobalConfigService _globalConfigService;
        protected readonly MixIdentityService _mixIdentityService;
        protected readonly MixService _mixService;
        protected readonly TranslatorService _translator;
        protected readonly Repository<MixCmsContext, MixCulture, int> _cultureRepository;
        public MixAuthorizedApiControllerBase(
            ILogger<MixApiControllerBase> logger,
            GlobalConfigService globalConfigService,
            MixService mixService,
            TranslatorService translator,
            Repository<MixCmsContext, MixCulture, int> cultureRepository, 
            MixIdentityService mixIdentityService) : base()
        {
            _logger = logger;
            _globalConfigService = globalConfigService;
            _mixService = mixService;
            _translator = translator;
            _cultureRepository = cultureRepository;
            _mixIdentityService = mixIdentityService;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            if (!_globalConfigService.GetConfig<bool>(MixAppSettingKeywords.IsInit))
            {
                _lang = RouteData?.Values["lang"] != null
                    ? RouteData.Values["lang"].ToString()
                    : _globalConfigService.GetConfig<string>(MixAppSettingKeywords.DefaultCulture);
                _culture = _cultureRepository.GetFirst(c => c.Specificulture == _lang);
            }
        }
    }
}
