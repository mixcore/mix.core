using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Mix.Queue.Models.QueueSetting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mix.Queue.Engines.MixQueue
{
    internal class MixQueuePublisher<T> : IQueuePublisher<T>
        where T : MessageQueueModel
    {
        private readonly MixQueueMessages<T> _queue;
        private MixTopicModel _topic;

        public MixQueuePublisher(QueueSetting queueSetting, string topicName, MixQueueMessages<T> queue)
        {
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

        public Task SendMessage(T message)
        {
            _topic.PushQueue(message);
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
