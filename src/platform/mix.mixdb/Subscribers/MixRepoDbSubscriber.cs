using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Mix.Constant.Constants;
using Mix.Database.Entities.Cms;
using Mix.Heart.Services;
using Mix.Heart.UnitOfWork;
using Mix.Mixdb.Interfaces;
using Mix.Mixdb.Services;
using Mix.Mixdb.ViewModels;
using Mix.Mq.Lib.Models;
using Mix.Queue.Engines;
using Mix.Queue.Interfaces;
using Mix.SignalR.Enums;
using Mix.SignalR.Interfaces;
using Mix.SignalR.Models;

namespace Mix.Mixdb.Subscribers
{
    public class MixRepoDbSubscriber : SubscriberBase
    {
        protected IPortalHubClientService PortalHub;
        private IMixdbStructure? _mixDbService;
        public MixRepoDbSubscriber(
            IConfiguration configuration,
            IServiceProvider serviceProvider,
            IPortalHubClientService portalHub,
            IMemoryQueueService<MessageQueueModel> queueService,
            ILogger<MixRepoDbSubscriber> logger,
            IPooledObjectPolicy<RabbitMQ.Client.IModel>? rabbitMQObjectPolicy = null)
            : base(MixQueueTopics.MixRepoDb, nameof(MixRepoDbSubscriber), 20, serviceProvider, configuration, queueService, logger, rabbitMQObjectPolicy)
        {
            PortalHub = portalHub;
        }

        public override async Task Handler(MessageQueueModel model, CancellationToken cancellationToken)
        {
            _mixDbService = GetRequiredService<IMixdbStructure>();
            if (_mixDbService is null)
            {
                throw new NullReferenceException();
            }
            var cacheService = GetRequiredService<MixCacheService>();
            switch (model.Action)
            {
                case MixRepoDbQueueAction.Backup:
                    await BackupDatabase(model);
                    break;
                case MixRepoDbQueueAction.Restore:
                    await RestoreDatabase(model);
                    break;
                case MixRepoDbQueueAction.Migrate:
                    await MigrateDatabase(model);
                    break;
                case MixRepoDbQueueAction.Update:
                    await UpdateDatabase(model);
                    break;
                default:
                    break;
            }
        }

        private async Task BackupDatabase(MessageQueueModel model)
        {
            await _mixDbService.BackupDatabase(model.Data);
            await SendMessage($"{MixRepoDbQueueAction.Backup} {model.Data} Successfully", true);
        }

        private async Task RestoreDatabase(MessageQueueModel model)
        {
            await _mixDbService.RestoreFromLocal(model.Data);
            await SendMessage($"{MixRepoDbQueueAction.Restore} {model.Data} Successfully", true);
        }

        private async Task MigrateDatabase(MessageQueueModel model)
        {
            await _mixDbService.MigrateDatabase(model.Data);
            string msg = $"{MixRepoDbQueueAction.Migrate} {model.Data} Successfully";
            await SendMessage(msg, true);
        }

        private async Task UpdateDatabase(MessageQueueModel model)
        {
            await _mixDbService.BackupDatabase(model.Data);
            await _mixDbService.MigrateDatabase(model.Data);
            await _mixDbService.RestoreFromLocal(model.Data);
            string msg = $"{MixRepoDbQueueAction.Update} {model.Data} Successfully";
            await SendMessage(msg, true);
        }

        public override async Task HandleException(MessageQueueModel data, Exception ex)
        {
            await SendMessage(data.Action, false, ex);
            await base.HandleException(data, ex);
        }

        private async Task SendMessage(string message, bool result, Exception? ex = null)
        {
            SignalRMessageModel msg = new()
            {
                Action = MessageAction.NewMessage,
                Type = result ? MessageType.Success : MessageType.Error,
                Title = message,
                From = new(GetType().FullName),
                Message = ex == null ? message : ex!.Message
            };
            await PortalHub.SendMessageAsync(msg);
        }
    }
}
