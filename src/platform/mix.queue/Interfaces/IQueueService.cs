using Mix.Queue.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Mix.Queue.Interfaces
{
    public interface IQueueService<T>
    {
        void PushQueue(T model);
        void PushQueue(int tenantId, string topicId, string action, object data);

        IList<T> ConsumeQueue(int length, string topicId);

        bool Any(string topicId);

        void PushMessage<TModel>(int tenantId, TModel data, string action, bool status);
        ConcurrentQueue<MessageQueueModel> GetQueue(string topicId);
    }
}
