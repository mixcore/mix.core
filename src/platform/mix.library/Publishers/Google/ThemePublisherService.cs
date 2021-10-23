using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Mix.Lib.ViewModels;
using Mix.Queue.Interfaces;
using Mix.Queue.Models;

namespace Mix.Lib.Publishers.Google
{
    public class ThemePublisherService : MixPublisherService<MixThemeViewModel>
    {
        public ThemePublisherService(
            IQueueService<QueueMessageModel> queueService, 
            IConfiguration configuration, IWebHostEnvironment environment) 
            : base(queueService, configuration, environment)
        {
        }
    }
}
