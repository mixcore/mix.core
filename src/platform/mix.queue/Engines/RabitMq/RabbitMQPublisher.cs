using Mix.Mq.Lib.Models;
using Mix.Queue.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using RabbitMQ.Client;
using System;
using System.Text;
using Microsoft.Extensions.ObjectPool;
using Newtonsoft.Json;

namespace Mix.Queue.Engines.RabbitMQ
{
    public class RabbitMQPublisher<T> : IQueuePublisher<T>
        where T : MessageQueueModel
    {
        private readonly string _topicId;
        private readonly DefaultObjectPool<IModel> _objectPool;

        public RabbitMQPublisher(IPooledObjectPolicy<IModel> objectPolicy, string topicId)
        {
            _topicId = topicId;
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

                channel.BasicPublish(
                    exchange: _topicId,
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
            List<Task> tasks = [];
            foreach (var item in messages)
            {
                tasks.Add(SendMessage(item));
            }
            return Task.WhenAll(tasks);
        }
    }
}
