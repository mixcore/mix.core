using Mix.Queue.Engines;
using Mix.Queue.Engines.MixQueue;

namespace Mixcore.Domain.Subscribers
{
    public sealed class ThemeSubscriber : SubscriberBase
    {
        static string topicId = typeof(MixThemeViewModel).FullName;
        public ThemeSubscriber(
            IConfiguration configuration,
            MixMemoryMessageQueue<MessageQueueModel> queueService) : base(topicId, MixModuleNames.Mixcore, configuration, queueService)
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
