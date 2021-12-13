using Microsoft.AspNetCore.Builder;
using Mix.Lib.Conventions;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Mix.Lib.Filters;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
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
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
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

    }
}
