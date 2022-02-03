using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Mix.Cms.Lib.Controllers;
using Mix.Cms.Lib.Repositories;
using Mix.Cms.Lib.SignalR.Hubs;
using Mix.Heart.NetCore;
using System.Reflection;

namespace Mix.Cms.Lib.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<AuditLogRepository>();
            services.AddRepositories(Assembly.GetExecutingAssembly());
            return services;
        }

        public static IServiceCollection AddGenerateApis(this IServiceCollection services)
        {
            services.AddGeneratedRestApi(Assembly.GetExecutingAssembly(), typeof(BaseRestApiController<,,>));
            return services;
        }

        public static IServiceCollection AddMixSignalR(this IServiceCollection services)
        {
            services.AddSignalR()
                   .AddJsonProtocol(options =>
                   {
                       options.PayloadSerializerOptions.PropertyNamingPolicy = null;
                   });
            return services;
        }

        public static IApplicationBuilder UseMixSignalR(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<PortalHub>("/portalHub");
                //endpoints.MapHub<ServiceHub>("/serviceHub");
                endpoints.MapHub<EditFileHub>("/editFileHub");
            });
            return app;
        }
    }
}