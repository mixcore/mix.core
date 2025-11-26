using Google.Cloud.PubSub.V1;
using Microsoft.Extensions.ObjectPool;
using Mix.Heart.Extensions;
using Mix.Mq.Lib.Models;
using Mix.Queue.Interfaces;
using Mix.Queue.Models.QueueSetting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Mix.Queue.Engines.RabitMQ
{
    internal class RabitMQSubscriber<T> : IQueueSubscriber
        where T : MessageQueueModel
    {
        public string SubscriptionId { get; set; }
        private readonly Func<T, Task> _messageHandler;
        private AsyncEventingBasicConsumer _consumer;
        private string _topicId;
        private DefaultObjectPool<IChannel> _objectPool;
        private IChannel _channel;

        public RabitMQSubscriber(
            IPooledObjectPolicy<IChannel> objectPolicy,
            string topicId,
            string subscriptionId,
            Func<T, Task> messageHandler)
        {
            _messageHandler = messageHandler;
            InitializeQueueAsync(objectPolicy, topicId, subscriptionId);
        }

        private async Task InitializeQueueAsync(IPooledObjectPolicy<IChannel> objectPolicy, string topicId, string subscriptionId)
        {
            SubscriptionId = subscriptionId;
            _topicId = topicId;
            _objectPool = new DefaultObjectPool<IChannel>(objectPolicy, Environment.ProcessorCount * 2);
            _channel = _objectPool.Get();
            await _channel.ExchangeDeclareAsync(exchange: topicId, type: ExchangeType.Topic);
            var queueResult = await _channel.QueueDeclareAsync(queue: subscriptionId,
                     durable: true,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);
            await _channel.QueueBindAsync(queue: queueResult.QueueName,
                              exchange: _topicId,
                              routingKey: _topicId);
            await _channel.BasicQosAsync(0, 1, false);
            _consumer = new AsyncEventingBasicConsumer(_channel);
            _consumer.ReceivedAsync += async (ch, ea) =>
            {
                // received message  
                try
                {
                    string body = System.Text.Encoding.UTF8.GetString(ea.Body.ToArray());
                    if (body.IsJsonString())
                    {
                        var msg = JObject.Parse(body).ToObject<T>();
                        _messageHandler(msg);
                        await _channel.BasicAckAsync(ea.DeliveryTag, false);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Cannot process message {subscriptionId}");
                    Console.Error.WriteLine(ex);
                    await _channel.BasicNackAsync(ea.DeliveryTag, false, false);
                }
            };
            await _channel.BasicConsumeAsync(queueResult.QueueName, false, _consumer);
        }

        /// <summary>
        /// Process message queue
        /// </summary>
        /// <returns></returns>
        public Task ProcessQueue(CancellationToken cancel)
        {
            return Task.CompletedTask;

        }
    }
}
