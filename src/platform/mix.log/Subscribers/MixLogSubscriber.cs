using Microsoft.Extensions.Configuration;
using Mix.Constant.Constants;
using Mix.Constant.Enums;
using Mix.Database.Entities.Queue;
using Mix.Heart.Enums;
using Mix.Heart.Extensions;
using Mix.Heart.Helpers;
using Mix.Log.Lib.Commands;
using Mix.Log.Lib.Interfaces;
using Mix.Log.Lib.Models;
using Mix.Queue.Engines;
using Mix.Queue.Engines.MixQueue;
using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Mix.Service.Services;
using Mix.SignalR.Enums;
using Mix.SignalR.Interfaces;
using Mix.SignalR.Models;
using Newtonsoft.Json.Linq;

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
            MixQueueMessages<MessageQueueModel> mixQueueService,
            IPortalHubClientService portalHub,
            IQueueService<MessageQueueModel> queueService,
            IMixQueueLog queueMessageLogService,
            IAuditLogService auditLogService)
            : base(TopicId, nameof(MixLogSubscriber), 20, serviceProvider, configuration, mixQueueService, queueService)
        {
            _queueMessageLogService = queueMessageLogService;
            _portalHub = portalHub;
            _auditLogService = auditLogService;
        }

        public override Task StartAsync(CancellationToken cancellationToken = default)
        {
            Task.Run(async () =>
            {
                while (_portalHub.Connection == null || _portalHub.Connection.State != Microsoft.AspNetCore.SignalR.Client.HubConnectionState.Connected)
                {
                    await Task.Delay(5000);
                    await _portalHub.StartConnection();
                }
            });
            return base.StartAsync(cancellationToken);
        }
        public override async Task Handler(MessageQueueModel model)
        {
            if (!allowActions.Contains(model.Action))
            {
                return;
            }

            switch (model.Action)
            {
                case MixQueueActions.AuditLog:
                    var aditLogCmd = model.ParseData<LogAuditLogCommand>();
                    if (aditLogCmd != null)
                    {
                        await _auditLogService.SaveRequestAsync(aditLogCmd.Request);
                    }
                    break;

                case MixQueueActions.EnqueueLog:
                    var queueMessage = model.ParseData<MessageQueueModel>();

                    if (queueMessage != null)
                    {
                        var queueLog = new MixQueueMessageLog()
                        {
                            Id = Guid.NewGuid(),
                            QueueMessageId = queueMessage.Id,
                            CreatedDateTime = DateTime.UtcNow,
                            Action = queueMessage.Action,
                            TopicId = queueMessage.TopicId,
                            DataTypeFullName = queueMessage.DataTypeFullName,
                            State = MixQueueMessageLogState.NACK,
                            Status = MixContentStatus.Published
                        };
                        if (model.Data.IsJsonString())
                        {
                            queueLog.ObjectData = JObject.Parse(model.Data);
                        }
                        else
                        {
                            queueLog.StringData = model.Data;
                        }
                        await _queueMessageLogService.SaveRequestAsync(queueLog);
                    }
                    break;
                case MixQueueActions.AckLog:
                    var ackQueueMessage = model.ParseData<MessageQueueModel>();

                    if (ackQueueMessage != null)
                    {
                        var queueLog = new MixQueueMessageLog()
                        {
                            Id = Guid.NewGuid(),
                            QueueMessageId = ackQueueMessage.Id,
                            CreatedDateTime = DateTime.UtcNow,
                            Action = ackQueueMessage.Action,
                            TopicId = ackQueueMessage.TopicId,
                            DataTypeFullName = ackQueueMessage.DataTypeFullName,
                            SubscriptionId = model.Sender,
                            State = MixQueueMessageLogState.ACK,
                            Status = MixContentStatus.Published
                        };
                        if (model.Data.IsJsonString())
                        {
                            queueLog.ObjectData = JObject.Parse(model.Data);
                        }
                        else
                        {
                            queueLog.StringData = model.Data;
                        }
                        await _queueMessageLogService.SaveRequestAsync(queueLog);
                    }
                    break;

                case MixQueueActions.ExceptionLog:
                    var ex = model.ParseData<Exception>();
                    await MixLogService.LogExceptionAsync(ex);
                    break;

                case MixQueueActions.DeadLetter:
                    var deadLetterMsg = model.ParseData<MessageQueueModel>();

                    if (deadLetterMsg != null)
                    {
                        var queueLog = new MixQueueMessageLog()
                        {
                            Id = Guid.NewGuid(),
                            QueueMessageId = deadLetterMsg.Id,
                            CreatedDateTime = DateTime.UtcNow,
                            Action = deadLetterMsg.Action,
                            TopicId = deadLetterMsg.TopicId,
                            DataTypeFullName = deadLetterMsg.DataTypeFullName,
                            State = MixQueueMessageLogState.DeadLetter,
                            Status = MixContentStatus.Published
                        };
                        if (model.Data.IsJsonString())
                        {
                            queueLog.ObjectData = JObject.Parse(model.Data);
                        }
                        else
                        {
                            queueLog.StringData = model.Data;
                        }
                        await _queueMessageLogService.SaveRequestAsync(queueLog);
                    }
                    break;
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
            await _portalHub.SendMessageAsync(msg);
        }
    }
}