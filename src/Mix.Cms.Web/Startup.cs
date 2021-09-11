using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mix.Cms.Api.RestFul;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Extensions;
using Mix.Cms.Lib.MixDatabase.Extensions;
using Mix.Cms.Lib.Models;
using Mix.Cms.Lib.Models.Account;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Cms.Messenger.Models.Data;
using Mix.Cms.Schedule;
using Mix.Infrastructure.Repositories;
using Mix.Rest.Api.Client;
using Mix.Services;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Mix.Cms.Web
{
    public partial class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private readonly string MixcoreAllowSpecificOrigins = "_mixcoreAllowSpecificOrigins";
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string[] allowedHosts = MixService.GetAppSetting<JArray>(MixAppSettingKeywords.AllowedHosts)
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

            services.AddResponseCompression();

            services.AddControllersWithViews()
                .AddRazorRuntimeCompilation()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                });

            #region Additionals Config for Mixcore Cms
            services.AddSingleton<TranslationTransformer>();
            services.AddHttpClient();
            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue; // In case of multipart
            });
            services.AddSingleton<HttpService>();
            services.Configure<GzipCompressionProviderOptions>(
                options => options.Level = System.IO.Compression.CompressionLevel.Fastest);
            services.AddResponseCompression(options => options.EnableForHttps = true);
            /* Additional Config for Mixcore Cms  */

            /* Mix: Add db contexts */
            services.AddDbContext<MixCmsContext>();
            services.AddDbContext<AuditContext>();
            services.AddDbContext<MixDbContext>();
            services.AddDbContext<MixChatServiceContext>();
            /* Mix: End Add db contexts */

            /* Mix: Inject Services */
            services.AddControllers(mvcOptions => mvcOptions.EnableEndpointRouting = false);
            services.AddRepositories();
            services.AddRestClientServices();
            services.AddGenerateApis();
            services.AddMixRestApi();
            services.AddMixDbRepository();
            services.AddMixSignalR();
            //services.AddMixGprc();
            services.AddMixScheduler(Configuration);

            services.AddScoped<InitCmsService>();
            services.AddSingleton<MixCacheService>();
            services.AddMixAuthorize<MixDbContext>(MixService.Instance.MixAuthentications);
            services.AddScoped<MixIdentityService>();
            /* Mix: End Inject Services */
            VerifyInitData(services);
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            if (!MixService.GetAppSetting<bool>(MixAppSettingKeywords.IsInit))
            {
                var context = MixService.GetDbContext();
                var pendingMigration = context.Database.GetPendingMigrations();
                if (pendingMigration.Count() > 0)
                {
                    context.Database.Migrate();
                }
            }

            app.UseResponseCompression();

            app.UseCors(MixcoreAllowSpecificOrigins);

            var provider = new FileExtensionContentTypeProvider();
            // Add new mappings
            app.UseDefaultFiles();
            provider.Mappings[".vue"] = "application/text";
            app.UseStaticFiles(new StaticFileOptions
            {
                ContentTypeProvider = provider
            });

            //app.UseGraphiQl("/api/graphql");

            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            #region Additionals Config for Mixcore Cms

            if (MixService.GetAppSetting<bool>("IsHttps"))
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

            if (!MixService.GetAppSetting<bool>(MixAppSettingKeywords.IsInit))
            {
                using (var ctx = MixService.GetDbContext())
                {
                    ctx.Database.Migrate();
                    var transaction = ctx.Database.BeginTransaction();
                    var sysDatabasesFile = MixFileRepository.Instance.GetFile("sys_databases", MixFileExtensions.Json, $"{MixFolders.DataFolder}");
                    var sysDatabases = JObject.Parse(sysDatabasesFile.Content)["data"].ToObject<List<Lib.ViewModels.MixDatabases.ImportViewModel>>();
                    foreach (var db in sysDatabases)
                    {
                        if (!ctx.MixDatabase.Any(m => m.Name == db.Name))
                        {
                            db.Id = 0;
                            db.SaveModel(true, ctx, transaction);
                        }
                    }
                    transaction.Commit();
                    transaction.Dispose();
                }
                using (var cacheCtx = MixCacheService.GetCacheDbContext())
                {
                    cacheCtx.Database.Migrate();
                }
                var serviceProvider = services.BuildServiceProvider();
                var _roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();
                InitCmsService.InitRolesAsync(_roleManager).GetAwaiter();
            }

            // Mix: Check if require ssl
            if (MixService.GetAppSetting<bool>("IsHttps"))
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