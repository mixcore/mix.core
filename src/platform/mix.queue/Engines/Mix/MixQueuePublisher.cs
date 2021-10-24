using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Mix.Queue.Models.QueueSetting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mix.Queue.Engines.MixQueue
{
    internal class MixQueuePublisher<T> : IQueuePublisher<T>
        where T: MessageQueueModel
    {
        private MixMemoryMessageQueue<T> _queue;
        private MixTopicModel _topic;
        private readonly MixQueueSetting _queueSetting;

        public MixQueuePublisher(QueueSetting queueSetting, string topicName, MixMemoryMessageQueue<T> queue)
        {
            _queueSetting = queueSetting as MixQueueSetting;
            _queue = queue;
            InitializeQueue(topicName);
        }

        private void InitializeQueue(string topicId)
        {
            if (_topic == null)
            {
                // First create a topic.
                _topic = _queue.GetTopic(topicId);
            }
        }

        public async Task SendMessage(T message)
        {
            _topic.PushQueue(message);
        }

        public async Task SendMessages(IList<T> messages)
        {
            var publishTasks =
               messages.Select(async message =>
               {
                   await SendMessage(message);
               });
            await Task.WhenAll(publishTasks);
        }
    }
}
