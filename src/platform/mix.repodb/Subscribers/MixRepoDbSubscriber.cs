using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Mix.Constant.Constants;
using Mix.Database.Entities.Cms;
using Mix.Heart.Services;
using Mix.Heart.UnitOfWork;
using Mix.Mq.Lib.Models;
using Mix.Queue.Engines;
using Mix.Queue.Interfaces;
using Mix.RepoDb.Interfaces;
using Mix.RepoDb.ViewModels;
using Mix.SignalR.Enums;
using Mix.SignalR.Interfaces;
using Mix.SignalR.Models;

namespace Mix.RepoDb.Subscribers
{
    public class MixRepoDbSubscriber : SubscriberBase
    {
        protected IPortalHubClientService PortalHub;
        protected UnitOfWorkInfo<MixCmsContext> _cmsUow;
        protected RepoDbMixDatabaseViewModel _database;
        private IMixDbService _mixDbService;
        public MixRepoDbSubscriber(
            IConfiguration configuration,
            IServiceProvider serviceProvider,
            IPortalHubClientService portalHub,
            IMemoryQueueService<MessageQueueModel> queueService,
            ILogger<MixRepoDbSubscriber> logger,
            IPooledObjectPolicy<RabbitMQ.Client.IModel> rabbitMqObjectPolicy = null)
            : base(MixQueueTopics.MixRepoDb, nameof(MixRepoDbSubscriber), 20, serviceProvider, configuration, queueService, logger, rabbitMqObjectPolicy)
        {
            PortalHub = portalHub;
        }

        public override async Task Handler(MessageQueueModel model)
        {
            try
            {

                _mixDbService = GetRequiredService<IMixDbService>();
                var cacheService = GetRequiredService<MixCacheService>();
                _cmsUow = GetRequiredService<UnitOfWorkInfo<MixCmsContext>>();
                _database = await RepoDbMixDatabaseViewModel.GetRepository(_cmsUow, cacheService)
                    .GetSingleAsync(m => m.SystemName == model.Data);
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
                await _cmsUow.CompleteAsync();
                _cmsUow.Dispose();
            }
            catch (Exception ex)
            {
                await SendMessage(model.Action, false, ex);
            }
        }

        private async Task BackupDatabase(MessageQueueModel model)
        {
            await _mixDbService.BackupDatabase(_database);
            await SendMessage($"{MixRepoDbQueueAction.Backup} {model.Data} Successfully", true);
        }

        private async Task RestoreDatabase(MessageQueueModel model)
        {
            await _mixDbService.RestoreFromLocal(_database);
            await SendMessage($"{MixRepoDbQueueAction.Restore} {model.Data} Successfully", true);
        }

        private async Task MigrateDatabase(MessageQueueModel model)
        {
            await _mixDbService.MigrateDatabase(_database);
            string msg = $"{MixRepoDbQueueAction.Migrate} {model.Data} Successfully";
            await SendMessage(msg, true);
        }

        private async Task UpdateDatabase(MessageQueueModel model)
        {
            await _mixDbService.BackupDatabase(_database);
            await _mixDbService.MigrateDatabase(_database);
            await _mixDbService.RestoreFromLocal(_database);
            string msg = $"{MixRepoDbQueueAction.Update} {model.Data} Successfully";
            await SendMessage(msg, true);
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
