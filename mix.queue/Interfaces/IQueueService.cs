using System.Collections.Generic;

namespace Mix.Queue.Interfaces
{
    public interface IQueueService<T>
    {
        void PushQueue(T model);

        IList<T> ConsumeQueue(int lenght);
    }
}
