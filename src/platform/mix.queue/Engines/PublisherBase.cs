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
    public abstract class PublisherBase : BackgroundService, IHostedService
    {
        private IQueueService<MessageQueueModel> _queueService;
        private List<IQueuePublisher<MessageQueueModel>> _publishers;
        private IConfiguration _configuration;
        private MixMemoryMessageQueue<MessageQueueModel> _queue;
        private const int MAX_CONSUME_LENGTH = 100;
        private string _topicId;

        public PublisherBase(
            string topicId,
            IQueueService<MessageQueueModel> queueService,
            IConfiguration configuration,
            MixMemoryMessageQueue<MessageQueueModel> queue)
        {
            _queueService = queueService;
            _configuration = configuration;
            _queue = queue;
            _topicId = topicId;
        }


        private List<IQueuePublisher<MessageQueueModel>> CreatePublisher(string topicName,
            MixMemoryMessageQueue<MessageQueueModel> queue, CancellationToken cancellationToken)
        {
            try
            {
                List<IQueuePublisher<MessageQueueModel>> queuePublishers =
                    new List<IQueuePublisher<MessageQueueModel>>();
                var providerSetting = _configuration["MessageQueueSetting:Provider"];

                var provider = Enum.Parse<MixQueueProvider>(providerSetting);

                switch (provider)
                {
                    case MixQueueProvider.AZURE:
                        var azureSettingPath = _configuration.GetSection("MessageQueueSetting:AzureServiceBus");
                        var azureSetting = new AzureQueueSetting();
                        azureSettingPath.Bind(azureSetting);

                        queuePublishers.Add(
                            QueueEngineFactory.CreatePublisher<MessageQueueModel>(
                                provider, azureSetting, topicName));
                        break;
                    case MixQueueProvider.GOOGLE:
                        var googleSettingPath = _configuration.GetSection("MessageQueueSetting:GoogleQueueSetting");
                        var googleSetting = new GoogleQueueSetting();
                        googleSettingPath.Bind(googleSetting);
                        googleSetting.CredentialFile = googleSetting.CredentialFile;

                        queuePublishers.Add(
                            QueueEngineFactory.CreatePublisher<MessageQueueModel>(
                                provider, googleSetting, topicName));
                        break;
                    case MixQueueProvider.MIX:
                        var mixSettingPath = _configuration.GetSection("MessageQueueSetting:Mix");
                        var mixSetting = new MixQueueSetting();
                        mixSettingPath.Bind(mixSetting);
                        queuePublishers.Add(
                           QueueEngineFactory.CreatePublisher(
                               provider, mixSetting, topicName, queue));

                        break;
                }

                return queuePublishers;
            }
            catch (Exception ex)
            {
                throw new MixException(Heart.Enums.MixErrorStatus.ServerError, ex);
            }
        }

        private Task StartMixQueueEngine(CancellationToken cancellationToken)
        {
            return Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var inQueueItems = _queueService.ConsumeQueue(MAX_CONSUME_LENGTH, _topicId);

                    if (inQueueItems.Any() && _publishers != null)
                    {
                        Parallel.ForEach(_publishers, async publisher => { await publisher.SendMessages(inQueueItems); });
                    }
                    await Task.Delay(1000, cancellationToken);
                }
            }, cancellationToken);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _publishers = CreatePublisher(_topicId, _queue, stoppingToken);
            return StartMixQueueEngine(stoppingToken);
        }
    }
}
