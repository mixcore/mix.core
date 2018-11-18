using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

[assembly: HostingStartup(typeof(Mix.Cms.Hub.SignalRStartup))]
namespace Mix.Cms.Hub
{
    public class SignalRStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            Console.Write(builder);
            //MixChatServiceContext context = new MixChatServiceContext();
            //context.Database.Migrate();
        }
    }
}
