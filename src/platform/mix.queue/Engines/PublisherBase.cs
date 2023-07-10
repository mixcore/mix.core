using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Mix.Heart.Exceptions;
using Mix.Queue.Engines.MixQueue;
using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Mix.Queue.Models.QueueSetting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mix.Queue.Engines
{
    public abstract class PublisherBase : BackgroundService
    {
        private readonly IQueueService<MessageQueueModel> _queueService;
        private List<IQueuePublisher<MessageQueueModel>> _publishers;
        private readonly IConfiguration _configuration;
        private readonly MixQueueMessages<MessageQueueModel> _queue;
        private const int MaxConsumeLength = 100;
        private readonly string _topicId;
        private MixQueueProvider _provider;
        protected PublisherBase(
            string topicId,
            IQueueService<MessageQueueModel> queueService,
            IConfiguration configuration,
            MixQueueMessages<MessageQueueModel> queue)
        {
            _queueService = queueService;
            _configuration = configuration;
            _queue = queue;
            _topicId = topicId;
        }

        private List<IQueuePublisher<MessageQueueModel>> CreatePublisher(
            string topicName)
        {
            try
            {
                var queuePublishers = new List<IQueuePublisher<MessageQueueModel>>();
                var providerSetting = _configuration["MessageQueueSetting:Provider"];

                _provider = Enum.Parse<MixQueueProvider>(providerSetting);

                switch (_provider)
                {
                    case MixQueueProvider.AZURE:
                        var azureSettingPath = _configuration.GetSection("MessageQueueSetting:AzureServiceBus");
                        var azureSetting = new AzureQueueSetting();
                        azureSettingPath.Bind(azureSetting);

                        queuePublishers.Add(
                            QueueEngineFactory.CreatePublisher<MessageQueueModel>(
                                _provider, azureSetting, topicName));
                        break;
                    case MixQueueProvider.GOOGLE:
                        var googleSettingPath = _configuration.GetSection("MessageQueueSetting:GoogleQueueSetting");
                        var googleSetting = new GoogleQueueSetting();
                        googleSettingPath.Bind(googleSetting);
                        googleSetting.CredentialFile = googleSetting.CredentialFile;

                        queuePublishers.Add(
                            QueueEngineFactory.CreatePublisher<MessageQueueModel>(
                                _provider, googleSetting, topicName));
                        break;
                    case MixQueueProvider.MIX:
                        var mixSettingPath = _configuration.GetSection("MessageQueueSetting:Mix");
                        var mixSetting = new MixQueueSetting();
                        mixSettingPath.Bind(mixSetting);
                        queuePublishers.Add(
                           QueueEngineFactory.CreatePublisher(
                               _provider, mixSetting, topicName, _queue));

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
                        var inQueueItems = _queueService.ConsumeQueue(MaxConsumeLength, _topicId);

                        if (inQueueItems.Any() && _publishers != null)
                        {
                            foreach (var publisher in _publishers)
                            {
                                // Publish messages to current Message Queue Provider
                                // If use Mix Queue => push to MixQueueMessages<MessageQueueModel> queue
                                await publisher.SendMessages(inQueueItems);
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
