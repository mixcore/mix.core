using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Mix.Lib.Conventions;
using Mix.Lib.Filters;
using Mix.Service.Interfaces;
using Mix.Shared.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static IHostApplicationBuilder AddIStartupServices(this IHostApplicationBuilder builder, string prefix)
        {
            builder.Services.AddControllers(options =>
            {
                options.Filters.Add(new HttpResponseExceptionFilter(builder.Environment.IsProduction()));
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

            foreach (var assembly in RefAssemblies(prefix))
            {
                var startupServices = assembly.GetExportedTypes().Where(IsStartupService);
                foreach (var startup in startupServices)
                {
                    ConstructorInfo classConstructor = startup.GetConstructor(Array.Empty<Type>());
                    var instance = classConstructor.Invoke(Array.Empty<Type>());
                    startup.GetMethod(nameof(IStartupService.AddServices)).Invoke(instance, new object[] { builder });
                }
            }
            return builder;
        }

        public static IApplicationBuilder UseIStartupApps(this IApplicationBuilder app, string prefix, IConfiguration configuration, bool isDevelop)
        {
            foreach (var assembly in RefAssemblies(prefix))
            {
                var startupServices = assembly.GetExportedTypes().Where(IsStartupService);
                foreach (var startup in startupServices)
                {
                    ConstructorInfo classConstructor = startup.GetConstructor(Array.Empty<Type>());
                    var instance = classConstructor.Invoke(Array.Empty<Type>());
                    startup.GetMethod(nameof(IStartupService.UseApps)).Invoke(instance, new object[] { app, configuration, isDevelop });
                }
            }

            return app;
        }

    }
}
