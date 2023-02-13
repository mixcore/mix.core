using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mix.Constant.Constants;
using Mix.Database.Entities.Cms;
using Mix.Heart.UnitOfWork;
using Mix.Queue.Engines;
using Mix.Queue.Engines.MixQueue;
using Mix.Queue.Models;
using Mix.RepoDb.Services;
using Mix.SignalR.Enums;
using Mix.SignalR.Models;
using Mix.SignalR.Services;

namespace Mix.RepoDb.Subscribers
{
    public class MixRepoDbSubscriber : SubscriberBase
    {
        protected PortalHubClientService PortalHub;
        
        private MixDbService _mixDbService;
        public MixRepoDbSubscriber(
            IConfiguration configuration,
            MixMemoryMessageQueue<MessageQueueModel> queueService,
            IServiceProvider serviceProvider,
            PortalHubClientService portalHub)
            : base(MixQueueTopics.MixRepoDb, string.Empty, serviceProvider, configuration, queueService)
        {
            PortalHub = portalHub;
        }

        public override async Task Handler(MessageQueueModel model)
        {
            try
            {
                using (ServiceScope = ServicesProvider.CreateScope())
                {
                    _mixDbService = GetScopedService<MixDbService>();
                    var cmsUow = GetScopedService<UnitOfWorkInfo<MixCmsContext>>();
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
                    await cmsUow.CompleteAsync();
                }
            }
            catch (Exception ex)
            {
                await SendMessage(model.Action, false, ex);
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

        private async Task SendMessage(string message, bool result, Exception? ex = null)
        {
            SignalRMessageModel msg = new()
            {
                Action = MessageAction.NewMessage,
                Type = result ? MessageType.Success : MessageType.Error,
                Title = message,
                From = new(GetType().FullName),
                Data = result,
                Message = ex == null ? message : ex!.Message
            };
            await PortalHub.SendMessageAsync(msg);
        }
    }
}
