using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
        private readonly IMixTenantService _mixTenantService;
        public MixViewModelChangedSubscriber(
            IServiceProvider serviceProvider,
            IConfiguration configuration,
            MixQueueMessages<MessageQueueModel> mixQueueService,
            IMixTenantService mixTenantService,
            IQueueService<MessageQueueModel> queueService)
            : base(TopicId, nameof(MixDbCommandSubscriber), 20, serviceProvider, configuration, mixQueueService, queueService)
        {
            _mixTenantService = mixTenantService;
        }

        public override async Task Handler(MessageQueueModel data)
        {
            switch (data.DataTypeFullName)
            {
                case var m when m == typeof(MixTemplateViewModel).FullName:
                    await TemplateHandler.MessageQueueHandler(data);
                    break;
                case var m when m == typeof(MixPageContentViewModel).FullName:
                    await PageContentHandler.MessageQueueHandler(data, CacheService);
                    break;
                case var m when m == typeof(MixTenantSystemViewModel).FullName:
                    await MixTenantSystemViewModelHandler.MessageQueueHandler(data, _mixTenantService);
                    break;
                case var m when m == typeof(MixTenantSystemViewModel).FullName:
                    await MixDomainViewModelHandler.MessageQueueHandler(data, _mixTenantService);
                    break;
                default:
                    break;
            }
        }

    }
}
