using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Mix.Queue.Models.QueueSetting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mix.Queue.Engines.MixQueue
{
    internal class MixQueueSubscriber : IQueueSubscriber
    {
        private readonly string _subscriptionId;
        private MixTopicModel _topic;
        private readonly MixQueueSetting _queueSetting;
        private readonly Func<MessageQueueModel, Task> _messageHandler;
        private readonly MixMemoryMessageQueue<MessageQueueModel> _queue;
        public MixQueueSubscriber(
            QueueSetting queueSetting,
            string topicId,
            string subscriptionId,
            Func<MessageQueueModel, Task> messageHandler,
            MixMemoryMessageQueue<MessageQueueModel> queue)
        {
            _queueSetting = queueSetting as MixQueueSetting;
            _queue = queue;
            _subscriptionId = subscriptionId;
            _messageHandler = messageHandler;
            Initialize(topicId, subscriptionId);

        }

        private void Initialize(string topicId, string subscriptionId)
        {
            _topic = _queue.GetTopic(topicId);
            _topic.CreateSubscription(subscriptionId);
        }

        /// <summary>
        /// Process message queue
        /// </summary>
        /// <returns></returns>
        public Task ProcessQueue()
        {
            CancellationToken cancellationToken = new CancellationToken();
            Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    _topic = _queue.GetTopic(_topic.Id);
                    var inQueueItems = _topic.ConsumeQueue(_subscriptionId, 10);

                    if (inQueueItems.Any())
                    {
                        await _messageHandler.Invoke(inQueueItems[0]);
                    }
                }
            }, cancellationToken);


            return Task.CompletedTask;
        }
    }
}
