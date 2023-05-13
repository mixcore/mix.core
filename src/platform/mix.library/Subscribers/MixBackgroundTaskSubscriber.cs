using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mix.Communicator.Models;
using Mix.Communicator.Services;
using Mix.Mixdb.Event.Services;
using Mix.Queue.Engines;
using Mix.Queue.Engines.MixQueue;
using Mix.Service.Commands;
using Mix.Service.Interfaces;
using Mix.SignalR.Enums;
using Mix.SignalR.Interfaces;
using Mix.SignalR.Models;

namespace Mix.Lib.Subscribers
{
    public class MixBackgroundTaskSubscriber : SubscriberBase
    {
        protected IPortalHubClientService PortalHub;
        protected IAuditLogService AuditLogService;
        protected MixDbEventService MixDbEventService;
        private const string TopicId = MixQueueTopics.MixBackgroundTasks;

        public MixBackgroundTaskSubscriber(
            IServiceProvider serviceProvider,
            IConfiguration configuration,
            MixMemoryMessageQueue<MessageQueueModel> queueService,
            IAuditLogService auditLogService,
            IPortalHubClientService portalHub,
            MixDbEventService mixDbEventService)
            : base(TopicId, string.Empty, serviceProvider, configuration, queueService)
        {
            AuditLogService = auditLogService;
            PortalHub = portalHub;
            MixDbEventService = mixDbEventService;
        }

        public override async Task Handler(MessageQueueModel model)
        {

            switch (model.Action)
            {
                case MixQueueActions.AuditLog:
                    var cmd = model.ParseData<LogAuditLogCommand>();
                    if (cmd != null)
                    {
                        await AuditLogService.SaveRequestAsync(cmd.Request);
                    }
                    break;
                
                case MixQueueActions.SendMail:
                    await SendMail(model);
                    break;
                
                case MixQueueActions.MixDbEvent:
                    var evtCmd = model.ParseData<MixDbEventCommand>();
                    if (evtCmd != null)
                    {
                        await MixDbEventService.HandleMessage(evtCmd);
                    }
                    break;
            }
        }

        public override Task HandleException(Exception ex)
        {
            return MixLogService.LogExceptionAsync(ex);
        }

        private async Task SendMail(MessageQueueModel model)
        {
            try
            {
                using (ServiceScope = ServicesProvider.CreateScope())
                {
                    var msg = model.ParseData<EmailMessageModel>();
                    var emailService = GetRequiredService<EmailService>();
                    await emailService.SendMail(msg);
                    await SendMessage($"Sent Email {msg.Subject} to {msg.To}", true);
                }
            }
            catch (Exception ex)
            {
                await MixLogService.LogExceptionAsync(ex);
                await SendMessage($"Error {model.Action}: {model.Data}", false, ex);
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
                Message = ex == null ? message : ex!.Message
            };
            await PortalHub.SendMessageAsync(msg);
        }
    }
}
