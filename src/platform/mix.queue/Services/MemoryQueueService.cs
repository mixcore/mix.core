using Mix.Heart.Helpers;
using Mix.Mq.Lib.Models;
using Mix.Queue.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Mix.Queue.Services
{
    public class MemoryQueueService : IMemoryQueueService<MessageQueueModel>
    {
        // Memory queue store message to local memory before push to MQ engine
        private readonly ConcurrentDictionary<string, ConcurrentQueue<MessageQueueModel>> _queues;
        public MemoryQueueService()
        {
            _queues = new ConcurrentDictionary<string, ConcurrentQueue<MessageQueueModel>>();
        }

        public bool Any(string topicId)
        {
            var queue = GetMemoryQueue(topicId);
            return queue.Any();
        }

        public IList<MessageQueueModel> ConsumeMemoryQueue(int length, string topicId)
        {
            var queue = GetMemoryQueue(topicId);
            List<MessageQueueModel> result = new();
            if (queue.All(m => m.TopicId != topicId))
            {
                return result;
            }

            int i = 1;

            // Consume memory messages and push to MessageQueue Provider
            while (i <= length && queue.Any())
            {
                queue.TryDequeue(out MessageQueueModel data);
                if (data != null)
                    result.Add(data);
                i++;
            }
            return result;
        }

        public IList<MessageQueueModel> ConsumeAllMemoryQueue(int length)
        {
            List<MessageQueueModel> result = new();
            foreach (var topic in _queues)
            {
                result.AddRange(ConsumeMemoryQueue(length, topic.Key));
            }
            return result;
        }
        public ConcurrentQueue<MessageQueueModel> GetMemoryQueue(string topicId)
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

        public void PushMemoryQueue(MessageQueueModel model)
        {
            var queue = GetMemoryQueue(model.TopicId);
            if (queue != null)
            {
                model.Id = Guid.NewGuid();
                queue.Enqueue(model);
                EnqueueLog(model);
            }

        }

        public void PushMemoryQueue(int tenantId, string topicId, string action, object data)
        {
            var msg = new MessageQueueModel(tenantId, topicId, action, data);
            PushMemoryQueue(msg);
        }

        // Push message to MQ Engine
        public void PushMessageToMemoryQueue<T>(int tenantId, T data, string action, bool success)
        {
            var msg = new MessageQueueModel(tenantId)
            {
                Action = action,
                Success = success,
            };
            msg.Package(data);
            PushMemoryQueue(msg);
        }


        private void EnqueueLog(MessageQueueModel model)
        {
            if (model.TopicId != MixQueueTopics.MixLog)
            {
                var logQueue = GetMemoryQueue(MixQueueTopics.MixLog);
                if (logQueue != null)
                {
                    logQueue.Enqueue(new MessageQueueModel()
                    {
                        TopicId = MixQueueTopics.MixLog,
                        Action = MixQueueActions.EnqueueLog,
                        Data = ReflectionHelper.ParseObject(model).ToString(),
                        TenantId = 1,
                        CreatedDate = DateTime.UtcNow
                    });
                }
            }
        }

    }
}
