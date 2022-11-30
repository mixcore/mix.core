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

namespace Mix.RepoDb.Sbuscribers
{
    public class MixRepoDbSubscriber : SubscriberBase
    {
        protected PortalHubClientService _portalHub;
        private IServiceProvider _servicesProvider;
        private IServiceScope _servicesScope;
        private UnitOfWorkInfo<MixCmsContext> _cmsUOW;
        private MixDbService _mixDbService;
        public MixRepoDbSubscriber(
            IConfiguration configuration,
            MixMemoryMessageQueue<MessageQueueModel> queueService,
            IServiceProvider servicesProvider,
            PortalHubClientService portalHub)
            : base(MixQueueTopics.MixRepoDb, string.Empty, configuration, queueService)
        {
            _portalHub = portalHub;
            _servicesProvider = servicesProvider;
            _servicesScope = _servicesProvider.CreateScope();
            _mixDbService = _servicesScope.ServiceProvider.GetRequiredService<MixDbService>();
        }

        public override async Task Handler(MessageQueueModel model)
        {

            _cmsUOW = _servicesScope.ServiceProvider.GetRequiredService<UnitOfWorkInfo<MixCmsContext>>();
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
                default:
                    break;
            }
            await _cmsUOW.CompleteAsync();
        }

        private async Task BackupDatabase(MessageQueueModel model)
        {
            try
            {
                await _mixDbService.BackupDatabase(model.Data);
                await SendMessage($"{MixRepoDbQueueAction.Backup} {model.Data} Successfully", true);
            }
            catch (Exception ex)
            {
                await SendMessage(MixRepoDbQueueAction.Backup, false, ex);
            }
        }

        private async Task RestoreDatabase(MessageQueueModel model)
        {
            try
            {
                await _mixDbService.RestoreFromLocal(model.Data);
                await SendMessage($"{MixRepoDbQueueAction.Restore} {model.Data} Successfully", true);
            }
            catch (Exception ex)
            {
                await SendMessage(MixRepoDbQueueAction.Restore, false, ex);
            }
        }

        private async Task MigrateDatabase(MessageQueueModel model)
        {
            try
            {
                await _mixDbService.MigrateDatabase(model.Data);
                string msg = $"{MixRepoDbQueueAction.Migrate} {model.Data} Successfully";
                await SendMessage(msg, true);
            }
            catch (Exception ex)
            {
                await SendMessage(MixRepoDbQueueAction.Migrate, false, ex);
            }
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
            await _portalHub.SendMessageAsync(msg);
        }
    }
}
