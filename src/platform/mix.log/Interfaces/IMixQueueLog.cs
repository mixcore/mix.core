using Mix.Mq.Lib.Models;

namespace Mix.Log.Lib.Interfaces
{
    public interface IMixQueueLog
    {
        Task AckQueueMessage(MessageQueueModel log);
        Task FailedQueueMessage(MessageQueueModel log);
        public Task EnqueueMessageAsync(MessageQueueModel request);
        Task DeadLetterMessageAsync(MessageQueueModel queueLog);
    }
}
