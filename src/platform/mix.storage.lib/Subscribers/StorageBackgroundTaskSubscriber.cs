using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mix.Communicator.Models;
using Mix.Communicator.Services;
using Mix.Mixdb.Event.Services;
using Mix.Queue.Engines;
using Mix.Queue.Engines.MixQueue;
using Mix.Service.Commands;
using Mix.Service.Interfaces;
using Mix.Service.Services;
using Mix.SignalR.Enums;
using Mix.SignalR.Interfaces;
using Mix.SignalR.Models;
using Mix.Storage.Lib.Services;

namespace Mix.Storage.Lib.Subscribers
{
    public class StorageBackgroundTaskSubscriber : SubscriberBase
    {
        protected IPortalHubClientService PortalHub;
        protected IAuditLogService AuditLogService;
        private const string TopicId = MixQueueTopics.MixBackgroundTasks;

        public StorageBackgroundTaskSubscriber(
            IServiceProvider serviceProvider,
            IConfiguration configuration,
            MixQueueMessages<MessageQueueModel> queueService,
            IAuditLogService auditLogService,
            IPortalHubClientService portalHub)
            : base(TopicId, "Storage", serviceProvider, configuration, queueService)
        {
            AuditLogService = auditLogService;
            PortalHub = portalHub;
        }

        public override async Task Handler(MessageQueueModel model)
        {
            try
            {
                using (ServiceScope = ServicesProvider.CreateScope())
                {
                    switch (model.Action)
                    {
                        case MixQueueActions.ScaleImage:
                            MixStorageService srv = GetRequiredService<MixStorageService>();
                            await srv.ScaleImage(model.Data);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                await MixLogService.LogExceptionAsync(ex);
                await SendMessage(model.Action, false, ex);
            }
        }

        public override Task HandleException(Exception ex)
        {
            return MixLogService.LogExceptionAsync(ex);
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
