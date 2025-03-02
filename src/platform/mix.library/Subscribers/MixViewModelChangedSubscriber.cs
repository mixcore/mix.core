using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Mix.Heart.Model;
using Mix.Lib.Interfaces;
using Mix.Lib.Subscribers.Handlers.MixViewModelChangedHandlers;
using Mix.Mixdb.Interfaces;
using Mix.Mq.Lib.Models;
using Mix.Queue.Engines;

namespace Mix.Lib.Subscribers
{
    public sealed class MixViewModelChangedSubscriber : SubscriberBase
    {
        private static readonly string TopicId = MixQueueTopics.MixViewModelChanged;
        private readonly IMixTenantService _mixTenantService;
        public MixViewModelChangedSubscriber(
            IServiceProvider serviceProvider,
            IConfiguration configuration,
            IMixTenantService mixTenantService,
            IMemoryQueueService<MessageQueueModel> queueService,
            ILogger<MixViewModelChangedSubscriber> logger,
            IPooledObjectPolicy<RabbitMQ.Client.IModel>? rabbitMQObjectPolicy = null)
            : base(TopicId, nameof(MixDbCommandSubscriber), 20, serviceProvider, configuration, queueService, logger, rabbitMQObjectPolicy)
        {
            _mixTenantService = mixTenantService;
        }

        public override async Task Handler(MessageQueueModel model, CancellationToken cancellationToken)
        {
            CacheService ??= GetRequiredService<MixCacheService>();
            await UpdateCacheHandler(model);
            switch (model.DataTypeFullName)
            {
                case var m when m == typeof(MixTemplateViewModel).FullName:
                    await TemplateHandler.MessageQueueHandler(model);
                    break;
                case var m when m == typeof(MixPageContentViewModel).FullName:
                    await PageContentHandler.MessageQueueHandler(model, CacheService);
                    break;
                case var m when m == typeof(MixTenantSystemViewModel).FullName:
                    await MixTenantSystemViewModelHandler.MessageQueueHandler(model, _mixTenantService);
                    break;
                case var m when m == typeof(MixTenantSystemViewModel).FullName:
                    await MixDomainViewModelHandler.MessageQueueHandler(model, _mixTenantService);
                    break;
                case var m when m == typeof(MixDatabaseColumnViewModel).FullName:
                    var mixDbService = GetRequiredService<IMixdbStructure>();
                    if (mixDbService != null) {
                        await MixDatabaseColumnViewModelHandler.MessageQueueHandler(model,mixDbService);
                    }
                    break;
                default:
                    break;
            }
        }

        private async Task UpdateCacheHandler(MessageQueueModel data)
        {
            if (data.Data.IsJsonString())
            {
                JObject vm = JObject.Parse(data.Data);

                if (vm.ContainsKey("modifiedEntities"))
                {
                    using (var serviceScope = ServicesProvider.CreateScope())
                    {
                        var cacheService = serviceScope.ServiceProvider.GetRequiredService<MixCacheService>();
                        switch (data.Action)
                        {
                            case "PATCH":
                            case "Post":
                            case "Put":
                            case "DELETE":
                                var modifiedEntities = vm.Value<JArray>("modifiedEntities")?.ToObject<List<ModifiedEntityModel>>();
                                await cacheService.RemoveCachesAsync(modifiedEntities);
                                break;
                            case "Get":
                            default:
                                break;
                        }
                        serviceScope.Dispose();
                    }
                }
            }
        }
    }
}
