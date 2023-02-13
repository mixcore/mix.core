using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Mix.Lib;
using Mix.Shared;
using Mix.Shared.Interfaces;
using Mix.SignalR.Services;
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
            services.AddSession();
            services.AddMixCommonServices(executingAssembly, configuration);
            services.AddMixDbContexts();
            services.AddUoWs();
            services.AddMixCache();
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
            services.AddResponseCaching();

            services.TryAddSingleton<AuditLogService>();
            services.TryAddSingleton<MixMemoryCacheService>();
            services.TryAddSingleton<PortalHubClientService>();

            services.AddMixRepoDb();
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
            services.AddMixCache();
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
            services.AddResponseCompression(options => options.EnableForHttps = true);
            services.AddResponseCaching();

            services.TryAddSingleton<AuditLogService>();
            services.TryAddSingleton<MixMemoryCacheService>();
            services.TryAddSingleton<PortalHubClientService>();
            services.AddMixRepoDb();
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
            app.UseSession();
            app.UseResponseCompression();
            app.UseMixResponseCaching();
            app.UseMixTenant();
            app.UseMixStaticFiles(contentRootPath);
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

        private static IApplicationBuilder UseMixStaticFiles(this IApplicationBuilder app, string contentRootPath)
        {
            app.UseDefaultFiles();
            var provider = new FileExtensionContentTypeProvider();
            provider.Mappings[".vue"] = "application/text";

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
