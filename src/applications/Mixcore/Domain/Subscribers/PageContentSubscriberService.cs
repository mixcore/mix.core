using Mix.Lib.Subscribers;
using Mix.Queue.Engines.MixQueue;

namespace Mixcore.Domain.Subscribers
{
    public class PageContentSubscriberService : SubscriberService
    {
        static string topicId = typeof(MixPageContentViewModel).FullName;
        private readonly ILogger<PageContentSubscriberService> logger;
        private readonly MixCacheService cacheService;

        public PageContentSubscriberService(
            ILogger<PageContentSubscriberService> logger,
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
            var template = data.Model.ToObject<MixPageContentViewModel>();
            switch (data.Action)
            {
                case MixRestAction.Get:
                    break;
                case MixRestAction.Post:
                case MixRestAction.Put:
                case MixRestAction.Patch:
                case MixRestAction.Delete:
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
