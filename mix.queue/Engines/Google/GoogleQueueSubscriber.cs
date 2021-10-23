using Google.Apis.Auth.OAuth2;
using Google.Cloud.PubSub.V1;
using Grpc.Auth;
using Grpc.Core;
using Mix.Queue.Interfaces;
using Mix.Queue.Models.QueueSetting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mix.Queue.Engines.GooglePubSub
{
    internal class GoogleQueueSubscriber : IQueueSubscriber
    {
        private SubscriberClient _subscriber;
        private readonly GoogleQueueSetting _queueSetting;
        private SubscriptionName _subscriptionName;
        private readonly Func<string, Task> _messageHandler;

        public GoogleQueueSubscriber(
            QueueSetting queueSetting, 
            string topicId,
            string subscriptionId,
            Func<string, Task> messageHandler)
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
                var createSettings = new SubscriberClient.ClientCreationSettings(
                    credentials: googleCredential.ToChannelCredentials());
                var subscriber = SubscriberClient.CreateAsync(_subscriptionName, createSettings);
                _subscriber = subscriber.Result;
            }
        }


        private Subscription CreateSubscription( string topicId, string subscriptionId)
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
        public async Task ProcessQueue()
        {
            await _subscriber.StartAsync(
                 (PubsubMessage message, CancellationToken cancel) =>
                 {
                     string body = System.Text.Encoding.UTF8.GetString(message.Data.ToArray());

                     _messageHandler(body);

                     return Task.FromResult(SubscriberClient.Reply.Ack);
                 });
        }
    }
}
