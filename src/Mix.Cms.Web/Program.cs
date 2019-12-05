using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Mix.Cms.Lib;

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
            // Mix: Create default appSettings if not exists
            if (!System.IO.File.Exists(MixConstants.CONST_FILE_APPSETTING))
            {
                System.IO.File.Copy(MixConstants.CONST_DEFAULT_FILE_APPSETTING, MixConstants.CONST_FILE_APPSETTING, false);
            }

            var config = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile(MixConstants.CONST_FILE_APPSETTING, optional: true, reloadOnChange: true)
           .Build();
            return Host.CreateDefaultBuilder(args)          
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseConfiguration(config);
                    webBuilder.UseStartup<Startup>();
                });
        }
    }
}
