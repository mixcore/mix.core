using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
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
        public MixDbCommandHubClientService(IConfiguration configuration, MixEndpointService mixEndpointService, ILogger<MixDbCommandHubClientService> logger)
            : base(HubEndpoints.MixDbHub, mixEndpointService.MixMq, logger, configuration)
        {
        }

        protected override Task HandleMessage(SignalRMessageModel message)
        {
            return Task.CompletedTask;
        }
    }
}
