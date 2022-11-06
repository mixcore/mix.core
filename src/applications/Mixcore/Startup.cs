using Microsoft.Extensions.FileProviders;
using Mix.Database.Entities.Account;
using Mix.Shared.Services;
using Mixcore.Domain.Subscribers;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace Mixcore
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
            services.AddWebEncoders(options =>
            {
                options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All);
            });


            services.AddMixServices(Assembly.GetExecutingAssembly(), Configuration);
            services.AddMixCors();
            services.AddScoped<MixNavigationService>();

            // Queue Subscribers
            services.AddHostedService<ThemeSubscriber>();
            services.AddHostedService<TemplateSubscriber>();
            services.AddHostedService<PageContentSubscriber>();
            services.AddHostedService<TenantSubscriber>();
            services.AddHostedService<DomainSubscriber>();

            services.AddMixAuthorize<MixCmsAccountContext>();
            services.AddMixRoutes();

            // Must app Auth config after Add mixservice to init App config 
            services.AddMixOcelot(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (!env.IsDevelopment())
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseMixCors();
            app.UseMixApps(Assembly.GetExecutingAssembly(), Configuration, env.ContentRootPath, env.IsDevelopment());
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(env.ContentRootPath, MixFolders.TemplatesFolder))
            });

            if (GlobalConfigService.Instance.AppSettings.EnableOcelot)
            {
                app.UseMixOcelot(Configuration, env.IsDevelopment());
            }
            if (GlobalConfigService.Instance.AppSettings.IsHttps)
            {
                app.UseHttpsRedirection();
            }

            app.UseMixRoutes();
        }
    }
}
