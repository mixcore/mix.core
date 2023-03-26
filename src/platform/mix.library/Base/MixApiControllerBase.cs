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
        protected readonly TranslatorService Translator;

        protected MixApiControllerBase(
            IConfiguration configuration,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            IQueueService<MessageQueueModel> queueService) : base()
        {
            Configuration = configuration;
            Translator = translator;
            MixIdentityService = mixIdentityService;
            QueueService = queueService;
        }
    }
}
