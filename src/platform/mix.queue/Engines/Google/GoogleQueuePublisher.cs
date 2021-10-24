using Google.Api.Gax.Grpc;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.PubSub.V1;
using Google.Protobuf;
using Grpc.Auth;
using Grpc.Core;
using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Mix.Queue.Models.QueueSetting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mix.Queue.Engines.GooglePubSub
{
    internal class GoogleQueuePublisher<T> : IQueuePublisher<T>
        where T : MessageQueueModel
    {
        private PublisherClient _publisher;
        private readonly GoogleQueueSetting _queueSetting;

        public GoogleQueuePublisher(QueueSetting queueSetting, string topicName)
        {
            _queueSetting = queueSetting as GoogleQueueSetting;
            InitializeQueue(topicName);
        }

        private void InitializeQueue(string topicName)
        {
            if (_publisher == null)
            {
                // First create a topic.
                CreateTopic(topicName);

                var googleCredential = GoogleCredential.FromFile(_queueSetting.CredentialFile);
                var createSettings = new PublisherClient.ClientCreationSettings(credentials: googleCredential.ToChannelCredentials());
                var toppicName = new TopicName(_queueSetting.ProjectId, topicName);
                var publisher = PublisherClient.CreateAsync(toppicName, createSettings);
                _publisher = publisher.Result;
            }
        }

        private Topic CreateTopic(string topicId)
        {
            try
            {
                PublisherServiceApiClientBuilder builder = new PublisherServiceApiClientBuilder();
                builder.CredentialsPath = _queueSetting.CredentialFile;
                PublisherServiceApiClient publisher = builder.Build();
                TopicName topicName = new TopicName(_queueSetting.ProjectId, topicId);
                return publisher.CreateTopic(topicName);
            }
            catch (RpcException e) when (e.Status.StatusCode == StatusCode.AlreadyExists)
            {
                // Already exists.  That's fine.
                return default;
            }
        }

        public async Task SendMessage(T message)
        {
            var body = JsonConvert.SerializeObject(message);

            var attributes = new Dictionary<string, string>
            {
                { "Message.Type.FullName", message.GetType().FullName }
            };

            var pubsubMessage = new PubsubMessage()
            {
                Data = ByteString.CopyFromUtf8(body)
            };

            pubsubMessage.Attributes.Add(attributes);

            await _publisher.PublishAsync(pubsubMessage);
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
