using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Mix.Queue.Services
{
    public class QueueService : IQueueService<MessageQueueModel>
    {
        private readonly ConcurrentDictionary<string, ConcurrentQueue<MessageQueueModel>> _queues;
        public QueueService()
        {
            _queues = new ConcurrentDictionary<string, ConcurrentQueue<MessageQueueModel>>();
        }

        public bool Any(string topicId)
        {
            var queue = GetQueue(topicId);
            return queue.Any();
        }

        public IList<MessageQueueModel> ConsumeQueue(int length, string topicId)
        {
            var queue = GetQueue(topicId);
            List<MessageQueueModel> result = new();
            if (queue.All(m => m.TopicId != topicId))
            {
                return result;
            }

            int i = 1;

            while (i <= length && queue.Any())
            {
                queue.TryDequeue(out MessageQueueModel data);
                if (data != null)
                    result.Add(data);
                i++;
            }
            return result;
        }

        private ConcurrentQueue<MessageQueueModel> GetQueue(string topicId)
        {
            if (string.IsNullOrEmpty(topicId))
            {
                return default;
            }

            if (!_queues.ContainsKey(topicId))
            {
                _queues.TryAdd(topicId, new ConcurrentQueue<MessageQueueModel>());
            }
            return _queues[topicId];
        }

        public void PushQueue(MessageQueueModel model)
        {
            var queue = GetQueue(model.TopicId);
            if (queue != null)
            {
                queue.Enqueue(model);
            }
        }

        public void PushQueue(int tenantId, string topicId, string action, object data)
        {
            var msg = new MessageQueueModel(tenantId, topicId, action, data);
            PushQueue(msg);
        }

        public void PushMessage<T>(int tenantId, T data, string action, bool success)
        {
            var msg = new MessageQueueModel(tenantId)
            {
                Action = action,
                Success = success,
            };
            msg.Package(data);
            PushQueue(msg);
        }
    }
}
