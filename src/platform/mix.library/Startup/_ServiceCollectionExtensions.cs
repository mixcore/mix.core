using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Lib.Extensions;
using Mix.Lib.Interfaces;
using Mix.Lib.Middlewares;
using Mix.Lib.Services;
using Mix.Mixdb.Event.Services;
using Mix.Mixdb.Extensions;
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
        public static List<Assembly> RefAssemblies(string prefix = MixConstants.CONST_PREFIX_ASSEMBLY)
        {
            return MixAssemblyFinder.GetAssembliesByPrefix(prefix);
        }

        #region Services

        public static IHostApplicationBuilder AddMixServices(this IHostApplicationBuilder builder, Assembly executingAssembly, string prefix = "mix")
        {
            var globalConfig = builder
                .Configuration
                .GetSection(MixAppSettingsSection.GlobalSettings)
                .Get<GlobalSettingsModel>();

            var redisConnectionString = builder
                .Configuration
                .GetSection(MixAppSettingsSection.Redis)
                .GetValue<string>(MixAppSettingsSection.ConnectionStrings);

            builder.Services.Configure<HostOptions>(options =>
            {
                options.ServicesStartConcurrently = true;
                options.ServicesStopConcurrently = false;
            });

            builder.Services.AddOptions<GlobalSettingsModel>()
                 .Bind(builder.Configuration.GetSection(MixAppSettingsSection.GlobalSettings))
                 .ValidateDataAnnotations();

            builder.Services.AddMvc().AddSessionStateTempDataProvider();

            if (!string.IsNullOrEmpty(redisConnectionString))
            {
                bool isConnected = false;
                while (!isConnected)
                {
                    try
                    {
                        var redis = ConnectionMultiplexer.Connect(redisConnectionString);
                        builder.Services.AddDataProtection()
                            .SetApplicationName(Assembly.GetExecutingAssembly().FullName)
                            .PersistKeysToStackExchangeRedis(redis, "DataProtection-Keys");

                        var sp = builder.Services.BuildServiceProvider();

                        // perform a protect operation to force the system to put at least
                        // one key in the key ring
                        sp.GetDataProtector("Sample.KeyManager.v1").Protect("payload");
                        Console.WriteLine("Performed a protect operation.");
                        isConnected = true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Cannot create redis client: {ex.Message}, using JSON cache instead");
                        Thread.Sleep(2000);
                    }
                }
            }
            else
            {
                builder.Services.AddDataProtection()
                .UnprotectKeysWithAnyCertificate()
                .SetApplicationName(Assembly.GetExecutingAssembly().FullName);
            }

            builder.AddMixCommonServices();
            builder.Services.TryAddSingleton<MixConfigurationService>();
            builder.Services.TryAddScoped<IMixCmsService, MixCmsService>();
            builder.Services.AddUoWs();
            builder.Services.AddMixDbContexts();
            builder.Services.CustomValidationResponse();
            builder.Services.AddHttpClient();
            builder.Services.AddHttpLogging(opt => opt.CombineLogs = true);

            builder.Services.AddQueues(executingAssembly, builder.Configuration);

            // Don't need to inject all entity repository by default
            //builder.Services.AddEntityRepositories();

            builder.Services.AddMixTenant(builder.Configuration);
            builder.Services.AddGeneratedPublisher();

            builder.AddIStartupServices(prefix);

            builder.Services.AddGeneratedRestApi(RefAssemblies("mix"));
            builder.Services.AddMixSwaggerServices(executingAssembly);
            builder.AddSSL();

            builder.Services.Configure<GzipCompressionProviderOptions>(
                options => options.Level = System.IO.Compression.CompressionLevel.Fastest);
            builder.Services.AddResponseCompression(options => options.EnableForHttps = true);
            builder.AddMixResponseCaching();
            builder.Services.TryAddSingleton<IMixMemoryCacheService, MixMemoryCacheService>();
            builder.Services.TryAddSingleton<IPortalHubClientService, PortalHubClientService>();
            builder.Services.TryAddSingleton<IMixDbCommandHubClientService, MixDbCommandHubClientService>();
            builder.Services.TryAddSingleton<MixDbEventService>();
            builder.Services.AddMixRepoDb(globalConfig);

            UnitOfWorkMiddleware.AddUnitOfWork<UnitOfWorkInfo<MixCacheDbContext>>();

            return builder;
        }

        // Clone Add MixService for unit test
        public static IHostApplicationBuilder AddMixTestServices(this IHostApplicationBuilder builder, Assembly executingAssembly, string prefix = "mix")
        {
            // Clone Settings from shared folder
            var globalConfig = builder.Configuration.GetSection(MixAppSettingsSection.GlobalSettings).Get<GlobalSettingsModel>()!;
            builder.Services.AddMvc().AddSessionStateTempDataProvider();

            builder.AddMixCommonServices();
            builder.Services.TryAddSingleton<MixConfigurationService>();
            builder.Services.TryAddScoped<IMixCmsService, MixCmsService>();

            builder.Services.AddMixDbContexts();
            builder.Services.AddUoWs();
            builder.Services.CustomValidationResponse();
            builder.Services.AddHttpClient();
            builder.Services.AddLogging();

            builder.Services.ApplyMigrations(builder.Configuration, globalConfig);

            builder.Services.AddQueues(executingAssembly, builder.Configuration);

            builder.Services.AddMixTenant(builder.Configuration);
            builder.Services.AddGeneratedPublisher();


            builder.AddIStartupServices(prefix);

            builder.Services.AddGeneratedRestApi(RefAssemblies("mix"));
            builder.Services.AddMixSwaggerServices(executingAssembly);
            builder.AddSSL();

            builder.Services.Configure<GzipCompressionProviderOptions>(
                options => options.Level = System.IO.Compression.CompressionLevel.Fastest);
            builder.Services.AddResponseCaching();

            builder.Services.TryAddSingleton<IMixMemoryCacheService, MixMemoryCacheService>();
            builder.Services.TryAddSingleton<IPortalHubClientService, PortalHubClientService>();
            builder.Services.AddMixRepoDb(globalConfig);

            UnitOfWorkMiddleware.AddUnitOfWork<UnitOfWorkInfo<MixCacheDbContext>>();
            return builder;
        }

        #endregion

        #region Apps

        public static IApplicationBuilder UseMixApps(
            this IApplicationBuilder app,
            Assembly executingAssembly,
            IConfiguration configuration,
            bool isDevelop,
            string prefix = "mix")
        {
            app.UseHttpLogging();
            app.UseUoWs();
            app.UseIStartupApps(prefix, configuration, isDevelop);
            app.UseMixSwaggerApps(isDevelop, executingAssembly);
            app.ConfigureExceptionHandler();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                foreach (var assembly in RefAssemblies(prefix))
                {
                    var startupServices = assembly.GetExportedTypes().Where(IsStartupService);
                    foreach (var startup in startupServices)
                    {
                        ConstructorInfo classConstructor = startup.GetConstructor([]);
                        var instance = classConstructor.Invoke([]);
                        startup.GetMethod(nameof(IStartupService.UseEndpoints)).Invoke(instance, [endpoints, configuration, isDevelop]);
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
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                ContentTypeProvider = provider
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(contentRootPath, MixFolders.TemplatesFolder))
            });
            return app;
        }

        #endregion

        #region Services


        private static IHostApplicationBuilder AddSSL(this IHostApplicationBuilder builder)
        {
            if (builder.Configuration.GetGlobalConfiguration<bool>(
                    nameof(AppSettingsModel.IsHttps)))
            {
                builder.Services.AddHttpsRedirection(options =>
                {
                    options.RedirectStatusCode = StatusCodes.Status308PermanentRedirect;
                    options.HttpsPort = 443;
                });
            }
            return builder;
        }

        #endregion

        private static List<Type> GetCandidatesByAttributeType(List<Assembly> assemblies, Type attributeType)
        {
            List<Type> types = [];
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
            List<Type> types = [];
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