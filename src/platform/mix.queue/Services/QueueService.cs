﻿using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Mix.Shared.Enums;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Mix.Queue.Services
{
    public class QueueService : IQueueService<MessageQueueModel>
    {
        private Dictionary<string, ConcurrentQueue<MessageQueueModel>> _queues;

        public QueueService()
        {
            _queues = new Dictionary<string, ConcurrentQueue<MessageQueueModel>>();
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
            if (!_queue.Any(m => m.FullName == topicId))
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
            if (_queues.ContainsKey(topicId))
            {
                return _queues[topicId];
            }
            _queues.Add(topicId, new ConcurrentQueue<MessageQueueModel>());
            return _queues[topicId];
        }

        public void PushQueue(MessageQueueModel model)
        {
            var _queue = GetQueue(model.FullName);
            _queue.Enqueue(model);
        }

        public void PushMessage<T>(T data, MixRestAction action, MixRestStatus status)
        {
            var msg = new MessageQueueModel()
            {
                Action = action,
                Status = status,
            };
            msg.Package(data);
            PushQueue(msg);
        }
    }
}