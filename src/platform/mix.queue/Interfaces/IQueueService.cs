using Mix.Shared.Enums;
using System.Collections.Generic;

namespace Mix.Queue.Interfaces
{
    public interface IQueueService<T>
    {
        void PushQueue(T model);

        IList<T> ConsumeQueue(int lenght, string topicId);

        bool Any(string topicId);

        void PushMessage<TModel>(TModel data, string action, bool status);
    }
}
