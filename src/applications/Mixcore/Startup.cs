using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mix.Database.Entities.Account;
using Mix.Database.Services;
using Mix.Identity;
using Mix.Identity.Services;
using Mix.Lib.Extensions;
using Mix.Shared.Enums;
using Mix.Shared.Models;
using Mix.Shared.Services;
using System.Reflection;

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
            services.AddScoped<MixAppSettingService>();
            services.AddScoped<MixDatabaseService>();
            MixAppSettingService appSettingService = new();
            var auth = appSettingService.LoadSection<MixAuthenticationConfigurations>(MixAppSettingsSection.Authentication);
            services.AddDbContext<ApplicationDbContext>();
            services.AddDbContext<MixCmsAccountContext>();
            services.AddMixServices(Configuration);
            services.AddMixAuthorize<ApplicationDbContext>(auth);
            services.AddMixSwaggerServices(Assembly.GetExecutingAssembly());
            services.AddControllersWithViews()
                .AddRazorRuntimeCompilation()
                .AddNewtonsoftJson();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, MixAppSettingService appSettingService)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseMixApps(env.IsDevelopment(), appSettingService);
            app.UseMixSwaggerApps(env.IsDevelopment(), Assembly.GetExecutingAssembly());

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}