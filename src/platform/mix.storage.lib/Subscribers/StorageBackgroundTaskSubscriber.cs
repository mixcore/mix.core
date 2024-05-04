using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Mix.Log.Lib.Interfaces;
using Mix.Mq.Lib.Models;
using Mix.Queue.Engines;
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
            IMemoryQueueService<MessageQueueModel> queueService,
            ILogger<StorageBackgroundTaskSubscriber> logger,
            IPooledObjectPolicy<RabbitMQ.Client.IModel> rabbitMqObjectPolicy = null)
            : base(TopicId, nameof(StorageBackgroundTaskSubscriber), 20, serviceProvider, configuration, queueService, logger, rabbitMqObjectPolicy)
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
