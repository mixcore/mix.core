using Mix.Lib.Subscribers;
using Mix.Lib.ViewModels;
using Mix.Queue.Engines.MixQueue;
using Mix.Queue.Models;
using Mix.Shared.Constants;
using System.Text.Json;

namespace Mixcore.Domain.Subscribers
{
    public class TemplateSubscriberService : SubscriberService
    {
        static string topicId = typeof(MixTemplateViewModel).FullName;
        public TemplateSubscriberService(
            IConfiguration configuration,
            MixMemoryMessageQueue<MessageQueueModel> queueService) : base(topicId, MixModuleNames.Mixcore, configuration, queueService)
        {
        }

        public override Task Handler(MessageQueueModel data)
        {
            var post = data.Model.ToObject<MixTemplateViewModel>();
            Console.WriteLine($"{data.Action} - {post.DisplayName}");
            Console.WriteLine(JsonSerializer.Serialize(post));
            return Task.CompletedTask;
        }
    }
}
