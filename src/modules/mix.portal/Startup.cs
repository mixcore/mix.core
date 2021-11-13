using Mix.Database.Entities.Account;
using Mix.Shared.Services;
using System.Reflection;

namespace Mix.Portal
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if (!Directory.Exists(MixFolders.MixCoreConfigurationFolder))
            {
                MixFileService _fileService = new();
                _fileService.CopyFolder(MixFolders.SharedConfigurationFolder, MixFolders.ConfiguratoinFolder);
            }

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin();
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                });
            });
            
            services.AddMixServices(Assembly.GetExecutingAssembly(), Configuration);

            // Must app Auth config after Add mixservice to init App config 
            services.AddMixAuthorize<ApplicationDbContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, GlobalConfigService globalConfigService)
        {
            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin();
                builder.AllowAnyMethod();
                builder.AllowAnyHeader();
                builder.WithExposedHeaders("Grpc-Status", "Grpc-Message");
            });
            app.UseMixApps(Assembly.GetExecutingAssembly(), Configuration, env.IsDevelopment(), globalConfigService);
        }
    }
}
