using Microsoft.AspNetCore.Builder;
using Mix.SignalR.Hubs;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMixSignalR(this IServiceCollection services)
        {
            services.AddSignalR()
                   .AddMessagePackProtocol();
            return services;
        }
        public static IApplicationBuilder UseMixSignalR(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<PortalHub>("/portalHub");
                endpoints.MapHub<EditFileHub>("/editFileHub");
            });
            return app;
        }
    }
}
