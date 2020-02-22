using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Mix.Cms.Lib;
using System.IO;

namespace Mix.Cms.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            if (!File.Exists($"{MixConstants.CONST_FILE_APPSETTING}"))
            {
                File.Copy($"{MixConstants.CONST_DEFAULT_FILE_APPSETTING}", $"{MixConstants.CONST_FILE_APPSETTING}");
            }
            var config = new ConfigurationBuilder()
          .SetBasePath(Directory.GetCurrentDirectory())
          .AddJsonFile(MixConstants.CONST_FILE_APPSETTING, optional: true, reloadOnChange: true)
          .Build();
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        }
    }
}