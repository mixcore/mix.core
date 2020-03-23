using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Mix.Cms.Service.Gprc
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMixGprc(this IServiceCollection services)
        {
            services.AddGrpc();
            services.AddCors(o => o.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .WithExposedHeaders("Grpc-Status", "Grpc-Message");
            }));
            return services;
        }

        public static IApplicationBuilder UseMixGprc(this IApplicationBuilder app)
        {
            app.UseGrpcWeb();
            app.UseCors();
            app.UseEndpoints(endpoints =>
            {   
                endpoints.MapGrpcService<GreeterService>().EnableGrpcWeb().RequireCors("AllowAll");

            });
            return app;
        }
    }
}
