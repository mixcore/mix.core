using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Mix.Cms.Lib;
using System.IO;

namespace Mix.Cms.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            if (!File.Exists($"{MixConstants.CONST_FILE_APPSETTING}.json"))
            {
                File.Copy($"{MixConstants.CONST_DEFAULT_FILE_APPSETTING}.json", $"{MixConstants.CONST_FILE_APPSETTING}.json");
            }
            var config = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile(MixConstants.CONST_FILE_APPSETTING, optional: true, reloadOnChange: true)
           .Build();
            return WebHost.CreateDefaultBuilder(args)
                .UseConfiguration(config)
                .UseSetting("https_port", "443")
                .UseStartup<Startup>().UseKestrel(options =>
                {
                    options.Limits.MaxRequestBodySize = 209715200;
                });
        }
    }
}
