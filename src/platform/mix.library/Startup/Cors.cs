using Microsoft.AspNetCore.Builder;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        static string[] origins = MixEndpointService.Instance.Endpoints
            .Where(e => !string.IsNullOrEmpty(e))
            .Distinct()
            .ToArray();

        public static IServiceCollection AddMixCors(this IServiceCollection services)
        {

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    if (origins.Count() > 0)
                    {
                        builder.WithOrigins(origins);
                        builder.AllowCredentials();
                    }
                    else
                    {
                        builder.AllowAnyOrigin();
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
                if (origins.Count() > 0)
                {
                    builder.WithOrigins(origins);
                    builder.AllowCredentials();
                }
                else
                {
                    builder.AllowAnyOrigin();
                }
                builder.AllowAnyMethod();
                builder.AllowAnyHeader();
            });
            return app;
        }
    }
}
