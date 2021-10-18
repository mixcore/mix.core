using Google.Apis.Auth.OAuth2;
using Google.Cloud.PubSub.V1;
using Google.Protobuf;
using Grpc.Auth;
using Mix.Queue.Interfaces;
using Mix.Queue.Models.QueueSetting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mix.Queue.Engines.Google
{
    internal class GoogleQueuePublisher<T> : IQueuePublisher<T>
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
                var googleCredential = GoogleCredential.FromFile(_queueSetting.CredentialFile);
                var createSettings = new PublisherClient.ClientCreationSettings(credentials: googleCredential.ToChannelCredentials());
                var toppicName = new TopicName(_queueSetting.ProjectId, topicName);
                var publisher = PublisherClient.CreateAsync(toppicName, createSettings);
                _publisher = publisher.Result;
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
