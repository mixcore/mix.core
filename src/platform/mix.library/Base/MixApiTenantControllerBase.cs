using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Mix.Lib.Extensions;
using Mix.Lib.Models;
using Mix.Lib.Services;

namespace Mix.Lib.Base
{
    public abstract class MixTenantApiControllerBase : Controller
    {
        protected IHttpContextAccessor _httpContextAccessor;
        protected ISession Session;
        protected string _lang;
        protected MixCulture _culture;
        protected readonly IQueueService<MessageQueueModel> _queueService;
        protected readonly IConfiguration _configuration;
        protected readonly MixIdentityService _mixIdentityService;
        protected readonly MixService _mixService;
        protected readonly TranslatorService _translator;
        protected MixTenantSystemModel CurrentTenant => Session.Get<MixTenantSystemModel>(MixRequestQueryKeywords.Tenant);
        public MixTenantApiControllerBase(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixService mixService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            IQueueService<MessageQueueModel> queueService)
        {
            _httpContextAccessor = httpContextAccessor;
            Session = httpContextAccessor.HttpContext.Session;
            _configuration = configuration;
            _mixService = mixService;
            _translator = translator;
            _mixIdentityService = mixIdentityService;
            _queueService = queueService;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            if (!GlobalConfigService.Instance.AppSettings.IsInit)
            {
                _lang = RouteData?.Values[MixRequestQueryKeywords.Specificulture] != null
                    ? RouteData.Values[MixRequestQueryKeywords.Specificulture].ToString()
                    : CurrentTenant.Configurations.DefaultCulture;
                _culture = CurrentTenant.Cultures.FirstOrDefault(c => c.Specificulture == _lang) ?? CurrentTenant.Cultures.FirstOrDefault();
            }
        }
    }
}
