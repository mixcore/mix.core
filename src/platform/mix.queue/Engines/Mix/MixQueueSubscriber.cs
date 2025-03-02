using Grpc.Core;
using Microsoft.Extensions.Hosting;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Heart.Helpers;
using Mix.Mq;
using Mix.Mq.Lib.Models;
using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Mix.Queue.Models.QueueSetting;
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
        private readonly MixQueueSetting _queueSetting;
        private readonly Func<T, Task> _messageHandler;
        private readonly IMemoryQueueService<MessageQueueModel> _memQueues;
        private readonly MixEndpointService _mixEndpointService;
        private GrpcChannelModel<MixMq.MixMqClient> _mixMqSubscriber;
        private SubscribeRequest _subscribeRequest;
        private AsyncServerStreamingCall<SubscribeReply> _call;
        public MixQueueSubscriber(
            IQueueSetting queueSetting,
            string topicId,
            string subscriptionId,
            Func<T, Task> messageHandler,
            IMemoryQueueService<MessageQueueModel> memQueues,
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
            try
            {
                _call = _mixMqSubscriber.Client.Subscribe(_subscribeRequest);
                while (await _call.ResponseStream.MoveNext())
                {
                    if (!IsProcessing)
                    {
                        IsProcessing = true;

                        if (_call.ResponseStream.Current.Messages.Count > 0)
                        {
                            foreach (var msg in _call.ResponseStream.Current.Messages)
                            {
                                var obj = JObject.Parse(msg).ToObject<T>();
                                AckQueueMessage(obj);
                                await _messageHandler.Invoke(obj);
                            }
                        }
                        IsProcessing = false;
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task Disconnect(CancellationToken cancellationToken = default)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                await _mixMqSubscriber.Client.DisconnectAsync(_subscribeRequest);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
            }
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                return ProcessQueue(stoppingToken);
            }
            return Task.CompletedTask;
        }

        private void AckQueueMessage(T model)
        {
            if (model.TopicId != MixQueueTopics.MixLog)
            {
                var logQueue = _memQueues.GetMemoryQueue(MixQueueTopics.MixLog);
                if (logQueue != null)
                {
                    logQueue.Enqueue(new MessageQueueModel()
                    {
                        TopicId = MixQueueTopics.MixLog,
                        Action = MixQueueActions.AckLog,
                        Sender = _subscriptionId,
                        Data = ReflectionHelper.ParseObject(model).ToString(Newtonsoft.Json.Formatting.None),
                        TenantId = 1,
                        CreatedDate = DateTime.UtcNow
                    });
                }
            }
        }

    }
}
