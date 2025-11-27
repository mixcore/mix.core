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
        private readonly DefaultObjectPool<IChannel> _objectPool;

        public RabbitMQPublisher(IPooledObjectPolicy<IChannel> objectPolicy, string topicId)
        {
            _topicId = topicId;
            _objectPool = new DefaultObjectPool<IChannel>(objectPolicy, Environment.ProcessorCount * 2);
        }

        public async Task SendMessage(T message)
        {
            var channel = _objectPool.Get();
            try
            {
                var sendBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

                await channel.BasicPublishAsync(
                    exchange: _topicId,
                    routingKey: $"{_topicId}",
                    true,
                    basicProperties: new BasicProperties() { Persistent = true },
                    body: sendBytes);
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

        public Task StopAsync()
        {
            return Task.CompletedTask;
        }
    }
}
