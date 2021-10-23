using Google.Apis.Auth.OAuth2;
using Google.Cloud.PubSub.V1;
using Grpc.Auth;
using Grpc.Core;
using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Mix.Queue.Models.QueueSetting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mix.Queue.Engines.GooglePubSub
{
    internal class MixQueueSubscriber : IQueueSubscriber
    {
        private readonly MixQueueSetting _queueSetting;
        private SubscriptionName _subscriptionName;
        private readonly Func<QueueMessageModel, Task> _messageHandler;
        private readonly IQueueService<QueueMessageModel> _queueService;
        public MixQueueSubscriber(
            QueueSetting queueSetting,
            string topicId,
            string subscriptionId,
            Func<QueueMessageModel, Task> messageHandler, 
            IQueueService<QueueMessageModel> queueService)
        {
            _queueSetting = queueSetting as MixQueueSetting;
            _messageHandler = messageHandler;
            InitializeQueue(topicId, subscriptionId);
            _queueService = queueService;
        }

        private void InitializeQueue(string topicId, string subscriptionId)
        {
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
                    var inQueueItems = _queueService.ConsumeQueue(10);

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
