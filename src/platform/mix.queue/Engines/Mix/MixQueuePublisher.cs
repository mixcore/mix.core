using Mix.Mq;
using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Mix.Queue.Models.QueueSetting;
using Mix.Shared.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mix.Queue.Engines.MixQueue
{
    public class MixQueuePublisher<T> : IQueuePublisher<T>
        where T : MessageQueueModel
    {
        private string _topicId;
        private readonly MixEndpointService _mixEndpointService;
        private GrpcChannelModel<MixMq.MixMqClient> _mixMqSubscriber;

        public MixQueuePublisher(QueueSetting queueSetting, string topicName, MixEndpointService mixEndpointService)
        {
            _topicId = topicName;
            _mixEndpointService = mixEndpointService;
            _mixMqSubscriber = new GrpcChannelModel<MixMq.MixMqClient>(_mixEndpointService.MixMq);
        }

        public Task SendMessage(T message)
        {
            if (message.Id == default)
            {
                message.Id = Guid.NewGuid();
            }
            message.CreatedDate = DateTime.UtcNow;
            _mixMqSubscriber.Client.Publish(new PublishMessageRequest
            {
                TopicId = _topicId,
                Message = JObject.FromObject(message).ToString(Newtonsoft.Json.Formatting.None)
            });

            return Task.CompletedTask;
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
