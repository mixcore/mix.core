using Microsoft.Extensions.ObjectPool;
using Mix.Mq.Lib.Models;
using Mix.Queue.Interfaces;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Queue.Engines.RabitMQ
{
    public class RabitMQPublisher<T> : IQueuePublisher<T>
        where T : MessageQueueModel
    {
        private DefaultObjectPool<IModel> _objectPool;
        private string _topicId;
        public RabitMQPublisher(IPooledObjectPolicy<IModel> objectPolicy, string topicId)
        {
            _topicId = topicId;
            InitializeQueue(objectPolicy);
        }

        private void InitializeQueue(IPooledObjectPolicy<IModel> objectPolicy)
        {
            _objectPool = new DefaultObjectPool<IModel>(objectPolicy, Environment.ProcessorCount * 2);
        }

        public Task SendMessage(T message)
        {
            var channel = _objectPool.Get();
            try
            {
                var sendBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                channel.BasicPublish(exchange: _topicId,
                     routingKey: $"{_topicId}",
                     basicProperties: properties,
                     body: sendBytes);
                return Task.CompletedTask;
            }
            catch
            {
                throw;
            }
            finally
            {
                _objectPool.Return(channel);
            }
        }

        public Task SendMessages(IList<T> messages)
        {
            List<Task> tasks = new List<Task>();
            foreach (var item in messages)
            {
                tasks.Add(SendMessage(item));
            }
            return Task.WhenAll(tasks);
        }
    }
}
