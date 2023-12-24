using Grpc.Core;
using Microsoft.Extensions.Hosting;
using Mix.Heart.Helpers;
using Mix.Mq;
using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Mix.Queue.Models.QueueSetting;
using Mix.Shared.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mix.Queue.Engines.MixQueue
{
    public class MixQueueSubscriber<T> : BackgroundService, IQueueSubscriber
        where T : MessageQueueModel
    {
        public string SubscriptionId { get; set; }
        public bool IsProcessing { get; private set; }
        private readonly string _subscriptionId;
        private MixTopicModel<T> _topic;
        private readonly MixQueueSetting _queueSetting;
        private readonly Func<T, Task> _messageHandler;
        private readonly IQueueService<MessageQueueModel> _memQueues;
        private readonly MixEndpointService _mixEndpointService;
        private GrpcChannelModel<MixMq.MixMqClient> _mixMqSubscriber;
        private SubscribeRequest _subscribeRequest;
        public MixQueueSubscriber(
            QueueSetting queueSetting,
            string topicId,
            string subscriptionId,
            Func<T, Task> messageHandler,
            IQueueService<MessageQueueModel> memQueues,
            MixEndpointService mixEndpointService)
        {
            _queueSetting = queueSetting as MixQueueSetting;
            _subscriptionId = subscriptionId;
            _messageHandler = messageHandler;
            _memQueues = memQueues;
            _mixEndpointService = mixEndpointService;
            _mixMqSubscriber = new GrpcChannelModel<MixMq.MixMqClient>(_mixEndpointService.MixMq);
            _subscribeRequest = new SubscribeRequest()
            {
                TopicId = topicId,
                SubsctiptionId = _subscriptionId,
            };
        }

        /// <summary>
        /// Process message queue
        /// </summary>
        /// <returns></returns>
        public async Task ProcessQueue(CancellationToken cancellationToken = default)
        {
            using var call = _mixMqSubscriber.Client.Subscribe(_subscribeRequest);

            while (await call.ResponseStream.MoveNext())
            {
                if (!IsProcessing)
                {
                    IsProcessing = true;

                    
                    if (call.ResponseStream.Current.Messages.Count > 0)
                    {
                        foreach (var msg in call.ResponseStream.Current.Messages)
                        {
                            var obj = JObject.Parse(msg).ToObject<T>();
                            AckQueueMessage(obj);
                            await _messageHandler.Invoke(obj);
                        }
                    }
                    IsProcessing = false;
                    await Task.Delay(1000, cancellationToken);
                }
            }
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return ProcessQueue(stoppingToken);
        }

        private void AckQueueMessage(T model)
        {
            if (model.TopicId != MixQueueTopics.MixLog)
            {
                var logQueue = _memQueues.GetQueue(MixQueueTopics.MixLog);
                if (logQueue != null)
                {
                    logQueue.Enqueue(new MessageQueueModel()
                    {
                        TopicId = MixQueueTopics.MixLog,
                        Action = MixQueueActions.AckLog,
                        Sender = _subscriptionId,
                        Data = ReflectionHelper.ParseObject(model).ToString(),
                        TenantId = 1,
                        CreatedDate = DateTime.UtcNow
                    });
                }
            }
        }
    }
}
