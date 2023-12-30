using Mix.Mq.Lib.Models;
using Mix.Queue.Engines.Azure;
using Mix.Queue.Engines.GooglePubSub;
using Mix.Queue.Engines.MixQueue;
using Mix.Queue.Interfaces;
using Mix.Queue.Models.QueueSetting;
using Mix.Shared.Services;
using System;
using System.Threading.Tasks;

namespace Mix.Queue.Engines
{
    public class QueueEngineFactory
    {
        public static IQueuePublisher<T> CreatePublisher<T>(
            MixQueueProvider provider, QueueSetting queueSetting, string topicId, MixEndpointService mixEndpointService)
            where T : MessageQueueModel
        {
            IQueuePublisher<T> publisher = default;
            switch (provider)
            {
                case MixQueueProvider.AZURE:
                    publisher = new AzureQueuePublisher<T>(queueSetting, topicId);
                    break;

                case MixQueueProvider.GOOGLE:
                    publisher = new GoogleQueuePublisher<T>(queueSetting, topicId);
                    break;

                case MixQueueProvider.MIX:
                    publisher = new MixQueuePublisher<T>(queueSetting, topicId, mixEndpointService);
                    break;
            }
            return publisher;
        }

        public static IQueueSubscriber CreateSubscriber<T>(
            MixQueueProvider provider,
            QueueSetting queueSetting,
            string topicId,
            string subscriptionId,
            Func<T, Task> handler,
            IQueueService<MessageQueueModel> memQueues,
            MixEndpointService mixEndpointService)
            where T : MessageQueueModel
        {
            IQueueSubscriber subscriber = default;
            switch (provider)
            {
                case MixQueueProvider.AZURE:
                    subscriber = new AzureQueueSubscriber<T>(queueSetting, topicId, subscriptionId, handler);
                    break;
                case MixQueueProvider.GOOGLE:
                    subscriber = new GoogleQueueSubscriber<T>(queueSetting, topicId, subscriptionId, handler);
                    break;
                case MixQueueProvider.MIX:
                    subscriber = new MixQueueSubscriber<T>(queueSetting, topicId, subscriptionId, handler, memQueues, mixEndpointService);
                    break;
            }
            subscriber.SubscriptionId = subscriptionId;
            return subscriber;
        }

    }
}
