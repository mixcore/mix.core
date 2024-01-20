using Azure.Messaging.ServiceBus.Administration;
using Grpc.Core;
using Mix.Mq.Lib.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;
using System.Threading;

namespace Mix.Mq.Server.Domain.Services
{
    public sealed class MixMqSubscriptionService : IHostedService
    {
        private readonly ILogger<MixMqService> _logger;
        private CancellationToken _cancellationToken;
        private GrpcStreamingService _grpcStreamingService { get; set; }
        public MixMqSubscriptionService(ILogger<MixMqService> logger, GrpcStreamingService grpcStreamingService)
        {
            _logger = logger;
            _grpcStreamingService = grpcStreamingService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return _grpcStreamingService.LoadAllSubscriptions(cancellationToken);
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{GetType().FullName} stopped");
            return Task.CompletedTask;
        }

       
    }
}
