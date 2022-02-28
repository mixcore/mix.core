using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Mix.SignalR.Hubs;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
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
