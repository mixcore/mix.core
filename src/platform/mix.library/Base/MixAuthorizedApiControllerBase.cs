using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Lib.Services;

namespace Mix.Lib.Base
{
    [MixAuthorize]
    public abstract class MixAuthorizedApiControllerBase : Controller
    {
        protected IHttpContextAccessor HttpContextAccessor;
        protected ISession Session;
        protected string Lang;
        protected MixCulture Culture;
        protected UnitOfWorkInfo Uow;
        protected readonly ILogger<MixTenantApiControllerBase> Logger;
        protected readonly MixIdentityService MixIdentityService;
        protected readonly TranslatorService Translator;
        protected readonly EntityRepository<MixCmsContext, MixCulture, int> CultureRepository;
        protected MixTenantSystemModel CurrentTenant => Session.Get<MixTenantSystemModel>(MixRequestQueryKeywords.Tenant);

        public IConfiguration Configuration { get; }

        protected MixAuthorizedApiControllerBase(
            ILogger<MixTenantApiControllerBase> logger,
            TranslatorService translator,
            EntityRepository<MixCmsContext, MixCulture, int> cultureRepository,
            MixIdentityService mixIdentityService,
            MixCmsContext context,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
        {
            Uow = new UnitOfWorkInfo(context);
            Logger = logger;
            Translator = translator;
            CultureRepository = cultureRepository;
            MixIdentityService = mixIdentityService;
            HttpContextAccessor = httpContextAccessor;
            Configuration = configuration;
            Session = httpContextAccessor.HttpContext?.Session;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            if (!Configuration.GetValue<bool>("IsInit"))
            {
                Lang = RouteData.Values["lang"] != null
                    ? RouteData.Values["lang"].ToString()
                    : CurrentTenant.Configurations.DefaultCulture;
                Culture = CultureRepository.GetFirst(c => c.Specificulture == Lang);
            }
        }
    }
}
