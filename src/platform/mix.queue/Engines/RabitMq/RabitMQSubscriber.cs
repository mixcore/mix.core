using Google.Cloud.PubSub.V1;
using Microsoft.Extensions.ObjectPool;
using Mix.Mq.Lib.Models;
using Mix.Queue.Interfaces;
using Mix.Queue.Models.QueueSetting;
using Newtonsoft.Json;
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
        private EventingBasicConsumer _consumer;
        private string _topicId;
        private DefaultObjectPool<IModel> _objectPool;
        private IModel _channel;

        public RabitMQSubscriber(
            IPooledObjectPolicy<IModel> objectPolicy,
            string topicId,
            string subscriptionId,
            Func<T, Task> messageHandler)
        {
            _messageHandler = messageHandler;
            InitializeQueue(objectPolicy, topicId, subscriptionId);
        }

        private void InitializeQueue(IPooledObjectPolicy<IModel> objectPolicy, string topicId, string subscriptionId)
        {
            SubscriptionId = subscriptionId;
            _topicId = topicId;
            _objectPool = new DefaultObjectPool<IModel>(objectPolicy, Environment.ProcessorCount * 2);
            _channel = _objectPool.Get();
            _channel.QueueDeclare(queue: _topicId,
                     durable: true,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);
            _channel.ExchangeDeclare(exchange: topicId, type: ExchangeType.Topic);
            var queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queue: queueName,
                              exchange: _topicId,
                              routingKey: subscriptionId);
            _channel.BasicQos(0, 1, false);
            _consumer = new EventingBasicConsumer(_channel);
            _consumer.Received += (ch, ea) =>
            {
                // received message  
                string body = System.Text.Encoding.UTF8.GetString(ea.Body.ToArray());
                var msg = JsonConvert.DeserializeObject<T>(body);
                Console.WriteLine(msg);
                _messageHandler(msg);
                _channel.BasicAck(ea.DeliveryTag, false);
            };
            _channel.BasicConsume(_topicId, false, _consumer);
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
