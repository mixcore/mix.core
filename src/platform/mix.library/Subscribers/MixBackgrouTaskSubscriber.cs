using Microsoft.Extensions.Configuration;
using Mix.Lib.Services;
using Mix.Queue.Engines;
using Mix.Queue.Engines.MixQueue;
using Mix.Shared.Commands;

namespace Mix.Lib.Subscribers
{
    public class MixBackgrouTaskSubscriber : SubscriberBase
    {
        protected AuditLogService _auditLogService;
        private static string topicId = MixQueueTopics.MixBackgroundTasks;
        public MixBackgrouTaskSubscriber(
            IConfiguration configuration,
            MixMemoryMessageQueue<MessageQueueModel> queueService, 
            AuditLogService auditLogService)
            : base(topicId, string.Empty, configuration, queueService)
        {
            _auditLogService = auditLogService;
        }

        public override Task Handler(MessageQueueModel model)
        {
            switch (model.Action)
            {
                case MixQueueActions.AuditLog:
                    var cmd = model.ParseData<LogAuditLogCommand>();
                    _auditLogService.SaveToDatabase(cmd.UserName, cmd.Request, true, null);
                    break;
            }
            return Task.CompletedTask;
        }
    }
}
