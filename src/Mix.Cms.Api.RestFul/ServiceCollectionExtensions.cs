using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Mix.Cms.Api.RestFul
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMixRestApi(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Mixcore API", Version = "v3" });
                c.CustomSchemaIds(x => x.FullName);
            });
            return services;
        }

        public static IApplicationBuilder UseMixRestApi(this IApplicationBuilder app)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mixcore API V1");
            });

            return app;
        }
    }
}