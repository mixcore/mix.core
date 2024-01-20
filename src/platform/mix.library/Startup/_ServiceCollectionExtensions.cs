using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Azure.Amqp.Framing;
using Microsoft.Build.Framework;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Mix.Lib.Interfaces;
using Mix.Lib.Middlewares;
using Mix.Lib.Services;
using Mix.Mixdb.Event.Services;
using Mix.Service.Interfaces;
using Mix.Shared;
using Mix.Shared.Interfaces;
using Mix.Shared.Models.Configurations;
using Mix.SignalR.Interfaces;
using StackExchange.Redis;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static List<Assembly> MixAssemblies
        {
            get => MixAssemblyFinder.GetAssembliesByPrefix("mix");
        }

        #region Services

        public static IServiceCollection AddMixServices(this IServiceCollection services, Assembly executingAssembly, IConfiguration configuration)
        {
            var globalConfig = configuration.GetSection(MixAppSettingsSection.GlobalSettings)
                                            .Get<GlobalSettingsModel>();
            
            var authConfig = configuration.GetSection(MixAppSettingsSection.Authentication)
                                            .Get<MixAuthenticationConfigurations>();
            
            var redisCnn = configuration.GetSection("Redis").GetValue<string>("ConnectionString");
            services.Configure<HostOptions>(options =>
            {
                options.ServicesStartConcurrently = true;
                options.ServicesStopConcurrently = false;
            });
            services.AddOptions<GlobalSettingsModel>()
                 .Bind(configuration.GetSection(MixAppSettingsSection.GlobalSettings))
                 .ValidateDataAnnotations();
            services.AddMvc().AddSessionStateTempDataProvider();

            if (!string.IsNullOrEmpty(redisCnn))
            {
                var redis = ConnectionMultiplexer.Connect(configuration.GetSection("Redis").GetValue<string>("ConnectionString"));
                services.AddDataProtection()
                    .SetApplicationName(authConfig.Issuer)
                    .PersistKeysToStackExchangeRedis(redis, "DataProtection-Keys");

                var sp = services.BuildServiceProvider();

                // perform a protect operation to force the system to put at least
                // one key in the key ring
                sp.GetDataProtector("Sample.KeyManager.v1").Protect("payload");
                Console.WriteLine("Performed a protect operation.");
                Thread.Sleep(2000);
            }
            else
            {
                services.AddDataProtection()
                .UnprotectKeysWithAnyCertificate()
                .SetApplicationName(authConfig.Issuer);
            }

            
            services.AddMixCommonServices(configuration);
            services.TryAddScoped<MixConfigurationService>();
            services.TryAddScoped<IMixCmsService, MixCmsService>();

            services.AddMixDbContexts();
            services.AddUoWs();
            services.CustomValidationResponse();
            services.AddHttpClient();
            services.AddHttpLogging(opt => opt.CombineLogs = true);

            services.AddQueues(executingAssembly, configuration);

            // Don't need to inject all entity repository by default
            //services.AddEntityRepositories();

            services.AddMixTenant(configuration);
            services.AddGeneratedPublisher();


            services.AddMixModuleServices(configuration);

            services.AddGeneratedRestApi(MixAssemblies);
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
            services.AddMixRepoDb(globalConfig);

            UnitOfWorkMiddleware.AddUnitOfWork<UnitOfWorkInfo<MixCacheDbContext>>();
            return services;
        }

        // Clone Add MixService for unit test
        public static IServiceCollection AddMixTestServices(this IServiceCollection services, Assembly executingAssembly, IConfiguration configuration)
        {
            // Clone Settings from shared folder
            var globalConfig = configuration.GetSection(MixAppSettingsSection.GlobalSettings).Get<GlobalSettingsModel>()!;
            var authConfig = configuration.GetSection(MixAppSettingsSection.Authentication)
                                            .Get<MixAuthenticationConfigurations>(); 
            services.AddMvc().AddSessionStateTempDataProvider();
          

            services.AddMixCommonServices(configuration);
            services.TryAddScoped<MixConfigurationService>();
            services.TryAddScoped<IMixCmsService, MixCmsService>();

            services.AddMixDbContexts();
            services.AddUoWs();
            services.CustomValidationResponse();
            services.AddHttpClient();
            services.AddLogging();

            services.ApplyMigrations(globalConfig);

            services.AddQueues(executingAssembly, configuration);

            services.AddMixTenant(configuration);
            services.AddGeneratedPublisher();


            services.AddMixModuleServices(configuration);

            services.AddGeneratedRestApi(MixAssemblies);
            services.AddMixSwaggerServices(executingAssembly);
            services.AddSSL();



            services.Configure<GzipCompressionProviderOptions>(
                options => options.Level = System.IO.Compression.CompressionLevel.Fastest);
            services.AddResponseCaching();

            services.TryAddSingleton<IMixMemoryCacheService, MixMemoryCacheService>();
            services.TryAddSingleton<IPortalHubClientService, PortalHubClientService>();
            services.AddMixRepoDb(globalConfig);

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
            app.UseUoWs();
            app.UseMixModuleApps(configuration, isDevelop);
            app.UseMixSwaggerApps(isDevelop, executingAssembly);
            app.ConfigureExceptionHandler();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                foreach (var assembly in MixAssemblies)
                {
                    var startupServices = assembly.GetExportedTypes().Where(IsStartupService);
                    foreach (var startup in startupServices)
                    {
                        ConstructorInfo classConstructor = startup.GetConstructor(Array.Empty<Type>());
                        var instance = classConstructor.Invoke(Array.Empty<Type>());
                        startup.GetMethod("UseEndpoints").Invoke(instance, new object[] { endpoints, configuration, isDevelop });
                    }
                }
            });
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
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(contentRootPath, MixFolders.TemplatesFolder))
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