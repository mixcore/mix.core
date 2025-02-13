using Microsoft.Extensions.ObjectPool;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Mq.Lib.Models;
using Mix.Queue.Engines.Azure;
using Mix.Queue.Engines.GooglePubSub;
using Mix.Queue.Engines.MixQueue;
using Mix.Queue.Engines.RabbitMQ;
using Mix.Queue.Interfaces;
using Mix.Queue.Models.QueueSetting;
using Mix.Shared.Services;
using RabbitMQ.Client;
using System;
using System.Threading.Tasks;

namespace Mix.Queue.Engines
{
    public class QueueEngineFactory
    {
        #region Publishers

        public static IQueuePublisher<T> CreatePublisher<T>(
            MixQueueProvider provider, IQueueSetting queueSetting, string topicId, MixEndpointService mixEndpointService)
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

        public static IQueuePublisher<T> CreateRabbitMqPublisher<T>(IPooledObjectPolicy<IModel> objectPolicy, string topicId)
             where T : MessageQueueModel
        {
            return new RabbitMQPublisher<T>(objectPolicy, topicId);
        }
        #endregion

        #region Subscribers
        public static IQueueSubscriber CreateSubscriber<T>(
            MixQueueProvider provider,
            IQueueSetting queueSetting,
            string topicId,
            string subscriptionId,
            Func<T, Task> handler,
            IMemoryQueueService<MessageQueueModel> memQueues,
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
        public static IQueueSubscriber CreateRabbitMQSubscriber<T>(IPooledObjectPolicy<IModel> objectPolicy, string topicId, string subscriptionId, Func<T, Task> handler)
            where T : MessageQueueModel
        {
            return new RabbitMQSubscriber<T>(objectPolicy, topicId, subscriptionId, handler);
        }
        #endregion

    }
}
