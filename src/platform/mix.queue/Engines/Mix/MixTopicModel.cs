using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Google.Type;
using Mix.Queue.Models;
using DateTime = System.DateTime;

namespace Mix.Queue.Engines.Mix
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
                    Status = MixQueueMessageStatus.Nack,
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
            subscription.Status = MixQueueMessageStatus.Ack;

            if (!Messages.Any())
            {
                return result;
            }

            int i = 1;


            while (i <= length && Messages.Any())
            {
                T data = Messages.First();
                if (!data.Subscriptions.Any(m => m.Id == subscription.Id))
                {
                    data.Subscriptions.Add(subscription);
                    result.Add(data);
                }
                if (data.Subscriptions.Count == Subscriptions.Count || data.CreatedDate.AddMinutes(1) < DateTime.UtcNow)
                {
                    //_messages.TryDequeue(out _);
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
