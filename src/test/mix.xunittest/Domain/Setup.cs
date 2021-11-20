using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mix.Database.Entities.Account;
using Mix.Lib.Helpers;
using Mix.Shared.Constants;
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
                MixCmsHelper.CopyFolder("../../../../../shared/MixContent", MixFolders.ConfiguratoinFolder);
            }
            ConfigureAppConfiguration((hostingContext, config) =>
            {
                bool reloadOnChange =
                    hostingContext.Configuration.GetValue("hostBuilder:reloadConfigOnChange", true);

                if (hostingContext.HostingEnvironment.IsDevelopment())
                    config.AddUserSecrets<Setup>(true, reloadOnChange);
            });

            ConfigureServices((context, services) =>
            {
                services.AddMixTestServices(Assembly.GetExecutingAssembly(), Configuration);
                services.AddMixAuthorize<ApplicationDbContext>();
            });
        }
    }
}
