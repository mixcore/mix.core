using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Mix.Cms.Lib;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Services;
using Mix.Heart.Helpers;
using System;
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
                var aesKey = AesEncryptionHelper.GenerateCombinedKeys(256);
                MixService.SetConfig<string>(MixAppSettingKeywords.ApiEncryptKey, aesKey);
                MixService.SetAuthConfig(MixAuthConfigurations.SecretKey, Guid.NewGuid().ToString("N"));
                MixService.SaveSettings();
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