using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Mix.Heart.Exceptions;
using Mix.Mq.Lib.Models;
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
        protected MixQueueProvider Provider;
        protected const int MaxConsumeLength = 100;
        protected readonly string _topicId;
        protected List<IQueuePublisher<MessageQueueModel>> Publishers;

        protected readonly IMemoryQueueService<MessageQueueModel> QueueService;
        protected readonly IConfiguration Configuration;
        protected readonly MixEndpointService MixEndpointService;
        protected readonly ILogger<PublisherBase> ILogger;
        protected readonly IPooledObjectPolicy<IModel> RabbitMqObjectPolicy;

        protected PublisherBase(
            string topicId,
            IMemoryQueueService<MessageQueueModel> queueService,
            IConfiguration configuration,
            MixEndpointService mixEndpointService,
            ILogger<PublisherBase> logger,
            IPooledObjectPolicy<IModel> rabbitMqObjectPolicy)
        {
            _topicId = topicId;
            ILogger = logger;
            QueueService = queueService;
            Configuration = configuration;
            MixEndpointService = mixEndpointService;
            RabbitMqObjectPolicy = rabbitMqObjectPolicy;
        }

        protected List<IQueuePublisher<MessageQueueModel>> CreatePublisher(
            string topicId)
        {
            try
            {
                var queuePublishers = new List<IQueuePublisher<MessageQueueModel>>();
                var providerSetting = Configuration["MessageQueueSetting:Provider"];
                if (string.IsNullOrEmpty(providerSetting))
                {
                    return default;
                }

                Provider = Enum.Parse<MixQueueProvider>(providerSetting);

                switch (Provider)
                {
                    case MixQueueProvider.AZURE:
                        var azureSettingPath = Configuration.GetSection("MessageQueueSetting:AzureServiceBus");
                        var azureSetting = new AzureQueueSetting();
                        azureSettingPath.Bind(azureSetting);

                        queuePublishers.Add(
                            QueueEngineFactory.CreatePublisher<MessageQueueModel>(
                                Provider, azureSetting, topicId, MixEndpointService));
                        break;
                    case MixQueueProvider.GOOGLE:
                        var googleSettingPath = Configuration.GetSection("MessageQueueSetting:GoogleQueueSetting");
                        var googleSetting = new GoogleQueueSetting();
                        googleSettingPath.Bind(googleSetting);
                        googleSetting.CredentialFile = googleSetting.CredentialFile;

                        queuePublishers.Add(
                            QueueEngineFactory.CreatePublisher<MessageQueueModel>(
                                Provider, googleSetting, topicId, MixEndpointService));
                        break;

                    case MixQueueProvider.RABBITMQ:
                        queuePublishers.Add(
                            QueueEngineFactory.CreateRabbitMqPublisher<MessageQueueModel>(RabbitMqObjectPolicy, topicId));
                        break;

                    case MixQueueProvider.MIX:
                        if (MixEndpointService.MixMq != null)
                        {
                            var mixSettingPath = Configuration.GetSection("MessageQueueSetting:Mix");
                            var mixSetting = new MixQueueSetting();
                            mixSettingPath.Bind(mixSetting);
                            queuePublishers.Add(
                               QueueEngineFactory.CreatePublisher<MessageQueueModel>(Provider, mixSetting, topicId, MixEndpointService));
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

        protected virtual async Task StartMixQueueEngine(CancellationToken cancellationToken = default)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                // Get messages from IQueueService 
                var inQueueItems = QueueService.ConsumeMemoryQueue(MaxConsumeLength, _topicId);
                if (inQueueItems.Any() && Publishers != null)
                {
                    foreach (var publisher in Publishers)
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
                            catch (Exception ex)
                            {
                                ILogger.LogError(ex, "{FullName}: Cannot Send message to queue", ILogger.GetType().FullName);
                                await Task.Delay(1000, cancellationToken);
                            }
                        }
                    }
                }

                await Task.Delay(50, cancellationToken);
            }
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Publishers = CreatePublisher(_topicId);
            return StartMixQueueEngine(stoppingToken);
        }
    }
}
