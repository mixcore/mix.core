using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Mix.Database.Services.MixGlobalSettings;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        static string[] origins;

        public static IHostApplicationBuilder AddMixCors(this IHostApplicationBuilder builder)
        {
            var mixEndpointService = builder.Services.GetService<MixEndpointService>();
            origins = mixEndpointService.Endpoints
            .Where(e => !string.IsNullOrEmpty(e))
            .Distinct()
            .ToArray();
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(p =>
                {
                    p.SetIsOriginAllowedToAllowWildcardSubdomains();
                    if (builder.Configuration.GetValue<bool>("AllowAnyOrigin"))
                    {
                        p.AllowAnyOrigin();
                    }
                    else
                    {
                        p.WithOrigins(origins);
                        p.AllowCredentials();
                    }
                    p.AllowAnyHeader();
                    p.AllowAnyMethod();
                });

                //options.AddPolicy(name: MixCorsPolicies.PublicApis,
                //    p =>
                //{
                //    p.AllowAnyOrigin();
                //    p.AllowAnyHeader();
                //    p.AllowAnyMethod();
                //});
            });
            return builder;
        }

        public static IApplicationBuilder UseMixCors(this IApplicationBuilder app, IConfiguration configuration)
        {
            app.UseCors(builder =>
            {
                builder.SetIsOriginAllowedToAllowWildcardSubdomains();
                if (configuration.GetValue<bool>("AllowAnyOrigin"))
                {
                    builder.AllowAnyOrigin();
                }
                else
                {
                    builder.WithOrigins(origins);
                    builder.AllowCredentials();
                }

                builder.AllowAnyMethod();
                builder.AllowAnyHeader();
            });
            return app;
        }
    }
}
