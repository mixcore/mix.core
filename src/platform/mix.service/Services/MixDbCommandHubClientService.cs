using Microsoft.AspNetCore.Http;
using Mix.Shared.Services;
using Mix.SignalR.Constants;
using Mix.SignalR.Interfaces;
using Mix.SignalR.Models;
using System;

namespace Mix.Service.Services
{
    public class MixDbCommandHubClientService : BaseHubClientService, IMixDbCommandHubClientService
    {
        public MixDbCommandHubClientService(MixEndpointService mixEndpointService)
            : base(HubEndpoints.MixDbCommandHub, mixEndpointService)
        {
        }

        protected override Task HandleMessage(SignalRMessageModel message)
        {
            Console.WriteLine(message.ToString());
            return Task.CompletedTask;
        }
    }
}
