using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Mix.Heart.Infrastructure.Repositories;
using Mix.Heart.Infrastructure.ViewModels;
using Mix.Lib.Conventions;
using Mix.Lib.Interfaces;
using Mix.Lib.Providers;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Mix.Heart.Extensions;
using Mix.Lib.Services;
using Microsoft.AspNetCore.StaticFiles;
using Mix.Shared.Constants;
using Mix.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Mix.Shared.Services;
using Mix.Database.Services;
using Mix.Shared.Enums;
using Microsoft.Extensions.Configuration;
using Mix.Identity.Models;

namespace Mix.Lib.Extensions
{
    public static class ServiceCollectionExtensions
    {
        static string MixcoreAllowSpecificOrigins = "_mixcoreAllowSpecificOrigins";

        public static IServiceCollection AddMixServices(this IServiceCollection services, IConfiguration Configuration)
        {
            var mixAuthentications = Configuration.GetSection(MixAppSettingsSection.Authentication.ToString()) as MixAuthenticationConfigurations;
            services.AddSingleton<MixFileService>();
            services.AddSingleton<MixAppSettingService>();
            services.AddSingleton<MixDatabaseService>();
            services.AddScoped<MixService>();
            services.AddResponseCompression();
            //services.AddDbContext<MixDbContext>();
            //services.AddMixAuthorize<MixDbContext>(mixAuthentications);
            services.AddScoped<TranslatorService>();
            services.AddScoped<ConfigurationService>();
            var serviceProvider = services.BuildServiceProvider();
            var appSettingService = serviceProvider.GetService<MixAppSettingService>();
            VerifyInitData(appSettingService);
            var assemblies = GetMixAssemblies();

            foreach (var assembly in assemblies)
            {
                var startupServices = assembly.GetExportedTypes().Where(IsStartupService);
                foreach (var startup in startupServices)
                {
                    ConstructorInfo classConstructor = startup.GetConstructor(Array.Empty<Type>());
                    var instance = classConstructor.Invoke(Array.Empty<Type>());
                    startup.GetMethod("AddServices").Invoke(instance, new object[] { services });

                }
                services.AddGeneratedRestApi(assembly);
                services.AddRepositories(assembly);
            }
            // Mix: Check if require ssl
            if (appSettingService.GetConfig<bool>(MixAppSettingsSection.GlobalSettings, "IsHttps"))
            {
                services.AddHttpsRedirection(options =>
                {
                    options.RedirectStatusCode = StatusCodes.Status308PermanentRedirect;
                    options.HttpsPort = 443;
                });
            }
            return services;
        }

        public static IApplicationBuilder UseMixApps(this IApplicationBuilder app, bool isDevelop)
        {
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
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            if (MixAppSettingService.Instance.GetConfig<bool>(MixAppSettingsSection.GlobalSettings, "IsHttps"))
            {
                app.UseHttpsRedirection();
            }

            var assemblies = GetMixAssemblies();
            foreach (var assembly in assemblies)
            {
                var startupServices = assembly.GetExportedTypes().Where(IsStartupService);
                foreach (var startup in startupServices)
                {
                    ConstructorInfo classConstructor = startup.GetConstructor(Array.Empty<Type>());
                    var instance = classConstructor.Invoke(Array.Empty<Type>());
                    startup.GetMethod("UseApps").Invoke(instance, new object[] { app, isDevelop });
                }
            }
            return app;
        }

        public static void AddMixSwaggerServices(this IServiceCollection services, Assembly assembly)
        {
            string title = assembly.ManifestModule.Name.Replace(".dll", string.Empty);
            string version = "v2";
            string swaggerBasePath = $"api/{version}/{title.Replace(".", "-").ToHypenCase()}";
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(version, new OpenApiInfo { Title = title, Version = version });
            });
        }

        public static void UseMixSwaggerApps(this IApplicationBuilder app, bool isDevelop, Assembly assembly)
        {
            string title = assembly.ManifestModule.Name.Replace(".dll", string.Empty);
            string version = "v2";
            string swaggerBasePath = $"api/{version}/{title.Replace(".", "-").ToHypenCase()}";
            string routePrefix = $"{swaggerBasePath}/swagger";
            string routeTemplate = swaggerBasePath + "/swagger/{documentName}/swagger.json";
            string endPoint = $"/{swaggerBasePath}/swagger/{version}/swagger.json";
            if (isDevelop)
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger(opt => opt.RouteTemplate = routeTemplate);
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint(endPoint, $"{title} {version}");
                    c.RoutePrefix = routePrefix;
                });
            }
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void VerifyInitData(MixAppSettingService appSettingService)
        {
            var mixService = new MixService(appSettingService);
            var mixDatabaseService = new MixDatabaseService(appSettingService);
            mixService.InitAppSettings();

            if (!appSettingService.GetConfig<bool>(MixAppSettingsSection.GlobalSettings, MixAppSettingKeywords.IsInit))
            {
                mixDatabaseService.InitMixCmsContext();
                MixCacheService.InitMixCacheContext();
            }
        }

        private static IServiceCollection AddGeneratedRestApi(this IServiceCollection services, Assembly assembly, Type baseType = null)
        {
            services.
                AddMvc(o => o.Conventions.Add(
                    new GenericControllerRouteConvention()
                )).
                ConfigureApplicationPartManager(m =>
                    m.FeatureProviders.Add(new GenericTypeControllerFeatureProvider(assembly, baseType)
                ));
            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services, Assembly assembly)
        {
            var candidates = assembly
                .GetExportedTypes()
                .Where(m => m.BaseType?.Name == typeof(ViewModelBase<,,>).Name);
            var repositoryType = typeof(DefaultRepository<,,>);
            foreach (var candidate in candidates)
            {
                if (candidate.BaseType.IsGenericType
                    && candidate.BaseType.GenericTypeArguments.Length == repositoryType.GetGenericArguments().Length)
                {
                    Type[] types = candidate.BaseType.GenericTypeArguments;
                    services.AddScoped(
                        repositoryType.MakeGenericType(types)
                    );
                }
            }
            return services;
        }

        internal static bool IsStartupService(Type type)
        {
            var typeInfo = type.GetTypeInfo();
            return
                typeInfo.IsClass &&
                !typeInfo.IsAbstract &&
                !typeInfo.IsGenericType &&
                typeof(IStartupService).IsAssignableFrom(type);
        }


        private static Assembly[] GetMixAssemblies()
        {
            var assemblies = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll")
                                .Where(x => AssemblyName.GetAssemblyName(x).FullName.StartsWith("mix."))
                                .Select(x => Assembly.Load(AssemblyName.GetAssemblyName(x)));
            return assemblies.ToArray();
        }
    }
}
