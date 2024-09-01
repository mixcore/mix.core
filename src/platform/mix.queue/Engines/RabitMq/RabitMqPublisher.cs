using Mix.Mq.Lib.Models;
using Mix.Queue.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using RabbitMQ.Client;
using Mix.Queue.Models.QueueSetting;
using System;
using System.Threading.Channels;
using System.Text;
using Microsoft.Extensions.ObjectPool;
using Google.Api;
using Newtonsoft.Json;

namespace Mix.Queue.Engines.RabitMQ
{
    public class RabitMQPublisher<T> : IQueuePublisher<T>
        where T : MessageQueueModel
    {
        private DefaultObjectPool<IModel> _objectPool;
        private readonly string _topicId;

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
