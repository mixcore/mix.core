using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Mix.Lib.Models;
using Mix.Lib.ViewModels;
using Mix.Queue.Interfaces;

namespace Mix.Lib.Publishers.Google
{
    public class ThemePublisherService : GooglePublisherService<MixThemeViewModel>
    {
        public ThemePublisherService(
            IQueueService<QueueMessageModel> queueService, 
            IConfiguration configuration, IWebHostEnvironment environment) 
            : base(queueService, configuration, environment)
        {
        }
    }
}
