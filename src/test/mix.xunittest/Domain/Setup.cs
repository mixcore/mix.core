using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mix.Database.Entities.Account;
using Mix.Lib.Helpers;

using System.Reflection;

namespace Mix.XUnittest
{
    public class Setup : Xunit.Di.Setup
    {
        public IConfiguration Configuration { get; }
        protected override void Configure()
        {
            if (!Directory.Exists(MixFolders.ConfiguratoinFolder))
            {
                MixHelper.CopyFolder("../../../../../shared/MixContent", MixFolders.ConfiguratoinFolder);
            }
            ConfigureAppConfiguration((hostingContext, config) =>
            {
                config
                      .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                      .AddJsonFile("appsettings.json", true, true)
                      .AddJsonFile("MixContent/AppConfigs/ocelot.json", true, true)
                      .AddJsonFile("MixContent/AppConfigs/queue.json", true, true)
                      .AddJsonFile("MixContent/AppConfigs/mix_heart.json", true, true)
                      .AddJsonFile("MixContent/AppConfigs/authentication.json", true, true);

                bool reloadOnChange =
                    hostingContext.Configuration.GetValue("hostBuilder:reloadConfigOnChange", true);

                if (hostingContext.HostingEnvironment.IsDevelopment())
                    config.AddUserSecrets<Setup>(true, reloadOnChange);
            });

            ConfigureServices((context, services) =>
            {
                services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
                services.AddMixTestServices(Assembly.GetExecutingAssembly(), Configuration);
                services.AddMixAuthorize<MixCmsAccountContext>();
            });
        }
    }
}
