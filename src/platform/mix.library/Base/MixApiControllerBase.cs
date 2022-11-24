using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Mix.Lib.Services;

namespace Mix.Lib.Base
{
    public abstract class MixApiControllerBase : Controller
    {
        protected readonly IQueueService<MessageQueueModel> QueueService;
        protected readonly IConfiguration Configuration;
        protected readonly MixIdentityService MixIdentityService;
        protected readonly MixService MixService;
        protected readonly TranslatorService Translator;

        protected MixApiControllerBase(
            IConfiguration configuration,
            MixService mixService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            IQueueService<MessageQueueModel> queueService) : base()
        {
            Configuration = configuration;
            MixService = mixService;
            Translator = translator;
            MixIdentityService = mixIdentityService;
            QueueService = queueService;
        }
    }
}
