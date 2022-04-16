using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Mix.Queue.Services
{
    public class QueueService : IQueueService<MessageQueueModel>
    {
        private ConcurrentDictionary<string, ConcurrentQueue<MessageQueueModel>> _queues;
        public QueueService()
        {
            _queues = new ConcurrentDictionary<string, ConcurrentQueue<MessageQueueModel>>();
        }

        public bool Any(string topicId)
        {
            var _queue = GetQueue(topicId);
            return _queue.Any();
        }

        public IList<MessageQueueModel> ConsumeQueue(int lenght, string topicId)
        {
            var _queue = GetQueue(topicId);
            List<MessageQueueModel> result = new List<MessageQueueModel>();
            if (!_queue.Any(m => m.TopicId == topicId))
                return result;

            int i = 1;

            while (i <= lenght && _queue.Any())
            {
                _queue.TryDequeue(out MessageQueueModel data);
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
            return _queues.ContainsKey(topicId) ? _queues[topicId] : default;
        }

        public void PushQueue(MessageQueueModel model)
        {
            var _queue = GetQueue(model.TopicId);
            if (_queue != null)
            {
                _queue.Enqueue(model);
            }
        }

        public void PushMessage<T>(T data, string action, bool success)
        {
            var msg = new MessageQueueModel()
            {
                Action = action,
                Success = success,
            };
            msg.Package(data);
            PushQueue(msg);
        }
    }
}
