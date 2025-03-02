using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Mix.Lib.Interfaces;
using Mix.Lib.Services;
using Mix.Mq.Lib.Models;

namespace Mix.Lib.Base
{
    public abstract class MixApiControllerBase : Controller
    {
        protected readonly IMemoryQueueService<MessageQueueModel> QueueService;
        protected readonly IConfiguration Configuration;
        protected readonly IMixCmsService MixCmsService;
        protected readonly IHttpContextAccessor HttpContextAccessor;
        protected readonly MixIdentityService MixIdentityService;
        protected readonly MixCacheService CacheService;

        protected MixApiControllerBase(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixCacheService cacheService,
            MixIdentityService mixIdentityService,
            IMemoryQueueService<MessageQueueModel> queueService) : base()
        {
            HttpContextAccessor = httpContextAccessor;
            CacheService = cacheService;
            Configuration = configuration;
            MixIdentityService = mixIdentityService;
            QueueService = queueService;
        }
    }
}
