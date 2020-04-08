using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Mix.Cms.Service.SignalR.Hubs;
using System;

namespace Mix.Cms.Service.SignalR
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMixSignalR(this IServiceCollection services)
        {
            services.AddSignalR()
                   .AddJsonProtocol(options =>
                   {
                       options.PayloadSerializerOptions.PropertyNamingPolicy = null;
                   })
                   .AddMessagePackProtocol();
            return services;
        }

        public static IApplicationBuilder UseMixSignalR(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<PortalHub>("/portalHub");
                endpoints.MapHub<ServiceHub>("/serviceHub");
                endpoints.MapHub<VideoChatHub>("/videoChatHub");
            });
            return app;
        }
    }
}
