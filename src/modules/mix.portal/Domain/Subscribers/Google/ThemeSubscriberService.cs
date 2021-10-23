using Microsoft.Extensions.Configuration;
using Mix.Lib.Subscribers;
using Mix.Lib.ViewModels;
using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using System;
using System.Threading.Tasks;

namespace Mix.Portal.Domain.Subscribers.Google
{
    public class ThemeSubscriberService : SubscriberService<MixThemeViewModel>
    {
        public ThemeSubscriberService(
            IConfiguration configuration,
            IQueueService<QueueMessageModel> queueService) : base("portal", configuration, queueService)
        {
        }

        public override Task Handler(QueueMessageModel data)
        {
            var post = data.Model.ToObject<MixThemeViewModel>();
            Console.WriteLine(post);
            return Task.CompletedTask;
        }
    }
}
