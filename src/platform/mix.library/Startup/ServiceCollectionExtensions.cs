using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Mix.Lib.Interfaces;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static List<Assembly> MixAssemblies { get => GetMixAssemblies(); }

        #region Services

        public static IServiceCollection AddMixServices(this IServiceCollection services, Assembly executingAssembly, IConfiguration configuration)
        {
            services.AddMixCommonServices(executingAssembly, configuration);
            services.AddMixDbContexts(executingAssembly, configuration);
            services.AddMixCache(executingAssembly, configuration);

            services.AddHttpClient();
            services.AddLogging();

            services.ApplyMigrations();

            services.AddQueues(executingAssembly, configuration);


            services.AddEntityRepositories();
            services.AddGeneratedPublisher();


            services.AddMixModuleServices(configuration);
            services.AddMixSignalR();

            services.AddGeneratedRestApi();
            services.AddMixSwaggerServices(executingAssembly);
            services.AddSSL();



            services.Configure<GzipCompressionProviderOptions>(
                options => options.Level = System.IO.Compression.CompressionLevel.Fastest);
            services.AddResponseCompression(options => options.EnableForHttps = true);
            services.AddResponseCaching();
            return services;
        }

        // Clone Add MixService for unit test
        public static IServiceCollection AddMixTestServices(this IServiceCollection services, Assembly executingAssembly, IConfiguration configuration)
        {
            // Clone Settings from shared folder
            services.AddMixCommonServices(executingAssembly, configuration);
            services.AddMixDbContexts(executingAssembly, configuration);
            services.AddMixCache(executingAssembly, configuration);

            services.AddHttpClient();
            services.AddLogging();

            services.ApplyMigrations();

            services.AddQueues(executingAssembly, configuration);


            services.AddEntityRepositories();


            services.AddMixModuleServices(configuration);
            services.AddMixSignalR();

            services.AddGeneratedRestApi();
            services.AddMixSwaggerServices(executingAssembly);
            services.AddSSL();

            services.AddResponseCompression();
            services.AddResponseCaching();
            return services;
        }

        #endregion

        #region Apps

        public static IApplicationBuilder UseMixApps(
            this IApplicationBuilder app,
            Assembly executingAssembly,
            IConfiguration configuration,
            bool isDevelop)
        {
            app.UseResponseCompression();
            app.UseMixResponseCaching();

            app.UseMixStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMixModuleApps(configuration, isDevelop);
            app.UseMixSwaggerApps(isDevelop, executingAssembly);
            app.UseMixSignalR();


            return app;
        }

        #endregion

        #region Helpers

        #region App

        private static IApplicationBuilder UseMixStaticFiles(this IApplicationBuilder app)
        {
            var provider = new FileExtensionContentTypeProvider();
            app.UseDefaultFiles();
            provider.Mappings[".vue"] = "application/text";
            app.UseStaticFiles(new StaticFileOptions
            {
                ContentTypeProvider = provider
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

        public static T GetService<T>(this IServiceCollection services)
        {
            var sp = services.BuildServiceProvider();
            return sp.GetService<T>();
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

        private static List<Assembly> GetMixAssemblies()
        {
            var assemblies = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll")
                                .Where(x => AssemblyName.GetAssemblyName(x).FullName.StartsWith("mix"))
                                .Select(x => Assembly.Load(AssemblyName.GetAssemblyName(x)));
            return assemblies.ToList();
        }

        #endregion

    }
}
