using Mix.Shared.Enums;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Mix.Queue.Models
{
    public class MixTopicModel
    {
        public string Id { get; set; }
        public List<MixSubscribtionModel> Subscriptions { get; set; } = new();
        private ConcurrentQueue<MessageQueueModel> Messages = new();

        public MixSubscribtionModel CreateSubscription(string subscriptionId)
        {
            var subscription = Subscriptions.Find(m => m.Id == subscriptionId);
            if (subscription == null)
            {
                subscription = new MixSubscribtionModel()
                {
                    Id = subscriptionId,
                    Status = MixQueueMessageStatus.Nack,
                    TopicId = Id
                };
                Subscriptions.Add(subscription);
            }
            return subscription;
        }

        public MixSubscribtionModel GetSubscription(string subscriptionId)
        {
            return Subscriptions.Find(m => m.Id == subscriptionId);
        }

        public bool Any()
        {
            return Messages.Any();
        }

        public IList<MessageQueueModel> ConsumeQueue(string subscriptionId, int lenght)
        {
            List<MessageQueueModel> result = new List<MessageQueueModel>();
            var subscription = Subscriptions.Find(m => m.Id == subscriptionId);
            subscription.Status = MixQueueMessageStatus.Ack;

            if (subscription == null)
            {
                return result;
            }
            if (!Messages.Any())
                return result;

            int i = 1;


            while (i <= lenght && Messages.Any(m => m.TopicId == subscription.TopicId))
            {
                MessageQueueModel data = Messages.First(m => m.TopicId == subscription.TopicId);
                data.Subscriptions.Add(subscription);
                if (data.Subscriptions.Count == Subscriptions.Count)
                {
                    Messages.TryDequeue(out MessageQueueModel removeData);
                }

                if (data != null)
                    result.Add(data);
                i++;
            }
            return result;
        }

        public void PushQueue(MessageQueueModel model)
        {
            Messages.Enqueue(model);
        }
    }
}
