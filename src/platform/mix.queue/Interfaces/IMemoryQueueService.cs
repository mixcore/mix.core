using Mix.Mq.Lib.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Mix.Queue.Interfaces
{
    public interface IMemoryQueueService<T>
    {
        void PushMemoryQueue(T model);

        void PushMemoryQueue(int tenantId, string topicId, string action, object data);

        IList<T> ConsumeMemoryQueue(int length, string topicId);

        IList<T> ConsumeAllMemoryQueue(int length);

        bool Any(string topicId);

        void PushMessageToMemoryQueue<TModel>(int tenantId, TModel data, string action, bool status);

        ConcurrentQueue<MessageQueueModel> GetMemoryQueue(string topicId);
    }
}
