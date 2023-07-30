using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Mix.Constant.Constants;
using Mix.Heart.Enums;
using Mix.Heart.Extensions;
using Mix.Heart.Helpers;
using Mix.Heart.Model;
using Mix.Heart.Services;
using Mix.Shared.Services;
using Mix.SignalR.Constants;
using Mix.SignalR.Enums;
using Mix.SignalR.Interfaces;
using Mix.SignalR.Models;
using Newtonsoft.Json.Linq;
using System;

namespace Mix.Service.Services
{
    public class PortalHubClientService : BaseHubClientService, IPortalHubClientService
    {
        private readonly IServiceProvider _serviceProvider;
        public PortalHubClientService(IServiceProvider serviceProvider, MixEndpointService mixEndpointService)
            : base(HubEndpoints.PortalHub, mixEndpointService)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task HandleMessage(SignalRMessageModel message)
        {
            if (message.Action == MessageAction.NewQueueMessage)
            {
                await HandleQueueMessage(message);
            }
        }

        private async Task HandleQueueMessage(SignalRMessageModel message)
        {
            using (var serviceScope = _serviceProvider.CreateScope())
            {

                switch (message.Title)
                {
                    case MixQueueTopics.MixViewModelChanged:
                        if (message.Data.IsJsonString())
                        {
                            var obj = JObject.Parse(message.Data);
                            if (obj.ContainsKey("modifiedEntities"))
                            {
                                var modifiedEntities = obj.Value<JArray>("modifiedEntities")?.ToObject<List<ModifiedEntityModel>>();
                                var cacheService = serviceScope.ServiceProvider.GetRequiredService<MixCacheService>();
                                await cacheService.RemoveCachesAsync(modifiedEntities);
                            }
                        }
                        break;
                }
            }
        }
    }
}
