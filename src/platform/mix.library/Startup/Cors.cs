using Microsoft.AspNetCore.Builder;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        static string[] origins;

        public static IServiceCollection AddMixCors(this IServiceCollection services)
        {
            var _mixEndpointService = services.GetService<MixEndpointService>();
            origins = _mixEndpointService.Endpoints
            .Where(e => !string.IsNullOrEmpty(e))
            .Distinct()
            .ToArray();
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.SetIsOriginAllowedToAllowWildcardSubdomains();
                    if (GlobalConfigService.Instance.AllowAnyOrigin)
                    {
                        builder.AllowAnyOrigin();
                    }
                    else if (origins.Count() > 0)
                    {
                        builder.WithOrigins(origins);
                        builder.AllowCredentials();
                    }
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                });
            });
            return services;
        }

        public static IApplicationBuilder UseMixCors(this IApplicationBuilder app)
        {
            app.UseCors(builder =>
            {
                builder.SetIsOriginAllowedToAllowWildcardSubdomains();
                if (GlobalConfigService.Instance.AllowAnyOrigin)
                {
                    builder.AllowAnyOrigin();
                }
                else if (origins.Count() > 0)
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
