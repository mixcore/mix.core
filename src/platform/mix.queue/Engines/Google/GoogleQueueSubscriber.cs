using Google.Apis.Auth.OAuth2;
using Google.Cloud.PubSub.V1;
using Grpc.Auth;
using Grpc.Core;
using Mix.Mq.Lib.Models;
using Mix.Queue.Interfaces;
using Mix.Queue.Models.QueueSetting;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mix.Queue.Engines.GooglePubSub
{
    internal class GoogleQueueSubscriber<T> : IQueueSubscriber
        where T : MessageQueueModel
    {
        public string SubscriptionId { get; set; }
        private SubscriberClient _subscriber;
        private readonly GoogleQueueSetting _queueSetting;
        private SubscriptionName _subscriptionName;
        private readonly Func<T, Task> _messageHandler;

        public GoogleQueueSubscriber(
            IQueueSetting queueSetting,
            string topicId,
            string subscriptionId,
            Func<T, Task> messageHandler)
        {
            _queueSetting = queueSetting as GoogleQueueSetting;
            _messageHandler = messageHandler;
            InitializeQueue(topicId, subscriptionId);
        }

        private void InitializeQueue(string topicId, string subscriptionId)
        {
            if (_subscriber == null)
            {
                CreateSubscription(topicId, subscriptionId);
                _subscriptionName = new SubscriptionName(_queueSetting.ProjectId, subscriptionId);
                var googleCredential = GoogleCredential.FromFile(_queueSetting.CredentialFile);

                var builder = new SubscriberClientBuilder
                {
                    Credential = googleCredential,
                    SubscriptionName = _subscriptionName
                };

                _subscriber = builder.Build();
            }
        }


        private Subscription CreateSubscription(string topicId, string subscriptionId)
        {
            SubscriberServiceApiClientBuilder builder = new();
            builder.CredentialsPath = _queueSetting.CredentialFile;
            SubscriberServiceApiClient subscriber = builder.Build();
            TopicName topicName = TopicName.FromProjectTopic(_queueSetting.ProjectId, topicId);

            SubscriptionName subscriptionName = SubscriptionName.FromProjectSubscription(_queueSetting.ProjectId, subscriptionId);
            Subscription subscription = null;

            try
            {
                subscription = subscriber.CreateSubscription(subscriptionName, topicName, pushConfig: null, ackDeadlineSeconds: 60);
            }
            catch (RpcException e) when (e.Status.StatusCode == StatusCode.AlreadyExists)
            {
                // Already exists.  That's fine.
            }
            return subscription;
        }

        /// <summary>
        /// Process message queue
        /// </summary>
        /// <returns></returns>
        public async Task ProcessQueue(CancellationToken cancel)
        {
            await _subscriber.StartAsync(
                 (PubsubMessage message, CancellationToken cancel) =>
                 {
                     string body = System.Text.Encoding.UTF8.GetString(message.Data.ToArray());
                     var msg = JsonConvert.DeserializeObject<T>(body);
                     _messageHandler(msg);

                     return Task.FromResult(SubscriberClient.Reply.Ack);
                 });
        }
    }
}
