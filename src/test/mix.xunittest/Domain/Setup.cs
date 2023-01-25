using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mix.Database.Entities.Account;
using Mix.Lib.Helpers;
using Mix.Shared.Helpers;
using Mix.Tenancy.Domain.Services;
using System.Reflection;

namespace Mix.XUnittest
{
    public class Setup : Xunit.Di.Setup
    {
        public IConfiguration Configuration { get; }
        protected override void Configure()
        {
            if (Directory.Exists(MixFolders.MixContentFolder))
            {
                MixFileHelper.DeleteFolder(MixFolders.MixContentFolder);
            }
            // TODO: To check again as once publish the release version, the path will be invalid
            MixHelper.CopyFolder("../../../../../shared/MixContent", MixFolders.MixContentFolder);
            ConfigureAppConfiguration((hostingContext, config) =>
            {
                config
                      .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                      .AddJsonFile("appsettings.json", true, true)
                      .AddJsonFile($"{MixAppConfigFilePaths.Shared}/AppConfigs/ocelot.json", true, true)
                      .AddJsonFile($"{MixAppConfigFilePaths.Shared}/AppConfigs/queue.json", true, true)
                      .AddJsonFile($"{MixAppConfigFilePaths.Shared}/AppConfigs/mix_heart.json", true, true)
                      .AddJsonFile($"{MixAppConfigFilePaths.Shared}/AppConfigs/authentication.json", true, true);

                bool reloadOnChange =
                    hostingContext.Configuration.GetValue("hostBuilder:reloadConfigOnChange", true);

                if (hostingContext.HostingEnvironment.IsDevelopment())
                    config.AddUserSecrets<Setup>(true, reloadOnChange);
            });

            ConfigureServices((context, services) =>
            {

                services.AddMixDbContexts();
                services.AddScoped<InitCmsService>();
                services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
                services.AddMixTestServices(Assembly.GetExecutingAssembly(), Configuration);
                services.AddMixAuthorize<MixCmsAccountContext>();
            });
        }
    }
}
