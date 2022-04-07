using Mix.Lib.Subscribers;
using Mix.Queue.Engines.MixQueue;

namespace Mixcore.Domain.Subscribers
{
    public class ThemeSubscriber : SubscriberBase
    {
        static string topicId = typeof(MixThemeViewModel).FullName;
        public ThemeSubscriber(
            IConfiguration configuration,
            MixMemoryMessageQueue<MessageQueueModel> queueService) : base(topicId, MixModuleNames.Mixcore, configuration, queueService)
        {
        }

        public override Task Handler(MessageQueueModel data)
        {
            var post = data.Data.ToObject<MixThemeViewModel>();
            Console.WriteLine($"{post.DisplayName} -  from {MixModuleNames.Mixcore}");
            return Task.CompletedTask;
        }
    }
}
