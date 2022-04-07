using Mix.Lib.Subscribers;
using Mix.Queue.Engines.MixQueue;

namespace Mixcore.Domain.Subscribers
{
    public class PageContentSubscriber : SubscriberBase
    {
        static string topicId = typeof(MixPageContentViewModel).FullName;
        private readonly ILogger<PageContentSubscriber> logger;
        private readonly MixCacheService cacheService;

        public PageContentSubscriber(
            ILogger<PageContentSubscriber> logger,
            IConfiguration configuration,
            MixMemoryMessageQueue<MessageQueueModel> queueService,
            MixCacheService cacheService)
            : base(topicId, MixModuleNames.Mixcore, configuration, queueService)
        {
            this.logger = logger;
            this.cacheService = cacheService;
        }

        public override async Task Handler(MessageQueueModel data)
        {
            var template = data.Data.ToObject<MixPageContentViewModel>();
            switch (data.Action)
            {
                case "Get":
                    break;
                case "Post":
                case "Put":
                case "Patch":
                case "Delete":
                    await DeleteCacheAsync(template);
                    break;
                default:
                    break;
            }
        }

        private async Task DeleteCacheAsync(MixPageContentViewModel data)
        {
            await cacheService.RemoveCacheAsync(data.Id.ToString(), typeof(PageContentViewModel));
        }
    }
}
