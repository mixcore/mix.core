using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mix.Communicator.Models;
using Mix.Communicator.Services;
using Mix.Lib.Subscribers;
using Mix.Log.Lib.Interfaces;
using Mix.Mixdb.Event.Services;
using Mix.Mq.Lib.Models;
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
        private static string[] allowActions =
        {
            MixQueueActions.ScaleImage
        };
        public StorageBackgroundTaskSubscriber(
            IServiceProvider serviceProvider,
            IConfiguration configuration,
            IAuditLogService auditLogService,
            IPortalHubClientService portalHub,
            IQueueService<MessageQueueModel> queueService)
            : base(TopicId, nameof(StorageBackgroundTaskSubscriber), 20, serviceProvider, configuration, queueService)
        {
            AuditLogService = auditLogService;
            PortalHub = portalHub;
        }

        public override async Task Handler(MessageQueueModel model)
        {
            if (!allowActions.Contains(model.Action))
            {
                return;
            }

            try
            {

                switch (model.Action)
                {
                    case MixQueueActions.ScaleImage:
                        using (ServiceScope = ServicesProvider.CreateScope())
                        {
                            MixStorageService srv = GetRequiredService<MixStorageService>();
                            await srv.ScaleImage(model.Data);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                await MixLogService.LogExceptionAsync(ex);
                await SendMessage(model.Action, false, ex);
                return;
            }
        }

        public override Task HandleException(MessageQueueModel model, Exception ex)
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
