using Google.Apis.Auth.OAuth2;
using Google.Cloud.PubSub.V1;
using Google.Protobuf;
using Grpc.Core;
using Mix.Mq.Lib.Models;
using Mix.Queue.Interfaces;
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
                var publisherClientBuilder = new PublisherClientBuilder
                {
                    Credential = googleCredential,
                    TopicName = new TopicName(_queueSetting.ProjectId, topicName)
                };

                _publisher = publisherClientBuilder.Build();
            }
        }

        private Topic CreateTopic(string topicId)
        {
            try
            {
                var builder = new PublisherServiceApiClientBuilder
                {
                    CredentialsPath = _queueSetting.CredentialFile
                };

                var publisher = builder.Build();
                var topicName = new TopicName(_queueSetting.ProjectId, topicId);
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
