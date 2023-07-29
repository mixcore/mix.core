using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Mix.Lib.Middlewares;
using Mix.Mixdb.Event.Services;
using Mix.Service.Interfaces;
using Mix.Shared;
using Mix.Shared.Interfaces;
using Mix.SignalR.Interfaces;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static List<Assembly> MixAssemblies
        {
            get => MixAssemblyFinder.GetMixAssemblies();
        }

        #region Services

        public static IServiceCollection AddMixServices(this IServiceCollection services, Assembly executingAssembly, IConfiguration configuration)
        {
            services.AddMvc().AddSessionStateTempDataProvider();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(4);
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.Strict;
                options.Cookie.HttpOnly = true;
                // Make the session cookie essential if you wish
                //options.Cookie.IsEssential = true;
            });
                services.AddMixCommonServices(executingAssembly, configuration);
            services.AddMixDbContexts();
            services.AddUoWs();
            services.AddMixCache(configuration);
            services.CustomValidationResponse();
            services.AddHttpClient();
            services.AddLogging();

            services.ApplyMigrations();

            services.AddQueues(executingAssembly, configuration);

            // Don't need to inject all entity repository by default
            //services.AddEntityRepositories();

            services.AddMixTenant();
            services.AddGeneratedPublisher();


            services.AddMixModuleServices(configuration);

            services.AddGeneratedRestApi();
            services.AddMixSwaggerServices(executingAssembly);
            services.AddSSL();

            services.Configure<GzipCompressionProviderOptions>(
                options => options.Level = System.IO.Compression.CompressionLevel.Fastest);
            services.AddResponseCompression(options => options.EnableForHttps = true);
            services.AddMixResponseCaching();
            services.TryAddSingleton<IMixMemoryCacheService, MixMemoryCacheService>();
            services.TryAddSingleton<IPortalHubClientService, PortalHubClientService>();
            services.TryAddSingleton<IMixDbCommandHubClientService, MixDbCommandHubClientService>();
            services.TryAddSingleton<MixDbEventService>();
            services.AddMixRepoDb();

            UnitOfWorkMiddleware.AddUnitOfWork<UnitOfWorkInfo<MixCacheDbContext>>();
            return services;
        }

        // Clone Add MixService for unit test
        public static IServiceCollection AddMixTestServices(this IServiceCollection services, Assembly executingAssembly, IConfiguration configuration)
        {
            // Clone Settings from shared folder
            services.AddMvc().AddSessionStateTempDataProvider();
            services.AddSession();
            services.AddMixCommonServices(executingAssembly, configuration);
            services.AddMixDbContexts();
            services.AddUoWs();
            services.AddMixCache(configuration);
            services.CustomValidationResponse();
            services.AddHttpClient();
            services.AddLogging();

            services.ApplyMigrations();

            services.AddQueues(executingAssembly, configuration);

            services.AddMixTenant();
            services.AddGeneratedPublisher();


            services.AddMixModuleServices(configuration);

            services.AddGeneratedRestApi();
            services.AddMixSwaggerServices(executingAssembly);
            services.AddSSL();



            services.Configure<GzipCompressionProviderOptions>(
                options => options.Level = System.IO.Compression.CompressionLevel.Fastest);
            services.AddResponseCaching();

            services.TryAddSingleton<IMixMemoryCacheService, MixMemoryCacheService>();
            services.TryAddSingleton<IPortalHubClientService, PortalHubClientService>();
            services.AddMixRepoDb();

            UnitOfWorkMiddleware.AddUnitOfWork<UnitOfWorkInfo<MixCacheDbContext>>();
            return services;
        }

        #endregion

        #region Apps

        public static IApplicationBuilder UseMixApps(
            this IApplicationBuilder app,
            Assembly executingAssembly,
            IConfiguration configuration,
            string contentRootPath,
            bool isDevelop)
        {
            app.UseHttpLogging();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseUoWs();
            app.UseMixModuleApps(configuration, isDevelop);
            app.UseMixSwaggerApps(isDevelop, executingAssembly);
            app.ConfigureExceptionHandler();

            return app;
        }

        #endregion

        #region Helpers

        #region App

        public static IApplicationBuilder UseMixStaticFiles(this IApplicationBuilder app, string contentRootPath)
        {
            app.UseDefaultFiles();
            var provider = new FileExtensionContentTypeProvider();
            provider.Mappings[".vue"] = "application/text";
            provider.Mappings[".xml"] = "application/xml";

            app.UseStaticFiles(new StaticFileOptions
            {
                ContentTypeProvider = provider
            });


            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(contentRootPath, MixFolders.StaticFiles)),
                RequestPath = "/mix-app"
            });

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(contentRootPath, MixFolders.StaticFiles)),
                RequestPath = $"/{MixFolders.StaticFiles}"
            });

            // Use staticfile for wwwroot
            app.UseStaticFiles();

            return app;
        }

        #endregion

        #region Services


        private static IServiceCollection AddSSL(this IServiceCollection services)
        {
            if (GlobalConfigService.Instance.AppSettings.IsHttps)
            {
                services.AddHttpsRedirection(options =>
                {
                    options.RedirectStatusCode = StatusCodes.Status308PermanentRedirect;
                    options.HttpsPort = 443;
                });
            }
            return services;
        }

        #endregion


        private static List<Type> GetCandidatesByAttributeType(List<Assembly> assemblies, Type attributeType)
        {
            List<Type> types = new();
            assemblies.ForEach(
                a => types.AddRange(a.GetExportedTypes()
                        .Where(
                            x => x.GetCustomAttributes(attributeType).Any()
                            )
                        ));
            return types;
        }

        private static bool IsStartupService(Type type)
        {
            var typeInfo = type.GetTypeInfo();
            return
                typeInfo.IsClass &&
                !typeInfo.IsAbstract &&
                !typeInfo.IsGenericType &&
                typeof(IStartupService).IsAssignableFrom(type);
        }


        private static List<Type> GetViewModelCandidates(List<Assembly> assemblies)
        {
            List<Type> types = new();
            assemblies.ForEach(
                a => types.AddRange(a.GetExportedTypes()
                        .Where(
                            x => x.BaseType?.Name == typeof(ViewModelBase<,,,>).Name
                            )
                        ));
            return types;
        }

        #endregion
    }
}