using Mix.Queue.Engines.GooglePubSub;
using Mix.Queue.Interfaces;
using Mix.Queue.Models.QueueSetting;
using Mix.Shared.Enums;
using System;
using System.Threading.Tasks;

namespace Mix.Queue.Engines
{
    public class QueueEngineFactory
    {
        public static IQueuePublisher<T> CreateGooglePublisher<T>(
            MixQueueProvider provider, QueueSetting queueSetting, string topicId)
        {
            IQueuePublisher<T> publisher = default;
            switch (provider)
            {
                case MixQueueProvider.GOOGLE:
                    publisher = new GoogleQueuePublisher<T>(queueSetting, topicId);
                    break;
            }
            return publisher;
        }

        public static IQueueSubscriber CreateGoogleSubscriber(
            MixQueueProvider provider,
            QueueSetting queueSetting,
            string topicId,
            string subscriptionId,
            Func<string, Task> handler)
        {
            IQueueSubscriber subscriber = default;
            switch (provider)
            {
                case MixQueueProvider.GOOGLE:
                    subscriber = new GoogleQueueSubscriber(queueSetting, topicId, subscriptionId, handler);
                    break;
            }
            return subscriber;
        }

    }
}
