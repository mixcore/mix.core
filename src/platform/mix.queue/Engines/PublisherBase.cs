using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Mix.Heart.Exceptions;
using Mix.Mq.Lib.Models;
using Mix.Queue.Engines.MixQueue;
using Mix.Queue.Interfaces;
using Mix.Queue.Models.QueueSetting;
using Mix.Shared.Services;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mix.Queue.Engines
{
    public abstract class PublisherBase : BackgroundService
    {
        private readonly IMemoryQueueService<MessageQueueModel> _queueService;
        private List<IQueuePublisher<MessageQueueModel>> _publishers;
        private readonly IConfiguration _configuration;
        private readonly MixEndpointService _mixEndpointService;
        private const int MaxConsumeLength = 100;
        private readonly string _topicId;
        private MixQueueProvider _provider;
        protected ILogger<PublisherBase> _logger;
        private readonly IPooledObjectPolicy<IModel> _rabbitMqObjectPolicy;
        protected PublisherBase(
            string topicId,
            IMemoryQueueService<MessageQueueModel> queueService,
            IConfiguration configuration,
            MixEndpointService mixEndpointService,
            ILogger<PublisherBase> logger,
            IPooledObjectPolicy<IModel> rabbitMqObjectPolicy)
        {
            _queueService = queueService;
            _configuration = configuration;
            _topicId = topicId;
            _mixEndpointService = mixEndpointService;
            _logger = logger;
            _rabbitMqObjectPolicy = rabbitMqObjectPolicy;
        }

        private List<IQueuePublisher<MessageQueueModel>> CreatePublisher(
            string topicId)
        {
            try
            {
                var queuePublishers = new List<IQueuePublisher<MessageQueueModel>>();
                var providerSetting = _configuration["MessageQueueSetting:Provider"];
                if (string.IsNullOrEmpty(providerSetting))
                {
                    return default;
                }

                _provider = Enum.Parse<MixQueueProvider>(providerSetting);

                switch (_provider)
                {
                    case MixQueueProvider.AZURE:
                        var azureSettingPath = _configuration.GetSection("MessageQueueSetting:AzureServiceBus");
                        var azureSetting = new AzureQueueSetting();
                        azureSettingPath.Bind(azureSetting);

                        queuePublishers.Add(
                            QueueEngineFactory.CreatePublisher<MessageQueueModel>(
                                _provider, azureSetting, topicId, _mixEndpointService));
                        break;
                    case MixQueueProvider.GOOGLE:
                        var googleSettingPath = _configuration.GetSection("MessageQueueSetting:GoogleQueueSetting");
                        var googleSetting = new GoogleQueueSetting();
                        googleSettingPath.Bind(googleSetting);
                        googleSetting.CredentialFile = googleSetting.CredentialFile;

                        queuePublishers.Add(
                            QueueEngineFactory.CreatePublisher<MessageQueueModel>(
                                _provider, googleSetting, topicId, _mixEndpointService));
                        break;

                    case MixQueueProvider.RABITMQ:
                        queuePublishers.Add(
                            QueueEngineFactory.CreateRabbitMqPublisher<MessageQueueModel>(_rabbitMqObjectPolicy, topicId));
                        break;

                    case MixQueueProvider.MIX:
                        if (_mixEndpointService.MixMq != null)
                        {
                            var mixSettingPath = _configuration.GetSection("MessageQueueSetting:Mix");
                            var mixSetting = new MixQueueSetting();
                            mixSettingPath.Bind(mixSetting);
                            queuePublishers.Add(
                               QueueEngineFactory.CreatePublisher<MessageQueueModel>(_provider, mixSetting, topicId, _mixEndpointService));
                        }
                        break;
                }

                return queuePublishers;
            }
            catch (Exception ex)
            {
                throw new MixException(Heart.Enums.MixErrorStatus.ServerError, ex);
            }
        }

        private Task StartMixQueueEngine(CancellationToken cancellationToken = default)
        {
            return Task.Run(async () =>
            {
                bool isProcessing = false;
                while (!cancellationToken.IsCancellationRequested)
                {
                    if (!isProcessing)
                    {
                        isProcessing = true;
                        // Get messages from IQueueService 
                        var inQueueItems = _queueService.ConsumeMemoryQueue(MaxConsumeLength, _topicId);

                        if (inQueueItems.Any() && _publishers != null)
                        {
                            foreach (var publisher in _publishers)
                            {
                                // Publish messages to current Message Queue Provider
                                // If cannot send msg, try to wait 1s then retry
                                bool publishing = true;
                                while (publishing)
                                {
                                    try
                                    {
                                        await publisher.SendMessages(inQueueItems);
                                        publishing = false;
                                    }
                                    catch (Exception ex) {
                                        _logger.LogError(ex, $"{_logger.GetType().FullName}: Cannot Send message to queue");
                                        await Task.Delay(1000, cancellationToken);
                                    }
                                }
                            }
                        }
                        isProcessing = false;
                    }
                    await Task.Delay(1000, cancellationToken);
                }
            }, cancellationToken);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _publishers = CreatePublisher(_topicId);
            return StartMixQueueEngine(stoppingToken);
        }
    }
}
