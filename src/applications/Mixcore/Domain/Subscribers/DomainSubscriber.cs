using Mix.Lib.Services;
using Mix.Queue.Engines;
using Mix.Queue.Engines.MixQueue;

namespace Mixcore.Domain.Subscribers
{
    public sealed class DomainSubscriber : SubscriberBase
    {
        private readonly MixTenantService _mixTenantService;
        private static readonly string TopicId = typeof(MixDomainViewModel).FullName;
        public DomainSubscriber(
            IServiceProvider serviceProvider,
            IConfiguration configuration,
            MixMemoryMessageQueue<MessageQueueModel> queueService,
            MixTenantService mixTenantService)
            : base(TopicId, MixModuleNames.Mixcore, serviceProvider, configuration, queueService)
        {
            _mixTenantService = mixTenantService;
        }

        public override Task Handler(MessageQueueModel data)
        {
            switch (data.Action)
            {
                case "Get":
                    break;
                case "Post":
                case "Put":
                case "Patch":
                case "Delete":
                    _ = _mixTenantService.Reload();
                    break;
                default:
                    break;
            }
            return Task.CompletedTask;
        }
    }
}
