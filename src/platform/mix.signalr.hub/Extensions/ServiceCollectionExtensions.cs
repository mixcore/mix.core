using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Mix.SignalR.Constants;
using Mix.SignalR.Hubs;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IEndpointRouteBuilder UseMixSignalRApp(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapHub<PortalHub>(HubEndpoints.PortalHub);
            endpoints.MapHub<MixDbHub>(HubEndpoints.MixDbHub);
            endpoints.MapHub<LogStreamHub>(HubEndpoints.LogStreamHub);
            endpoints.MapHub<EditFileHub>(HubEndpoints.EditFileHub);
            endpoints.MapHub<MixThemeHub>(HubEndpoints.MixThemeHub);
            //endpoints.MapHub<HighFrequencyHub>(HubEndpoints.HighFrequencyHub);
            //endpoints.MapHub<VideoCallHub>(HubEndpoints.VideoCallHub);
            //endpoints.MapHub<AuthHub>("/hubs/auth");
            //endpoints.MapHub<SignalingHub>("/hubs/signaling");
            return endpoints;
        }
    }
}
