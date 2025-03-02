using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Mix.Database.Services.MixGlobalSettings;
using Mix.SignalR.Constants;
using Mix.SignalR.Interfaces;
using Mix.SignalR.Models;
using System;

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
