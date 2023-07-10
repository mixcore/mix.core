using Google.Cloud.PubSub.V1;
using Mix.Queue.Engines.Mix;
using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Mix.Queue.Models.QueueSetting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mix.Queue.Engines.MixQueue
{
    public class MixQueuePublisher<T> : IQueuePublisher<T>
        where T : MessageQueueModel
    {
        private readonly MixQueueMessages<T> _queue;
        private MixTopicModel<T> _topic;
        private string _topicId;

        public MixQueuePublisher(QueueSetting queueSetting, string topicName, MixQueueMessages<T> queue)
        {
            _queue = queue;
            _topicId = topicName;
            //InitializeQueue(topicName);
        }

        private void InitializeQueue(string topicId)
        {
            if (_topic == null)
            {
                // First create a topic.
                _topic = _queue.GetTopic(topicId);
            }
        }

        public Task SendMessage(T message)
        {
            message.Id = Guid.NewGuid();
            _queue.GetTopic(_topicId).PushQueue(message);
            return Task.CompletedTask;
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
