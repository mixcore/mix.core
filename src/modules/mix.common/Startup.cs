using Mix.Database.Entities.Account;
using Mix.Lib.Services;
using System.Reflection;

namespace Mix.Common
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IHostApplicationBuilder builder)
        {
            MixFileHelper.CopyFolder(MixFolders.MixCoreConfigurationFolder, MixFolders.MixContentFolder);

            builder.AddMixServices(Assembly.GetExecutingAssembly());

            // Must app Auth config after Add mixservice to init App config 
            builder.Services.AddMixAuthorize<MixCmsAccountContext>(Configuration);
            builder.Services.AddScoped<MixIdentityService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMixApps(Assembly.GetExecutingAssembly(), Configuration, !env.IsProduction());
        }
    }
}
