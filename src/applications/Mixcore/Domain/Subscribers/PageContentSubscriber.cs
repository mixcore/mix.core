using Mix.Queue.Engines;
using Mix.Queue.Engines.MixQueue;

namespace Mixcore.Domain.Subscribers
{
    public sealed class PageContentSubscriber : SubscriberBase
    {
        private static readonly string TopicId = typeof(MixPageContentViewModel).FullName;
        private readonly MixCacheService _cacheService;

        public PageContentSubscriber(
            IConfiguration configuration,
            MixMemoryMessageQueue<MessageQueueModel> queueService,
            MixCacheService cacheService)
            : base(TopicId, MixModuleNames.Mixcore, configuration, queueService)
        {
            _cacheService = cacheService;
        }

        public override async Task Handler(MessageQueueModel data)
        {
            var template = data.ParseData<MixPageContentViewModel>();
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
            await _cacheService.RemoveCacheAsync(data.Id.ToString(), typeof(PageContentViewModel));
        }
    }
}
