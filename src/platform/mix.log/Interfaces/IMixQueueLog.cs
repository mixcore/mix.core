using Microsoft.AspNetCore.Http;
using Mix.Database.Entities.Queue;
using Mix.Queue.Models;
using Mix.Service.Models;
using Newtonsoft.Json.Linq;

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
