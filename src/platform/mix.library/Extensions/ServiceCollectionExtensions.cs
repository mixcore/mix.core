using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Mix.Lib.Conventions;
using Mix.Lib.Interfaces;
using Mix.Lib.Providers;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Mix.Lib.Services;
using Microsoft.AspNetCore.StaticFiles;
using Mix.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Mix.Shared.Services;
using Mix.Database.Services;
using Microsoft.Extensions.Configuration;
using Mix.Heart.Extensions;
using Mix.Database.Entities.Cms;
using Mix.Database.Extenstions;
using Mix.Lib.Filters;
using Mix.Lib.Attributes;
using System.Collections.Generic;
using Mix.Database.Entities.Account;
using Mix.Heart.ViewModel;
using System.Text.Json.Serialization;
using Mix.Heart.UnitOfWork;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mix.Lib.Extensions
{
    public static class ServiceCollectionExtensions
    {
        static readonly string MixcoreAllowSpecificOrigins = "_mixcoreAllowSpecificOrigins";
        static List<Assembly> MixAssemblies { get => GetMixAssemblies(); }

        #region Services

        public static IServiceCollection AddMixServices(this IServiceCollection services, Assembly executingAssembly, IConfiguration configuration)
        {
            // Clone Settings from shared folder
            InitAppSettings();
            services.AddLogging();
            services.AddDbContext<ApplicationDbContext>();
            services.AddDbContext<MixCmsContext>();
            services.AddDbContext<MixCmsAccountContext>();

            services.AddSingleton<MixFileService>();
            services.InitMixContext();
            services.AddEntityRepositories();
            services.AddScoped<MixService>();
            services.AddScoped<TranslatorService>();
            services.AddScoped<MixConfigurationService>();
            services.AddMixModuleServices(configuration);
            services.AddGeneratedRestApi();
            services.AddMixSwaggerServices(executingAssembly);
            services.AddSSL();

            services.AddResponseCompression();
            return services;
        }

        #endregion

        #region Apps

        public static IApplicationBuilder UseMixApps(
            this IApplicationBuilder app,
            Assembly executingAssembly,
            IConfiguration configuration,
            bool isDevelop,
            GlobalConfigService globalConfigService)
        {
            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin();
                builder.AllowAnyMethod();
                builder.AllowAnyHeader();
                builder.WithExposedHeaders("Grpc-Status", "Grpc-Message");
            });

            app.UseMixStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            if (globalConfigService.GetConfig<bool>(MixAppSettingKeywords.IsHttps))
            {
                app.UseHttpsRedirection();
            }

            app.UseMixModuleApps(configuration, isDevelop);
            app.UseMixSwaggerApps(isDevelop, executingAssembly);

            app.UseResponseCompression();
            return app;
        }

        #endregion

        #region Private

        #region App

        private static IApplicationBuilder UseMixSwaggerApps(this IApplicationBuilder app, bool isDevelop, Assembly assembly)
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

            return app;
        }

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

        private static IApplicationBuilder UseMixModuleApps(this IApplicationBuilder app, IConfiguration configuration, bool isDevelop)
        {
            foreach (var assembly in MixAssemblies)
            {
                var startupServices = assembly.GetExportedTypes().Where(IsStartupService);
                foreach (var startup in startupServices)
                {
                    ConstructorInfo classConstructor = startup.GetConstructor(Array.Empty<Type>());
                    var instance = classConstructor.Invoke(Array.Empty<Type>());
                    startup.GetMethod("UseApps").Invoke(instance, new object[] { app, configuration, isDevelop });
                }
            }

            return app;
        }
        #endregion

        #region Services

        private static IServiceCollection InitMixContext(this IServiceCollection services)
        {
            services.AddScoped<GlobalConfigService>();
            services.AddScoped<CultureService>();
            services.AddScoped<AuthConfigService>();
            services.AddScoped<SmtpConfigService>();
            services.AddScoped<MixEndpointService>();
            services.AddScoped<IPSecurityConfigService>();

            services.AddScoped<MixDatabaseService>();
            services.AddScoped<MixDataService>();

            return services;
        }

        private static void InitAppSettings()
        {
            MixFileService _fileService = new();

            if (!Directory.Exists(MixFolders.ConfiguratoinFolder))
            {
                _fileService.CopyFolder(MixFolders.SharedConfigurationFolder, MixFolders.ConfiguratoinFolder);
            }

            GlobalConfigService globalConfigService = new();

            if (!globalConfigService.GetConfig<bool>(MixAppSettingKeywords.IsInit))
            {
                var mixDatabaseService = new MixDatabaseService(globalConfigService);
                mixDatabaseService.InitMixCmsContext();

                // TODO: Update cache service
                //MixCacheService.InitMixCacheContext();
            }
        }

        private static IServiceCollection AddSSL(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var globalConfigService = serviceProvider.GetService<GlobalConfigService>();
            if (globalConfigService.GetConfig<bool>(MixAppSettingKeywords.IsHttps))
            {
                services.AddHttpsRedirection(options =>
                {
                    options.RedirectStatusCode = StatusCodes.Status308PermanentRedirect;
                    options.HttpsPort = 443;
                });
            }
            return services;
        }

        private static IServiceCollection AddMixSwaggerServices(this IServiceCollection services, Assembly assembly)
        {
            string title = assembly.ManifestModule.Name.Replace(".dll", string.Empty).ToHypenCase(' ');
            string version = "v2";
            string swaggerBasePath = $"api/{version}/{title.Replace(".", "-").ToHypenCase()}";
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(version, new OpenApiInfo { Title = title, Version = version });
                c.CustomSchemaIds(x => x.FullName);
            });
            return services;
        }

        private static IServiceCollection AddMixModuleServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add(new HttpResponseExceptionFilter());
                options.Conventions.Add(new ControllerDocumentationConvention());
            })
            .AddJsonOptions(opts =>
            {
                var enumConverter = new JsonStringEnumConverter();
                opts.JsonSerializerOptions.Converters.Add(enumConverter);
            })
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            foreach (var assembly in MixAssemblies)
            {
                var startupServices = assembly.GetExportedTypes().Where(IsStartupService);
                foreach (var startup in startupServices)
                {
                    ConstructorInfo classConstructor = startup.GetConstructor(Array.Empty<Type>());
                    var instance = classConstructor.Invoke(Array.Empty<Type>());
                    startup.GetMethod("AddServices").Invoke(instance, new object[] { services, configuration });

                }
            }
            return services;
        }

        private static IServiceCollection AddGeneratedRestApi(this IServiceCollection services)
        {
            List<Type> restCandidates = GetCandidatesByAttributeType(MixAssemblies, typeof(GenerateRestApiControllerAttribute));
            services.
                AddControllers(o => o.Conventions.Add(
                    new GenericControllerRouteConvention()
                )).
                ConfigureApplicationPartManager(m =>
                    {
                        m.FeatureProviders.Add(
                            new GenericTypeControllerFeatureProvider(restCandidates));
                    }
                    )
                .AddJsonOptions(opts =>
                {
                    var enumConverter = new JsonStringEnumConverter();
                    opts.JsonSerializerOptions.Converters.Add(enumConverter);
                })
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter()));
            return services;
        }

        #endregion


        private static bool IsStartupService(Type type)
        {
            var typeInfo = type.GetTypeInfo();
            return
                typeInfo.IsClass &&
                !typeInfo.IsAbstract &&
                !typeInfo.IsGenericType &&
                typeof(IStartupService).IsAssignableFrom(type);
        }

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
                                .Where(x => AssemblyName.GetAssemblyName(x).FullName.StartsWith("mix."))
                                .Select(x => Assembly.Load(AssemblyName.GetAssemblyName(x)));
            return assemblies.ToList();
        }

        #endregion

    }
}
