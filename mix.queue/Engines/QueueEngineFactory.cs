using Mix.Queue.Engines.Google;
using Mix.Queue.Interfaces;
using Mix.Queue.Models.QueueSetting;
using Mix.Shared.Enums;
using System;

namespace Mix.Queue.Engines
{
    public class QueueEngineFactory
    {
        public static IQueuePublisher<T> CreateGooglePublisher<T>(MixQueueProvider provider, QueueSetting queueSetting, string topicName)
        {
            IQueuePublisher<T> publisher = default;
            switch (provider)
            {
                case MixQueueProvider.GOOGLE:
                    publisher = new GoogleQueuePublisher<T>(queueSetting, topicName);
                    break;
            }
            return publisher;
        }

        public static IQueueSubscriber CreateGoogleSubscriber(MixQueueProvider provider, QueueSetting queueSetting, string subscriptionName, Action<string> handler)
        {
            IQueueSubscriber subscriber = default;
            switch (provider)
            {
                case MixQueueProvider.GOOGLE:
                    subscriber = new GoogleQueueSubscriber(queueSetting, subscriptionName, handler);
                    break;
            }
            return subscriber;
        }
    }
}
