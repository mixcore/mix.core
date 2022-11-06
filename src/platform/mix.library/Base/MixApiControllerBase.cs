using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Mix.Lib.Services;

namespace Mix.Lib.Base
{
    public abstract class MixApiControllerBase : Controller
    {
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
            MixIdentityService mixIdentityService,
            IQueueService<MessageQueueModel> queueService) : base()
        {
            _configuration = configuration;
            _mixService = mixService;
            _translator = translator;
            _mixIdentityService = mixIdentityService;
            _queueService = queueService;
        }
    }
}
