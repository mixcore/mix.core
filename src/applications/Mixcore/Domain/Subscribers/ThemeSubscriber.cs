using Mix.Queue.Engines;
using Mix.Queue.Engines.MixQueue;

namespace Mixcore.Domain.Subscribers
{
    public sealed class ThemeSubscriber : SubscriberBase
    {
        private static readonly string TopicId = typeof(MixThemeViewModel).FullName;
        public ThemeSubscriber(
            IServiceProvider serviceProvider,
            IConfiguration configuration,
            MixMemoryMessageQueue<MessageQueueModel> queueService) 
            : base(TopicId, MixModuleNames.Mixcore, serviceProvider, configuration, queueService)
        {
        }

        public override Task Handler(MessageQueueModel data)
        {
            var post = data.ParseData<MixThemeViewModel>();
            Console.WriteLine($"{post.DisplayName} -  from {MixModuleNames.Mixcore}");
            return Task.CompletedTask;
        }
    }
}
