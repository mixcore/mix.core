using System.Collections.Concurrent;
using Mix.Constant.Enums;

namespace Mix.Mq.Lib.Models
{
    public class MixTopicModel<T>
        where T : MessageQueueModel
    {
        public string Id { get; set; }
        public bool IsProcessing { get; set; }
        public List<MixSubscriptionModel> Subscriptions { get; set; } = new();
        public ConcurrentQueue<T> Messages { get; private set; } = new();
        public MixSubscriptionModel CreateSubscription(string subscriptionId)
        {
            var subscription = Subscriptions.Find(m => m.Id == subscriptionId);
            if (subscription == null)
            {
                subscription = new MixSubscriptionModel()
                {
                    Id = subscriptionId,
                    Status = MixQueueMessageLogState.NACK,
                    TopicId = Id
                };
                Subscriptions.Add(subscription);
            }
            return subscription;
        }

        public MixSubscriptionModel GetSubscription(string subscriptionId)
        {
            return Subscriptions.Find(m => m.Id == subscriptionId);
        }

        public bool Any()
        {
            return Messages.Any();
        }

        public IList<T> ConsumeQueue(string subscriptionId, int length)
        {
            var result = new List<T>();
            var subscription = Subscriptions.Find(m => m.Id == subscriptionId);

            if (subscription == null)
            {
                return result;
            }

            if (!Messages.Any())
            {
                return result;
            }

            int i = 1;

            var messages = Messages.Where(m => !m.Subscriptions.Any(s => s.Id == subscriptionId));
            while (i <= length && messages.Any())
            {
                T? data = messages.FirstOrDefault();
                if (data == null)
                {
                    Messages.TryDequeue(out _);
                    continue;
                }
                if (!data!.Subscriptions.Any(m => m.Id == subscription.Id))
                {
                    data.Subscriptions.Add(subscription);
                    result.Add(data);
                }
                if (data.Subscriptions.Count == Subscriptions.Count)
                {
                    Messages.TryDequeue(out _);
                }
                i++;
            }

            return result;
        }

        public void PushQueue(T model)
        {
            if (!Messages.Any(m => m.Id == model.Id))
            {
                Messages.Enqueue(model);
            }
        }
    }
}
