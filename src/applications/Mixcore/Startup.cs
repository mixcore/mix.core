using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Mix.Database.Entities.Account;
using Mix.Lib.Middlewares;
using Mix.Log.Lib;
using Mix.Shared.Models.Configurations;
using Mix.Shared.Services;
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
            services.AddMixLog(Configuration);
            services.AddMixAuthorize<MixCmsAccountContext>(Configuration);

            services.TryAddScoped<MixcorePostService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (!env.IsDevelopment())
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseMixTenant();

            app.UseMiddleware<AuditlogMiddleware>();

            app.UseRouting();

            // Typically, UseStaticFiles is called before UseCors. Apps that use JavaScript to retrieve static files cross site must call UseCors before UseStaticFiles.
            app.UseMixStaticFiles(env.ContentRootPath);
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(env.ContentRootPath, MixFolders.TemplatesFolder))
            });

            // UseCors must be placed after UseRouting and before UseAuthorization. This is to ensure that CORS headers are included in the response for both authorized and unauthorized calls.
            app.UseMixCors();

            // must go between app.UseRouting() and app.UseEndpoints.
            app.UseMixAuth();

            app.UseMixApps(Assembly.GetExecutingAssembly(), Configuration, env.ContentRootPath, env.IsDevelopment());

            app.UseResponseCompression();
            app.UseMixResponseCaching();

            

            if (GlobalConfigService.Instance.AppSettings.IsHttps)
            {
                app.UseHttpsRedirection();
            }


        }
    }
}
