using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Mix.Constant.Constants;
using Mix.Database.Entities.Cms;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Heart.Model;
using Mix.Heart.Services;
using Mix.Lib.Interfaces;
using Mix.SignalR.Constants;
using Mix.SignalR.Enums;
using Mix.SignalR.Interfaces;
using Mix.SignalR.Models;
using Newtonsoft.Json.Linq;

namespace Mix.Service.Services
{
    public class PortalHubClientService : BaseHubClientService, IPortalHubClientService
    {
        private static Type[] DomainTypes =
        {
            typeof(MixDomain),typeof(MixTenant), typeof(MixCulture)
        };

        private readonly IServiceProvider _serviceProvider;
        private readonly IMixTenantService _mixTenantService;
        public PortalHubClientService(
            IServiceProvider serviceProvider,
            MixEndpointService mixEndpointService,
            IMixTenantService mixTenantService,
            ILogger<PortalHubClientService> logger)
            : base(HubEndpoints.PortalHub, mixEndpointService.MixMq, logger)
        {
            _serviceProvider = serviceProvider;
            _mixTenantService = mixTenantService;
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
                        if (message.Data != null && message.Data.GetType() != typeof(string))
                        {
                            var obj = JObject.FromObject(message.Data);
                            if (obj.ContainsKey("modifiedEntities"))
                            {
                                var modifiedEntities = obj.Value<JArray>("modifiedEntities")?.ToObject<List<ModifiedEntityModel>>();
                                if (modifiedEntities != null)
                                {
                                    var cacheService = serviceScope.ServiceProvider.GetRequiredService<MixCacheService>();
                                    await cacheService.RemoveCachesAsync(modifiedEntities);
                                    if (modifiedEntities.Any(m => DomainTypes.Contains(m.EntityType)))
                                    {
                                        Thread.Sleep(10000);
                                        await _mixTenantService.Reload();
                                    }
                                }
                            }

                        }
                        if (message.Message == MixQueueActions.ClearCache)
                        {
                            var cacheService = serviceScope.ServiceProvider.GetRequiredService<MixCacheService>();
                            await cacheService.ClearAllCacheAsync();
                        }

                        break;
                }
            }
        }
    }
}
