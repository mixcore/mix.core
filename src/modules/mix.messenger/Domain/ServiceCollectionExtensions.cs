using Microsoft.AspNetCore.Builder;
using Mix.SignalR.Constants;
using Mix.SignalR.Hubs;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IApplicationBuilder UseMixSignalRApp(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<PortalHub>(HubEndpoints.PortalHub);
                endpoints.MapHub<EditFileHub>(HubEndpoints.EditFileHub);
                endpoints.MapHub<MixThemeHub>(HubEndpoints.MixThemeHub);
                endpoints.MapHub<HighFrequencyHub>(HubEndpoints.HighFrequencyHub);
            });
            return app;
        }
    }
}
