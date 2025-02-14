using Azure.Core;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Mix.Heart.Extensions;
using Mix.Mq.Lib.Models;
using Mix.Signalr.Hub.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using static Mix.Mq.Server.MixMq;

namespace Mix.Mq.Server.Domain.Services;

public class MixMqService : MixMqBase
{
    private readonly ILogger<MixMqService> _logger;
    private readonly MixQueueMessages<MessageQueueModel> _queue;
    private readonly GrpcStreamingService _subscriptionService;
    public MixMqService(ILogger<MixMqService> logger, MixQueueMessages<MessageQueueModel> queue, GrpcStreamingService subscriptionService)
    {
        _logger = logger;
        _queue = queue;
        _subscriptionService = subscriptionService;
    }

    public override Task Subscribe(MixSubscribeRequest request, IServerStreamWriter<MixSubscribeReply> responseStream, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation($"{request.SubsctiptionId} started at {DateTime.UtcNow.AddHours(7)}");
            return _subscriptionService.AddSubscription(request, responseStream);
        }
        catch (Exception e)
        {
            _logger.LogError($"{request.SubsctiptionId} broken at {DateTime.UtcNow.AddHours(7)}: {e.Message}", e);
            return _subscriptionService.RemoveSubscription(request);
        }
    }

    public override async Task<Empty> Disconnect(MixSubscribeRequest request, ServerCallContext context)
    {
        await _subscriptionService.RemoveSubscription(request);
        _logger.LogInformation($"{request.SubsctiptionId} disconnected at {DateTime.UtcNow.AddHours(7)}");
        return new();
    }

    public override Task<Empty> Publish(MixPublishMessageRequest request, ServerCallContext context)
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
