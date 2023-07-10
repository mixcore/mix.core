using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Mix.Queue.Models;

namespace Mix.Queue.Engines.Mix
{
    public class MixTopicModel<T>
        where T : MessageQueueModel
    {
        public string Id { get; set; }
        public List<MixSubscriptionModel> Subscriptions { get; set; } = new();
        private readonly ConcurrentQueue<T> _messages = new();

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
            return _messages.Any();
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

            if (!_messages.Any())
            {
                return result;
            }

            int i = 1;


            while (i <= length && _messages.Any(m => m.TopicId == subscription.TopicId))
            {
                T data = _messages.First(m => m.TopicId == subscription.TopicId);
                if (!data.Subscriptions.Any(m => m.Id == subscription.Id))
                {
                    data.Subscriptions.Add(subscription);
                    result.Add(data);
                }

                if (data.Subscriptions.Count == Subscriptions.Count)
                {
                    _messages.TryDequeue(out _);
                }


                i++;
            }
            return result;
        }

        public void PushQueue(T model)
        {
            if (!_messages.Any(m => m.Id == model.Id))
            {
                _messages.Enqueue(model);
            }
        }
    }
}
