using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Mix.Lib.Interfaces;
using Mix.Lib.Services;

namespace Mix.Lib.Base
{
    public abstract class MixApiControllerBase : Controller
    {
        protected readonly IQueueService<MessageQueueModel> QueueService;
        protected readonly IConfiguration Configuration;
        protected readonly IMixCmsService MixCmsService;
        protected readonly IHttpContextAccessor HttpContextAccessor;
        protected readonly MixIdentityService MixIdentityService;
        protected readonly TranslatorService Translator;
        protected readonly MixCacheService CacheService;

        protected MixApiControllerBase(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixCacheService cacheService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            IQueueService<MessageQueueModel> queueService) : base()
        {
            HttpContextAccessor = httpContextAccessor;
            CacheService = cacheService;
            Configuration = configuration;
            Translator = translator;
            MixIdentityService = mixIdentityService;
            QueueService = queueService;
        }
    }
}
