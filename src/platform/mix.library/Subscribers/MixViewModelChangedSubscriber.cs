using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Mix.Lib.Interfaces;
using Mix.Lib.Subscribers.Handlers.MixViewModelChangedHandlers;
using Mix.Queue.Engines;
using Mix.Queue.Engines.MixQueue;
using Org.BouncyCastle.Asn1.X509.Qualified;

namespace Mix.Lib.Subscribers
{
    public sealed class MixViewModelChangedSubscriber : SubscriberBase
    {
        private static readonly string TopicId = MixQueueTopics.MixViewModelChanged;
        private readonly ILogger<MixViewModelChangedSubscriber> _logger;
        private readonly MixCacheService _cacheService;
        private readonly IMixTenantService _mixTenantService;
        public MixViewModelChangedSubscriber(
            ILogger<MixViewModelChangedSubscriber> logger,
            IServiceProvider serviceProvider,
            IConfiguration configuration,
            MixMemoryMessageQueue<MessageQueueModel> queueService,
            MixCacheService cacheService,
            IMixTenantService mixTenantService) : base(TopicId, MixModuleNames.Mixcore, serviceProvider, configuration, queueService)
        {
            _logger = logger;
            _cacheService = cacheService;
            _mixTenantService = mixTenantService;
        }

        public override Task Handler(MessageQueueModel data)
        {
            switch (data.DataTypeFullName)
            {
                case var m when m == typeof(MixTemplateViewModel).FullName:
                    TemplateHandler.MessageQueueHandler(data);
                    break;
                case var m when m == typeof(MixPageContentViewModel).FullName:
                    PageContentHandler.MessageQueueHandler(data, _cacheService);
                    break;
                case var m when m == typeof(MixTenantSystemViewModel).FullName:
                    MixTenantSystemViewModelHandler.MessageQueueHandler(data, _mixTenantService);
                    break;
                case var m when m == typeof(MixTenantSystemViewModel).FullName:
                    MixDomainViewModelHandler.MessageQueueHandler(data, _mixTenantService);
                    break;
                default:
                    break;
            }

            return Task.CompletedTask;
        }

    }
}
