using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Heart.Exceptions;
using Mix.Mq.Lib.Models;
using Mix.Mqtt.Lib.Models;
using Mix.Queue.Engines.RabbitMQ;
using Mix.Queue.Interfaces;
using Mix.Queue.Models.QueueSetting;
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
        protected readonly IPooledObjectPolicy<IChannel>? RabbitMqObjectPolicy;

        protected PublisherBase(
            string topicId,
            IMemoryQueueService<MessageQueueModel> queueService,
            IConfiguration configuration,
            MixEndpointService mixEndpointService,
            ILogger<PublisherBase> logger,
            IPooledObjectPolicy<IChannel>? rabbitMQObjectPolicy = null)
        {
            _topicId = topicId;
            ILogger = logger;
            QueueService = queueService;
            Configuration = configuration;
            MixEndpointService = mixEndpointService;
            RabbitMqObjectPolicy = rabbitMQObjectPolicy;
        }

        public virtual List<IQueuePublisher<MessageQueueModel>>? CreatePublisher(
            string topicId)
        {
            try
            {
                var queuePublishers = new List<IQueuePublisher<MessageQueueModel>>();
                var providerSetting = Configuration[$"{MixAppSettingsSection.MessageQueueSettings}:Provider"];
                if (string.IsNullOrEmpty(providerSetting))
                {
                    return default;
                }

                Provider = Enum.Parse<MixQueueProvider>(providerSetting);

                switch (Provider)
                {
                    case MixQueueProvider.AZURE:
                        queuePublishers.Add(
                            QueueEngineFactory.CreatePublisher<MessageQueueModel>(
                                Provider, Configuration, topicId, MixEndpointService));
                        break;
                    case MixQueueProvider.GOOGLE:
                        queuePublishers.Add(
                            QueueEngineFactory.CreatePublisher<MessageQueueModel>(
                                Provider, Configuration, topicId, MixEndpointService));
                        break;

                    case MixQueueProvider.RABBITMQ:
                        queuePublishers.Add(
                            QueueEngineFactory.CreateRabbitMqPublisher<MessageQueueModel>(RabbitMqObjectPolicy, topicId));
                        break;

                    case MixQueueProvider.MIX:
                        if (MixEndpointService.MixMq != null)
                        {
                            queuePublishers.Add(
                               QueueEngineFactory.CreatePublisher<MessageQueueModel>(Provider, Configuration, topicId, MixEndpointService));
                        }
                        break;
                    case MixQueueProvider.MQTT:
                        //if (MixEndpointService.MixMq != null)
                        //{
                        //    var mixSettingPath = Configuration.GetSection($"{MixAppSettingsSection.MessageQueueSettings}:Mix");
                        queuePublishers.Add(
                           QueueEngineFactory.CreatePublisher<MessageQueueModel>(
                               Provider,
                               Configuration,
                               topicId,
                               MixEndpointService));
                        //}
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

                await Task.Delay(100, cancellationToken);
            }
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Publishers = CreatePublisher(_topicId);
            return StartMixQueueEngine(stoppingToken);
        }
    }
}
