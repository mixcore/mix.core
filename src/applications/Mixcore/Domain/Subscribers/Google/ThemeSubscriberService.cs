using Microsoft.Extensions.Configuration;
using Mix.Lib.Models;
using Mix.Lib.Subscribers.Google;
using Mix.Lib.ViewModels;
using System;
using System.Threading.Tasks;

namespace Mixcore.Domain.Subscribers.Google
{
    public class ThemeSubscriberService : GoogleSubscriberService<MixThemeViewModel>
    {
        public ThemeSubscriberService(
            IConfiguration configuration) : base("mixcore", configuration)
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
