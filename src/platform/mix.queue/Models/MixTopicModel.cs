﻿using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Mix.Mq.Lib.Models
{
    public class MixTopicModel<T>
        where T : MessageQueueModel
    {
        public string Id { get; set; }
        public bool IsProcessing { get; set; }
        public ConcurrentDictionary<string, MixSubscriptionModel> Subscriptions { get; set; } = new();
        public ConcurrentQueue<T> Messages { get; private set; } = new();
        public MixSubscriptionModel CreateSubscription(string subscriptionId)
        {
            if (!Subscriptions.TryGetValue(subscriptionId, out var subscription))
            {
                subscription = new MixSubscriptionModel()
                {
                    Id = subscriptionId,
                    Status = MixQueueMessageLogState.NACK,
                    TopicId = Id
                };
                Subscriptions.TryAdd(subscriptionId, subscription);
            }
            return subscription;
        }

        public MixSubscriptionModel GetSubscription(string subscriptionId)
        {
            return Subscriptions.GetValueOrDefault(subscriptionId);
        }

        public bool RemoveSubscription(string subscriptionId)
        {
            return Subscriptions.TryRemove(subscriptionId, out _);
        }

        public bool Any()
        {
            return Messages.Any();
        }

        public IList<T> ConsumeQueue(string subscriptionId, int length)
        {
            var result = new List<T>();

            if (Messages.FirstOrDefault() == null)
            {
                Messages.TryDequeue(out _);
            }

            if (!Messages.Any())
            {
                return result;
            }

            if (!Subscriptions.TryGetValue(subscriptionId, out var subscription))
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
                    continue;
                }
                if (!data!.Subscriptions.Any(m => m.Id == subscription.Id))
                {
                    data.Subscriptions.Add(subscription);
                    result.Add(data);
                }
            }

            while (Messages.FirstOrDefault()?.Subscriptions.Count == Subscriptions.Count)
            {
                Messages.TryDequeue(out _);
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
