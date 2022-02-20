using Mix.Lib.Subscribers;
using Mix.Lib.ViewModels;
using Mix.Queue.Engines.MixQueue;

namespace Mixcore.Domain.Subscribers
{
    public class ThemeSubscriberService : SubscriberService
    {
        static string topicId = typeof(MixThemeViewModel).FullName;
        public ThemeSubscriberService(
            IConfiguration configuration,
            MixMemoryMessageQueue<MessageQueueModel> queueService) : base(topicId, MixModuleNames.Mixcore, configuration, queueService)
        {
        }

        public override Task Handler(MessageQueueModel data)
        {
            var post = data.Model.ToObject<MixThemeViewModel>();
            Console.WriteLine($"{post.DisplayName} -  from {MixModuleNames.Mixcore}");
            return Task.CompletedTask;
        }
    }
}
