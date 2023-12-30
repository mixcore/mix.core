using Azure.Core;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Mix.Heart.Extensions;
using Mix.Mq;
using Mix.Mq.Lib.Models;
using Newtonsoft.Json.Linq;
using System.Threading;

namespace Mix.Mq.Services;

public class MixMqService : MixMq.MixMqBase
{
    private readonly ILogger<MixMqService> _logger;
    private readonly MixQueueMessages<MessageQueueModel> _queue;
    public MixMqService(ILogger<MixMqService> logger, MixQueueMessages<MessageQueueModel> queue)
    {
        _logger = logger;
        _queue = queue;
    }

    public override async Task Subscribe(SubscribeRequest request, IServerStreamWriter<SubscribeReply> responseStream, ServerCallContext context)
    {
        var _topic = _queue.GetTopic(request.TopicId);
        Initialize(_topic, request.SubsctiptionId);

        while (!context.CancellationToken.IsCancellationRequested)
        {
            try
            {
                var inQueueItems = _queue.GetTopic(request.TopicId).ConsumeQueue(request.SubsctiptionId, 10);
                if (inQueueItems.Count > 0)
                {
                    var result = new SubscribeReply
                    {
                        Messages = { }
                    };
                    foreach (var item in inQueueItems)
                    {
                        result.Messages.Add(JObject.FromObject(item).ToString(Newtonsoft.Json.Formatting.None));
                    }
                    await responseStream.WriteAsync(result);
                }
                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cannot consume queue");
            }
        }

        _logger.LogInformation($"Request {request.TopicId} - {request.SubsctiptionId} canceled");
    }

    public override Task<Empty> Publish(PublishMessageRequest request, ServerCallContext context)
    {
        if (!request.Message.IsJsonString())
        {
            return Task.FromResult<Empty>(new());
        }

        MessageQueueModel? msg = TryParseMessage(request.Message);
        if (msg != null)
        {
            _queue.GetTopic(request.TopicId).PushQueue(msg);
        }

        return Task.FromResult<Empty>(new());
    }

    private void Initialize(MixTopicModel<MessageQueueModel> topic, string subscriptionId)
    {
        while (topic == null)
        {
            Thread.Sleep(1000);
        }
        topic.CreateSubscription(subscriptionId);
    }

    private MessageQueueModel? TryParseMessage(string message)
    {
        try
        {
            return JObject.Parse(message).ToObject<MessageQueueModel>();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "From MixMq");
            return default;
        }
    }
}
