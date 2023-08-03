using Microsoft.AspNetCore.Http;
using Mix.Shared.Services;
using Mix.SignalR.Constants;
using Mix.SignalR.Interfaces;
using Mix.SignalR.Models;
using System;

namespace Mix.Service.Services
{
    public class LogStreamHubClientService : BaseHubClientService, ILogStreamHubClientService
    {
        public LogStreamHubClientService(MixEndpointService mixEndpointService)
            : base(HubEndpoints.LogStreamHub, mixEndpointService)
        {
        }

        protected override Task HandleMessage(SignalRMessageModel message)
        {
            return Task.CompletedTask;
        }
    }
}
