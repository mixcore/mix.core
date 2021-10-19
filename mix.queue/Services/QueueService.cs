using Mix.Queue.Interfaces;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Mix.Queue.Services
{
    public class QueueService<T> : IQueueService<T>
    {
        private ConcurrentQueue<T> _queue = new ConcurrentQueue<T>();

        public QueueService()
        {
            _queue = new ConcurrentQueue<T>();
        }

        public IList<T> ConsumeQueue(int lenght)
        {
            List<T> result = new List<T>();
            if (!_queue.Any())
                return result;

            int i = 1;

            while (i <= lenght && _queue.Any())
            {
                _queue.TryDequeue(out T data);
                if (data != null)
                    result.Add(data);
                i++;
            }
            return result;
        }

        public void PushQueue(T model)
        {
            _queue.Enqueue(model);
        }
    }
}
