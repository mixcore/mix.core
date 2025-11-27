using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Mix.Database.Services.MixGlobalSettings;
using Mix.SignalR.Constants;
using Mix.SignalR.Interfaces;
using Mix.SignalR.Models;
using System;

namespace Mix.Service.Services
{
    public class MixDbCommandHubClientService : BaseHubClientService, IMixDbCommandHubClientService
    {
        public MixDbCommandHubClientService(MixEndpointService mixEndpointService, ILogger<MixDbCommandHubClientService> logger)
            : base(HubEndpoints.MixDbHub, mixEndpointService.MixMq, logger)
        {
        }

        protected override Task HandleMessage(SignalRMessageModel message)
        {
            return Task.CompletedTask;
        }
    }
}
