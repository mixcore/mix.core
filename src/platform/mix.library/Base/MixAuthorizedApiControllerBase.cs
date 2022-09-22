using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Mix.Lib.Extensions;
using Mix.Lib.Services;

namespace Mix.Lib.Base
{
    [MixAuthorize]
    public abstract class MixAuthorizedApiControllerBase : Controller
    {
        protected IHttpContextAccessor _httpContextAccessor;
        protected ISession _session;
        private MixTenantSystemViewModel _currentTenant;
        protected MixTenantSystemViewModel CurrentTenant
        {
            get
            {
                if (_currentTenant == null)
                {
                    _currentTenant = _session.Get<MixTenantSystemViewModel>(MixRequestQueryKeywords.Tenant);
                }
                return _currentTenant;
            }
        }
        protected string _lang;
        protected MixCulture _culture;
        protected UnitOfWorkInfo _uow;
        protected readonly ILogger<MixApiControllerBase> _logger;
        protected readonly MixIdentityService _mixIdentityService;
        protected readonly MixService _mixService;
        protected readonly TranslatorService _translator;
        protected readonly EntityRepository<MixCmsContext, MixCulture, int> _cultureRepository;
        public MixAuthorizedApiControllerBase(
            ILogger<MixApiControllerBase> logger,
            MixService mixService,
            TranslatorService translator,
            EntityRepository<MixCmsContext, MixCulture, int> cultureRepository,
            MixIdentityService mixIdentityService,
            MixCmsContext context
,
            IHttpContextAccessor httpContextAccessor) : base()
        {
            _uow = new UnitOfWorkInfo(context);
            _logger = logger;
            _mixService = mixService;
            _translator = translator;
            _cultureRepository = cultureRepository;
            _mixIdentityService = mixIdentityService;
            _httpContextAccessor = httpContextAccessor;
            _session = httpContextAccessor.HttpContext.Session;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            if (!GlobalConfigService.Instance.AppSettings.IsInit)
            {
                _lang = RouteData?.Values["lang"] != null
                    ? RouteData.Values["lang"].ToString()
                    : CurrentTenant.Configurations.DefaultCulture;
                _culture = _cultureRepository.GetFirst(c => c.Specificulture == _lang);
            }
        }
    }
}
