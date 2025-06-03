using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.ObjectPool;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Mq.Lib.Models;
using Mix.Mqtt.Lib.Models;
using Mix.Queue.Engines.Azure;
using Mix.Queue.Engines.GooglePubSub;
using Mix.Queue.Engines.MixQueue;
using Mix.Queue.Engines.Mqtt;
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
            MixQueueProvider provider, IConfiguration configuration, string topicId, MixEndpointService mixEndpointService)
            where T : MessageQueueModel
        {
            IQueuePublisher<T> publisher = default;
            switch (provider)
            {
                case MixQueueProvider.AZURE:
                    publisher = new AzureQueuePublisher<T>(configuration.GetSection($"{MixAppSettingsSection.MessageQueueSettings}:AzureServiceBus").Get<AzureQueueSetting>(), topicId);
                    break;

                case MixQueueProvider.GOOGLE:
                    publisher = new GoogleQueuePublisher<T>(configuration.GetSection($"{MixAppSettingsSection.MessageQueueSettings}:GoogleQueueSetting").Get<GoogleQueueSetting>(), topicId);
                    break;

                case MixQueueProvider.MIX:
                    publisher = new MixQueuePublisher<T>(configuration.GetSection($"{MixAppSettingsSection.MessageQueueSettings}:Mix").Get<MixQueueSetting>(), topicId, mixEndpointService);
                    break;
                case MixQueueProvider.MQTT:
                    publisher = new MqttPublisher<T>(configuration.GetSection($"{MixAppSettingsSection.MessageQueueSettings}:MQTT").Get<MQTTSetting>(), topicId, mixEndpointService);
                    break;
            }
            return publisher;
        }

        public static IQueuePublisher<T> CreateRabbitMqPublisher<T>(IPooledObjectPolicy<IChannel> objectPolicy, string topicId)
             where T : MessageQueueModel
        {
            return new RabbitMQPublisher<T>(objectPolicy, topicId);
        }
        #endregion

        #region Subscribers
        public static IQueueSubscriber? CreateSubscriber<T>(
            MixQueueProvider provider,
            IConfiguration configuration,
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
                    subscriber = new AzureQueueSubscriber<T>(configuration.GetSection($"{MixAppSettingsSection.MessageQueueSettings}:AzureServiceBus").Get<AzureQueueSetting>(), topicId, subscriptionId, handler);
                    break;
                case MixQueueProvider.GOOGLE:
                    subscriber = new GoogleQueueSubscriber<T>(configuration.GetSection($"{MixAppSettingsSection.MessageQueueSettings}:GoogleQueueSetting").Get<GoogleQueueSetting>(), topicId, subscriptionId, handler);
                    break;
                case MixQueueProvider.MIX:
                    subscriber = new MixQueueSubscriber<T>(configuration.GetSection($"{MixAppSettingsSection.MessageQueueSettings}:Mix").Get<MixQueueSetting>(), topicId, subscriptionId, handler, memQueues, mixEndpointService);
                    break;
                case MixQueueProvider.MQTT:
                    subscriber = new MqttSubscriber<T>(configuration.GetSection($"{MixAppSettingsSection.MessageQueueSettings}:MQTT").Get<MQTTSetting>(), topicId, handler);
                    break;
            }
            subscriber.SubscriptionId = subscriptionId;
            return subscriber;
        }
        public static IQueueSubscriber CreateRabbitMQSubscriber<T>(IPooledObjectPolicy<IChannel> objectPolicy, string topicId, string subscriptionId, Func<T, Task> handler)
            where T : MessageQueueModel
        {
            var subscriber = new RabbitMQSubscriber<T>(objectPolicy, topicId, subscriptionId, handler);
            subscriber.InitializeQueueAsync(objectPolicy, topicId, subscriptionId).Wait();
            return subscriber;
        }
        #endregion

    }
}
