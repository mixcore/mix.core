using Mix.Lib.Services;
using Mix.Queue.Engines;
using Mix.Queue.Engines.MixQueue;

namespace Mixcore.Domain.Subscribers
{
    public sealed class TenantSubscriber : SubscriberBase
    {
        private static readonly string TopicId = typeof(MixTenantSystemViewModel).FullName;
        private readonly IMixTenantService _mixTenantService;

        public TenantSubscriber(
            IServiceProvider serviceProvider, 
            IConfiguration configuration, 
            MixMemoryMessageQueue<MessageQueueModel> queueService,
            IMixTenantService mixTenantService) : base(TopicId, MixModuleNames.Mixcore, serviceProvider, configuration, queueService)
        {
            _mixTenantService = mixTenantService;
        }

        public override async Task Handler(MessageQueueModel data)
        {
            switch (data.Action)
            {
                case "Get":
                    break;
                case "Post":
                case "Put":
                case "Patch":
                case "Delete":
                    await _mixTenantService.Reload();
                    break;
                default:
                    break;
            }
        }
    }
}
