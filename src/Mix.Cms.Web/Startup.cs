using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mix.Cms.Api.RestFul;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Extensions;
using Mix.Cms.Lib.Models.Account;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Cms.Messenger.Models.Data;
using Mix.Cms.Schedule;
using Mix.Cms.Schedule.Jobs;
using Mix.Cms.Service.SignalR;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mix.Cms.Web
{
    public partial class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        readonly string MixcoreAllowSpecificOrigins = "_mixcoreAllowSpecificOrigins";
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string[] allowedHosts = MixService.GetConfig<JArray>(MixAppSettingKeywords.AllowedHosts)
                                        .Select(m => m.Value<string>("text")).ToArray();
            services.AddCors(options =>
            {
                options.AddPolicy(name: MixcoreAllowSpecificOrigins,
                                  builder =>
                                  {
                                      builder.WithOrigins(allowedHosts);
                                      builder.AllowAnyHeader();
                                      builder.AllowAnyMethod();
                                  });
            });

            services.AddControllersWithViews()
                .AddRazorRuntimeCompilation()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                });

            #region Additionals Config for Mixcore Cms

            /* Additional Config for Mixcore Cms  */

            /* Mix: Add db contexts */
            services.AddDbContext<MixCmsContext>();
            services.AddDbContext<MixDbContext>();
            services.AddDbContext<MixChatServiceContext>();
            /* Mix: End Add db contexts */

            /* Mix: Inject Services */
            services.AddControllers(mvcOptions => mvcOptions.EnableEndpointRouting = false);
            services.AddGenerateApis();
            services.AddMixRestApi();
            services.AddMixSignalR();
            //services.AddMixGprc();
            services.AddMixScheduler(Configuration);
            /* Mix: End Inject Services */

            VerifyInitData(services);

            services.AddMixAuthorize(Configuration);
            /* End Additional Config for Mixcore Cms  */

            #endregion Additionals Config for Mixcore Cms

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
            if (!MixService.GetConfig<bool>("IsInit"))
            {
                var context = MixService.GetDbContext();
                var pendingMigration = context.Database.GetPendingMigrations();
                if (pendingMigration.Count() > 0)
                {
                    context.Database.Migrate();
                }
            }

            app.UseCors(MixcoreAllowSpecificOrigins);

            var provider = new FileExtensionContentTypeProvider();
            // Add new mappings
            provider.Mappings[".vue"] = "application/text";

            app.UseStaticFiles(new StaticFileOptions
            {
                ContentTypeProvider = provider
            });

            //app.UseStaticFiles();
            //app.UseGraphiQl("/api/graphql");

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            #region Additionals Config for Mixcore Cms

            if (MixService.GetConfig<bool>("IsHttps"))
            {
                app.UseHttpsRedirection();
            }
            app.UseMixRestApi();
            //app.UseMixGprc();

            app.UseMixSignalR();

            app.UseMixRoutes();
            app.UseMixScheduler();
            #endregion Additionals Config for Mixcore Cms

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