using Microsoft.Extensions.Hosting;
using Mix.Queue.Engines.Mix;
using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Mix.Queue.Models.QueueSetting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mix.Queue.Engines.MixQueue
{
    public class MixQueueSubscriber<T> : BackgroundService, IQueueSubscriber
        where T : MessageQueueModel
    {
        private readonly string _subscriptionId;
        private MixTopicModel<T> _topic;
        private readonly MixQueueSetting _queueSetting;
        private readonly Func<T, Task> _messageHandler;
        private readonly MixQueueMessages<T> _queue;
        protected bool Processing { get; private set; }
        public MixQueueSubscriber(
            QueueSetting queueSetting,
            string topicId,
            string subscriptionId,
            Func<T, Task> messageHandler,
            MixQueueMessages<T> queue)
        {
            _queueSetting = queueSetting as MixQueueSetting;
            _queue = queue;
            _subscriptionId = subscriptionId;
            _messageHandler = messageHandler;
            Initialize(topicId, subscriptionId);

        }

        private void Initialize(string topicId, string subscriptionId)
        {
            while (_topic == null)
            {
                _topic = _queue.GetTopic(topicId);
                Thread.Sleep(1000);
            }
            _topic.CreateSubscription(subscriptionId);
        }

        /// <summary>
        /// Process message queue
        /// </summary>
        /// <returns></returns>
        public Task ProcessQueue(CancellationToken cancellationToken = default)
        {
            Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    if (!Processing)
                    {
                        Processing = true;
                        _topic = _queue.GetTopic(_topic.Id);
                        var inQueueItems = _topic.ConsumeQueue(_subscriptionId, 10);
                        if (inQueueItems.Count > 0)
                        {
                            foreach (var msg in inQueueItems)
                            {
                                await _messageHandler.Invoke(msg);
                            }
                        }
                        Processing = false;
                        await Task.Delay(1000, cancellationToken);
                    }
                }
            }, cancellationToken);


            return Task.CompletedTask;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return ProcessQueue(stoppingToken);
        }
    }
}
