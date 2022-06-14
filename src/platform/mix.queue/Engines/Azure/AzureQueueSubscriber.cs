using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Mix.Queue.Models.QueueSetting;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mix.Queue.Engines.Azure
{
    internal class AzureQueueSubscriber : IQueueSubscriber
    {
        private ServiceBusProcessor _subscriber;
        private static ServiceBusAdministrationClient _adminClient;
        private static ServiceBusClient _client;
        private readonly AzureQueueSetting _queueSetting;
        private readonly Func<MessageQueueModel, Task> _messageHandler;
        public AzureQueueSubscriber(
            QueueSetting queueSetting,
            string topicId,
            string subscriptionId,
            Func<MessageQueueModel, Task> messageHandler)
        {
            _queueSetting = queueSetting as AzureQueueSetting;
            _messageHandler = messageHandler;
            InitializeQueue(topicId, subscriptionId);
        }

        private void InitializeQueue(string topicId, string subscriptionId)
        {
            if (_subscriber == null)
            {
                _adminClient = new ServiceBusAdministrationClient(_queueSetting.ConnectionString);
                _client = new ServiceBusClient(_queueSetting.ConnectionString);

                CreateSubscription(topicId, subscriptionId).GetAwaiter().GetResult();

            }
        }


        private async Task CreateSubscription(string topicId, string subscriptionId)
        {
            _adminClient = new ServiceBusAdministrationClient(_queueSetting.ConnectionString);
            if (!await _adminClient.TopicExistsAsync(topicId))
                await _adminClient.CreateTopicAsync(topicId);
            if (!await _adminClient.SubscriptionExistsAsync(topicId, subscriptionId))
                await _adminClient.CreateSubscriptionAsync(topicId, subscriptionId);

            _subscriber = _client.CreateProcessor(topicId, subscriptionId, new ServiceBusProcessorOptions());

            // add handler to process messages
            _subscriber.ProcessMessageAsync += MessageHandler;

            // add handler to process any errors
            _subscriber.ProcessErrorAsync += ErrorHandler;

            // start processing 
            await _subscriber.StartProcessingAsync();
        }

        async Task MessageHandler(ProcessMessageEventArgs args)
        {
            string body = args.Message.Body.ToString();
            var msg = JsonConvert.DeserializeObject<MessageQueueModel>(body);
            await _messageHandler(msg);
            // complete the message. messages is deleted from the subscription. 
            await args.CompleteMessageAsync(args.Message);
        }

        // handle any errors when receiving messages
        Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
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
