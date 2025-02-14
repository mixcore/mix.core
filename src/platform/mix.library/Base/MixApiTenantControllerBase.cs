using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Lib.Interfaces;
using Mix.Lib.Services;
using Mix.Mq.Lib.Models;

namespace Mix.Lib.Base
{
    [ResponseCache(CacheProfileName = "Default")]
    public abstract class MixTenantApiControllerBase : Controller
    {
        protected readonly MixCacheService CacheService;
        protected IHttpContextAccessor HttpContextAccessor;
        protected ISession Session;
        protected string Lang;
        protected MixCulture Culture;
        protected readonly IMemoryQueueService<MessageQueueModel> QueueService;
        protected readonly IConfiguration Configuration;
        protected readonly MixIdentityService MixIdentityService;
        protected readonly TranslatorService Translator;
        protected IMixTenantService MixTenantService;
        protected MixTenantSystemModel CurrentTenant { get; set; }
        protected MixTenantApiControllerBase(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixCacheService cacheService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            IMemoryQueueService<MessageQueueModel> queueService,
            IMixTenantService mixTenantService)
        {
            HttpContextAccessor = httpContextAccessor;
            Session = httpContextAccessor.HttpContext?.Session;
            Configuration = configuration;
            CacheService = cacheService;
            Translator = translator;
            MixIdentityService = mixIdentityService;
            QueueService = queueService;
            MixTenantService = mixTenantService;


        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (Configuration.GetValue<InitStep>("InitStatus") == InitStep.Blank)
            {
                return;
            }
            CurrentTenant = Session.Get<MixTenantSystemModel>(MixRequestQueryKeywords.Tenant);
            CurrentTenant ??= MixTenantService.GetDefaultTenant().GetAwaiter().GetResult();
            Lang = RouteData.Values[MixRequestQueryKeywords.Specificulture] != null
                ? RouteData.Values[MixRequestQueryKeywords.Specificulture].ToString()
                : CurrentTenant.Configurations.DefaultCulture;

            Culture = CurrentTenant.Cultures.FirstOrDefault(c => c.Specificulture == Lang) ?? CurrentTenant.Cultures.FirstOrDefault();
            base.OnActionExecuting(context);
        }

        protected bool IsValidCulture(string culture)
        {
            return CurrentTenant != null && !string.IsNullOrEmpty(culture)
                && CurrentTenant.Cultures.Any(m => m.Specificulture == culture);
        }
    }
}
