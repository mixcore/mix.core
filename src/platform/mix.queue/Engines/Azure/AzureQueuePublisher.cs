using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Mix.Mq.Lib.Models;
using Mix.Queue.Interfaces;
using Mix.Queue.Models.QueueSetting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Queue.Engines.Azure
{
    internal class AzureQueuePublisher<T> : IQueuePublisher<T>
        where T : MessageQueueModel
    {
        private ServiceBusSender _publisher;
        private static ServiceBusAdministrationClient _adminClient;
        private static ServiceBusClient _client;
        private readonly AzureQueueSetting _queueSetting;

        public AzureQueuePublisher(QueueSetting queueSetting, string topicName)
        {
            _queueSetting = queueSetting as AzureQueueSetting;
            InitializeQueue(topicName);
        }

        private void InitializeQueue(string topicId)
        {
            if (_publisher == null)
            {
                CreateTopicAsync(topicId).GetAwaiter().GetResult();
                _client = new ServiceBusClient(_queueSetting.ConnectionString);
                _publisher = _client.CreateSender(topicId);
            }
        }

        private async Task CreateTopicAsync(string topicId)
        {
            _adminClient = new ServiceBusAdministrationClient(_queueSetting.ConnectionString);
            if (!await _adminClient.TopicExistsAsync(topicId))
                await _adminClient.CreateTopicAsync(topicId);
        }

        public async Task SendMessage(T message)
        {
            var body = JsonConvert.SerializeObject(message);

            var pubsubMessage = new ServiceBusMessage()
            {
                Body = new(Encoding.UTF8.GetBytes(body))
            };

            await _publisher.SendMessageAsync(pubsubMessage);
        }

        public async Task SendMessages(IList<T> messages)
        {
            var publishTasks =
               messages.Select(async message =>
               {
                   await SendMessage(message);
               });
            await Task.WhenAll(publishTasks);
        }
    }
}
