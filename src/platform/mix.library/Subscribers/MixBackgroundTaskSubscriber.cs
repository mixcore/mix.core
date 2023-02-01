using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mix.Communicator.Models;
using Mix.Communicator.Services;
using Mix.Queue.Engines;
using Mix.Queue.Engines.MixQueue;
using Mix.SignalR.Enums;
using Mix.SignalR.Models;
using Mix.SignalR.Services;
using System;

namespace Mix.Lib.Subscribers
{
    public class MixBackgroundTaskSubscriber : SubscriberBase
    {
        protected PortalHubClientService PortalHub;
        protected AuditLogService AuditLogService;
        private const string TopicId = MixQueueTopics.MixBackgroundTasks;

        public MixBackgroundTaskSubscriber(
            IServiceProvider serviceProvider,
            IConfiguration configuration,
            MixMemoryMessageQueue<MessageQueueModel> queueService,
            AuditLogService auditLogService,
            PortalHubClientService portalHub)
            : base(TopicId, string.Empty, serviceProvider, configuration, queueService)
        {
            AuditLogService = auditLogService;
            PortalHub = portalHub;
        }

        public override async Task Handler(MessageQueueModel model)
        {

            switch (model.Action)
            {
                case MixQueueActions.AuditLog:
                    var cmd = model.ParseData<LogAuditLogCommand>();
                    AuditLogService.SaveToDatabase(cmd.UserName, cmd.Request, true, null);
                    break;
                case MixQueueActions.SendMail:
                    await SendMail(model);

                    break;
            }
        }

        private async Task SendMail(MessageQueueModel model)
        {
            try
            {
                using (ServiceScope = _servicesProvider.CreateScope())
                {
                    var msg = model.ParseData<EmailMessageModel>();
                    var emailService = GetScopedService<EmailService>();
                    await emailService.SendMail(msg);
                    await SendMessage($"Sent Email {msg.Subject} to {msg.To}", true);
                }
            }
            catch (Exception ex)
            {
                MixService.LogException(ex);
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
                Data = result,
                Message = ex == null ? message : ex!.Message
            };
            await PortalHub.SendMessageAsync(msg);
        }
    }
}
