using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Mix.Lib.Services;

namespace Mix.Lib.Base
{
    public abstract class MixTenantApiControllerBase : Controller
    {
        protected IHttpContextAccessor HttpContextAccessor;
        protected ISession Session;
        protected string Lang;
        protected MixCulture Culture;
        protected readonly IQueueService<MessageQueueModel> QueueService;
        protected readonly IConfiguration Configuration;
        protected readonly MixIdentityService MixIdentityService;
        protected readonly MixService MixService;
        protected readonly TranslatorService Translator;
        protected MixTenantSystemModel CurrentTenant => Session.Get<MixTenantSystemModel>(MixRequestQueryKeywords.Tenant);
        protected MixTenantApiControllerBase(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixService mixService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            IQueueService<MessageQueueModel> queueService)
        {
            HttpContextAccessor = httpContextAccessor;
            Session = httpContextAccessor.HttpContext?.Session;
            Configuration = configuration;
            MixService = mixService;
            Translator = translator;
            MixIdentityService = mixIdentityService;
            QueueService = queueService;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            if (GlobalConfigService.Instance.AppSettings.IsInit)
            {
                return;
            }

            Lang = RouteData.Values[MixRequestQueryKeywords.Specificulture] != null
                ? RouteData.Values[MixRequestQueryKeywords.Specificulture].ToString()
                : CurrentTenant.Configurations.DefaultCulture;

            Culture = CurrentTenant.Cultures.FirstOrDefault(c => c.Specificulture == Lang) ?? CurrentTenant.Cultures.FirstOrDefault();
        }
    }
}
