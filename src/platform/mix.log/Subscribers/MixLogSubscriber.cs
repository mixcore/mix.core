using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Mix.Constant.Constants;
using Mix.Log.Lib.Commands;
using Mix.Log.Lib.Interfaces;
using Mix.Mq.Lib.Models;
using Mix.Queue.Engines;
using Mix.Queue.Interfaces;
using Mix.Service.Services;
using Mix.SignalR.Enums;
using Mix.SignalR.Interfaces;
using Mix.SignalR.Models;

namespace Mix.Log.Lib.Subscribers
{
    public class MixLogSubscriber : SubscriberBase
    {
        protected IPortalHubClientService _portalHub;
        protected IAuditLogService _auditLogService;
        protected IMixQueueLog _queueMessageLogService;
        private const string TopicId = MixQueueTopics.MixLog;
        private static string[] allowActions =
        {
            MixQueueActions.AuditLog,
            MixQueueActions.AckLog,
            MixQueueActions.ExceptionLog,
            MixQueueActions.DeadLetter,
            MixQueueActions.EnqueueLog,
        };
        public MixLogSubscriber(
            IServiceProvider serviceProvider,
            IConfiguration configuration,
            IPortalHubClientService portalHub,
            IMemoryQueueService<MessageQueueModel> queueService,
            IMixQueueLog queueMessageLogService,
            IAuditLogService auditLogService,
            ILogger<MixLogSubscriber> logger,
            IPooledObjectPolicy<RabbitMQ.Client.IModel>? rabbitMQObjectPolicy = null)
            : base(TopicId, nameof(MixLogSubscriber), 20, serviceProvider, configuration, queueService, logger, rabbitMQObjectPolicy)
        {
            _queueMessageLogService = queueMessageLogService;
            _portalHub = portalHub;
            _auditLogService = auditLogService;
        }

        public override Task StartAsync(CancellationToken cancellationToken = default)
        {
            base.StartAsync(cancellationToken);

            return Task.Run(async () =>
            {
                while (_portalHub.Connection == null || _portalHub.Connection.State != Microsoft.AspNetCore.SignalR.Client.HubConnectionState.Connected)
                {
                    try
                    {
                        await Task.Delay(5000);
                        await _portalHub.StartConnection();
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(GetType().Name, ex);
                    }
                }
            });
        }
        public override async Task Handler(MessageQueueModel model, CancellationToken cancellationToken)
        {
            if (!allowActions.Contains(model.Action))
            {
                return;
            }
            try
            {

                switch (model.Action)
                {
                    case MixQueueActions.AuditLog:
                        var auditLogCmd = model.ParseData<LogAuditLogCommand>();
                        if (auditLogCmd != null)
                        {
                            await _auditLogService.SaveRequestAsync(auditLogCmd.Request);
                        }
                        break;

                    case MixQueueActions.EnqueueLog:
                        var queueMessage = model.ParseData<MessageQueueModel>();

                        if (queueMessage != null)
                        {
                            await _queueMessageLogService.EnqueueMessageAsync(queueMessage);
                        }
                        break;
                    case MixQueueActions.AckLog:
                        var ackQueueMessage = model.ParseData<MessageQueueModel>();

                        if (ackQueueMessage != null)
                        {
                            ackQueueMessage.Sender = model.Sender;
                            await _queueMessageLogService.AckQueueMessage(ackQueueMessage);
                        }
                        break;

                    case MixQueueActions.ExceptionLog:
                        var ex = model.ParseData<Exception>();
                        await MixLogService.LogExceptionAsync(ex);
                        break;

                    case MixQueueActions.QueueFailed:
                        var queueEx = model.ParseData<MessageQueueModel>();
                        if (queueEx != null)
                        {
                            queueEx.Sender = model.Sender;
                            await _queueMessageLogService.FailedQueueMessage(queueEx);
                        }
                        break;

                    case MixQueueActions.DeadLetter:
                        var deadLetterMsg = model.ParseData<MessageQueueModel>();

                        if (deadLetterMsg != null)
                        {
                            deadLetterMsg.Sender = model.Sender;

                            await _queueMessageLogService.DeadLetterMessageAsync(deadLetterMsg);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                await MixLogService.LogExceptionAsync(ex);
                await SendMessage(model.Action, false, ex);
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
            await _portalHub.SendMessageAsync(msg);
        }
    }
}