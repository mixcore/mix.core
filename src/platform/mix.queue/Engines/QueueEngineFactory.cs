using Mix.Queue.Engines.GooglePubSub;
using Mix.Queue.Engines.MixQueue;
using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Mix.Queue.Models.QueueSetting;

using System;
using System.Threading.Tasks;

namespace Mix.Queue.Engines
{
    public class QueueEngineFactory
    {
        public static IQueuePublisher<T> CreatePublisher<T>(
            MixQueueProvider provider, QueueSetting queueSetting, string topicId,
            MixMemoryMessageQueue<T> queue = null)
            where T : MessageQueueModel
        {
            IQueuePublisher<T> publisher = default;
            switch (provider)
            {
                case MixQueueProvider.GOOGLE:
                    publisher = new GoogleQueuePublisher<T>(queueSetting, topicId);
                    break;

                case MixQueueProvider.MIX:
                    publisher = new MixQueuePublisher<T>(queueSetting, topicId, queue);
                    break;
            }
            return publisher;
        }

        public static IQueueSubscriber CreateSubscriber(
            MixQueueProvider provider,
            QueueSetting queueSetting,
            string topicId,
            string subscriptionId,
            Func<MessageQueueModel, Task> handler,
            MixMemoryMessageQueue<MessageQueueModel> queueService)
        {
            IQueueSubscriber subscriber = default;
            switch (provider)
            {
                case MixQueueProvider.GOOGLE:
                    subscriber = new GoogleQueueSubscriber(queueSetting, topicId, subscriptionId, handler);
                    break;
                case MixQueueProvider.MIX:
                    subscriber = new MixQueueSubscriber(queueSetting, topicId, subscriptionId, handler, queueService);
                    break;
            }
            return subscriber;
        }

    }
}
