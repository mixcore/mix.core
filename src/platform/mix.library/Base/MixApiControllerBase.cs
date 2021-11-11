using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Mix.Database.Entities.Cms;
using Mix.Heart.Repository;
using Mix.Lib.Services;
using Mix.Shared.Services;

namespace Mix.Lib.Base
{
    public abstract class MixApiControllerBase : Controller
    {
        protected string _lang;
        protected MixCulture _culture;
        protected readonly IConfiguration _configuration;
        protected readonly GlobalConfigService _globalConfigService;
        protected readonly MixIdentityService _mixIdentityService;
        protected readonly MixService _mixService;
        protected readonly TranslatorService _translator;
        protected readonly EntityRepository<MixCmsContext, MixCulture, int> _cultureRepository;
        public MixApiControllerBase(
            IConfiguration configuration,
            GlobalConfigService globalConfigService,
            MixService mixService,
            TranslatorService translator,
            EntityRepository<MixCmsContext, MixCulture, int> cultureRepository, 
            MixIdentityService mixIdentityService) : base()
        {
            _configuration = configuration;
            _globalConfigService = globalConfigService;
            _mixService = mixService;
            _translator = translator;
            _cultureRepository = cultureRepository;
            _mixIdentityService = mixIdentityService;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            if (!_globalConfigService.AppSettings.IsInit)
            {
                _lang = RouteData?.Values["lang"] != null
                    ? RouteData.Values["lang"].ToString()
                    : _globalConfigService.AppSettings.DefaultCulture;
                _culture = _cultureRepository.GetFirst(c => c.Specificulture == _lang);
            }
        }
    }
}
