using Microsoft.Extensions.Configuration;
using Mix.Queue.Engines;
using Mix.Queue.Engines.MixQueue;

namespace Mix.Lib.Subscribers
{
    public class MixBackgroundTaskSubscriber : SubscriberBase
    {
        protected AuditLogService AuditLogService;
        private const string TopicId = MixQueueTopics.MixBackgroundTasks;

        public MixBackgroundTaskSubscriber(
            IConfiguration configuration,
            MixMemoryMessageQueue<MessageQueueModel> queueService,
            AuditLogService auditLogService)
            : base(TopicId, string.Empty, configuration, queueService)
        {
            AuditLogService = auditLogService;
        }

        public override Task Handler(MessageQueueModel model)
        {
            switch (model.Action)
            {
                case MixQueueActions.AuditLog:
                    var cmd = model.ParseData<LogAuditLogCommand>();
                    AuditLogService.SaveToDatabase(cmd.UserName, cmd.Request, true, null);
                    break;
            }
            return Task.CompletedTask;
        }
    }
}
