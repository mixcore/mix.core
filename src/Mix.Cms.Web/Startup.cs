using GraphiQl;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mix.Cms.Api.GraphQL;
using Mix.Cms.Lib.Models.Account;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;

namespace Mix.Cms.Web
{
    public partial class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews()
                .AddRazorRuntimeCompilation()
                .AddNewtonsoftJson();

            #region Addictionals Config for Mixcore Cms

            /* Addictional Config for Mixcore Cms  */

            /* Mix: Add db contexts */
            services.AddDbContext<MixCmsContext>();
            services.AddDbContext<MixDbContext>();
            /* Mix: End Add db contexts */

            /* Mix: Inject Services */
            services.AddSingleton<MixService>();
            services.AddSignalR();
            services.AddControllers(mvcOptions =>
               mvcOptions.EnableEndpointRouting = false);

            services.AddOData();
            services.AddMyGraphQL();

            /* Mix: End Inject Services */

            VerifyInitData(services);

            ConfigAuthorization(services, Configuration);

            /* End Addictional Config for Mixcore Cms  */

            #endregion Addictionals Config for Mixcore Cms
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseStaticFiles();
            app.UseGraphiQl("/api/graphql");
            
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            #region Addictionals Config for Mixcore Cms

            if (MixService.GetConfig<bool>("IsHttps"))
            {
                app.UseHttpsRedirection();
            }

            ConfigRoutes(app);

            #endregion Addictionals Config for Mixcore Cms
        }

        // Mix: Check custom cms config
        private void VerifyInitData(IServiceCollection services)
        {
            // Mix: Migrate db if already inited

            if (!MixService.GetConfig<bool>("IsInit"))
            {
                using (var ctx = new MixCmsContext())
                {
                    ctx.Database.Migrate();
                }
            }

            // Mix: Check if require ssl
            if (MixService.GetConfig<bool>("IsHttps"))
            {
                services.AddHttpsRedirection(options =>
                {
                    options.RedirectStatusCode = StatusCodes.Status308PermanentRedirect;
                    options.HttpsPort = 443;
                });
            }
        }
    }
}