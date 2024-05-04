using Grpc.Core;
using Mix.Mq.Lib.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;

namespace Mix.Mq.Server.Domain.Services
{
    public class GrpcStreamingService
    {
        public ConcurrentDictionary<SubscribeRequest, IServerStreamWriter<SubscribeReply>> MessageSubscriptions { get; set; }
        public ConcurrentQueue<Task> Tasks;
        private readonly MixQueueMessages<MessageQueueModel> _queue;
        private CancellationToken _cancellationToken;
        private ILogger<GrpcStreamingService> _logger;
        public ConcurrentDictionary<string, List<MessageQueueModel>> PendingMessages;

        public GrpcStreamingService(MixQueueMessages<MessageQueueModel> queue, ILogger<GrpcStreamingService> logger)
        {
            MessageSubscriptions = new();
            _queue = queue;
            Tasks = new();
            PendingMessages = new();
            _logger = logger;
        }

        public Task LoadAllSubscriptions(CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;
            return Task.WhenAll(Tasks);
        }


        public Task AddSubscription(SubscribeRequest request, IServerStreamWriter<SubscribeReply> responseStream)
        {
            MessageSubscriptions.TryRemove(request, out _);
            if (MessageSubscriptions.TryAdd(request, responseStream))
            {
                return CreateSubscription(request, responseStream);
            }
            return Task.CompletedTask;
        }
        public Task RemoveSubscription(SubscribeRequest request)
        {
            var _topic = _queue.GetTopic(request.TopicId);
            _topic.RemoveSubscription(request.SubsctiptionId);
            MessageSubscriptions.TryRemove(request, out _);
            return Task.CompletedTask;
        }

        private async Task CreateSubscription(SubscribeRequest request, IServerStreamWriter<SubscribeReply> responseStream)
        {
            var _topic = _queue.GetTopic(request.TopicId);
            Initialize(_topic, request.SubsctiptionId);

            while (true)
            {
                var inQueueItems = _topic.ConsumeQueue(request.SubsctiptionId, 10);
                try
                {
                    await SendPendingMessages(request, responseStream);

                    var result = new SubscribeReply
                    {
                        Messages = { }
                    };
                    if (inQueueItems.Count > 0)
                    {
                        foreach (var item in inQueueItems)
                        {
                            result.Messages.Add(JObject.FromObject(item).ToString(Newtonsoft.Json.Formatting.None));
                        }
                    }

                    await responseStream.WriteAsync(result).ConfigureAwait(false);
                }
                catch (RpcException ex)
                {
                    _logger.LogError($"RpcException at{DateTime.UtcNow.AddHours(7)} - {GetType().Name} Cannot write stream message to {request.SubsctiptionId}: {ex.Message}", ex);
                    AddToPendingMessages(request.SubsctiptionId, inQueueItems.ToList());
                    //await RemoveSubscription(request);
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Exception at{DateTime.UtcNow.AddHours(7)} - {GetType().Name} Cannot write stream message to {request.SubsctiptionId}: {ex.Message}", ex);
                    AddToPendingMessages(request.SubsctiptionId, inQueueItems.ToList());
                    //await RemoveSubscription(request);
                    break;
                }

                await Task.Delay(1000);
            }
            _logger.LogInformation($"{request.SubsctiptionId} stopped at {DateTime.UtcNow.AddHours(7)}");
        }

        private async Task SendPendingMessages(SubscribeRequest request, IServerStreamWriter<SubscribeReply> responseStream)
        {
            PendingMessages.TryGetValue(request.SubsctiptionId, out var messages);
            if (messages != null && messages.Count > 0)
            {
                try
                {

                    var result = new SubscribeReply
                    {
                        Messages = { }
                    };
                    foreach (var item in messages)
                    {
                        result.Messages.Add(JObject.FromObject(item).ToString(Newtonsoft.Json.Formatting.None));
                    }
                    await responseStream.WriteAsync(result).ConfigureAwait(false); ;
                    PendingMessages.TryRemove(request.SubsctiptionId, out _);
                }
                catch (RpcException ex)
                {
                    _logger.LogError($"RpcException - {GetType().Name} Cannot write pending stream message to {request.SubsctiptionId}: {ex.Message}", ex);
                    AddToPendingMessages(request.SubsctiptionId, messages);
                    await RemoveSubscription(request);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Exception - {GetType().Name} Cannot write pending stream message to {request.SubsctiptionId}: {ex.Message}", ex);
                    AddToPendingMessages(request.SubsctiptionId, messages);
                    await RemoveSubscription(request);
                }
            }
        }

        private void AddToPendingMessages(string subscriptionId, List<MessageQueueModel> inQueueItems)
        {
            PendingMessages.TryGetValue(subscriptionId, out var messages);
            if (messages == null)
            {
                PendingMessages.TryAdd(subscriptionId, inQueueItems);
            }
            else
            {
                PendingMessages[subscriptionId].AddRange(inQueueItems);
            }
        }

        private void Initialize(MixTopicModel<MessageQueueModel> topic, string subscriptionId)
        {
            while (topic == null)
            {
                Thread.Sleep(1000);
            }
            topic.CreateSubscription(subscriptionId);
        }
    }
}
