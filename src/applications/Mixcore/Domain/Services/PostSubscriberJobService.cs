using Microsoft.Extensions.Configuration;
using Mix.Lib.Models;
using Mix.Lib.Subscribers.Google;
using Mix.Portal.Domain.ViewModels;
using System;
using System.Threading.Tasks;

namespace Mixcore.Domain.Services
{
    public class PostSubscriberJobService : GoogleSubscriberJobService<MixPostContentViewModel>
    {
        public PostSubscriberJobService(
            IConfiguration configuration) : base(configuration)
        {
        }

        public override Task Handler(QueueMessageModel data)
        {
            var post = data.Model.ToObject<MixPostContentViewModel>();
            Console.WriteLine(post);
            return Task.CompletedTask;
        }
    }
}
