using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Mix.Lib.Services;

namespace Mix.Lib.Base
{
    public abstract class MixApiControllerBase : Controller
    {
        protected int MixTenantId { get; set; }
        protected string _lang;
        protected MixCulture _culture;
        protected readonly IQueueService<MessageQueueModel> _queueService;
        protected readonly IConfiguration _configuration;
        protected readonly MixIdentityService _mixIdentityService;
        protected readonly MixService _mixService;
        protected readonly TranslatorService _translator;
        protected readonly EntityRepository<MixCmsContext, MixCulture, int> _cultureRepository;
        public MixApiControllerBase(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixService mixService,
            TranslatorService translator,
            EntityRepository<MixCmsContext, MixCulture, int> cultureRepository,
            MixIdentityService mixIdentityService,
            IQueueService<MessageQueueModel> queueService) : base()
        {
            _configuration = configuration;
            _mixService = mixService;
            _translator = translator;
            _cultureRepository = cultureRepository;
            _mixIdentityService = mixIdentityService;
            _queueService = queueService;
            if (httpContextAccessor.HttpContext.Session.GetInt32(MixRequestQueryKeywords.TenantId).HasValue)
            {
                MixTenantId = httpContextAccessor.HttpContext.Session.GetInt32(MixRequestQueryKeywords.TenantId).Value;
            }
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            if (!GlobalConfigService.Instance.AppSettings.IsInit)
            {
                _lang = RouteData?.Values[MixRequestQueryKeywords.Specificulture] != null
                    ? RouteData.Values[MixRequestQueryKeywords.Specificulture].ToString()
                    : GlobalConfigService.Instance.AppSettings.DefaultCulture;
                _culture = _cultureRepository.GetFirst(c => c.Specificulture == _lang);
            }
        }
    }
}
