using Microsoft.Extensions.Logging;
using Mix.Shared.Services;
using Mix.SignalR.Constants;
using Mix.SignalR.Interfaces;
using Mix.SignalR.Models;

namespace Mix.Service.Services
{
    public class LogStreamHubClientService : BaseHubClientService, ILogStreamHubClientService
    {
        public LogStreamHubClientService(MixEndpointService mixEndpointService, ILogger<LogStreamHubClientService> logger)
            : base(HubEndpoints.LogStreamHub, mixEndpointService.MixMq, logger)
        {
        }

        protected override Task HandleMessage(SignalRMessageModel message)
        {
            return Task.CompletedTask;
        }
    }
}
