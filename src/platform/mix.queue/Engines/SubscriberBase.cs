using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Mix.Heart.Exceptions;
using Mix.Queue.Engines.MixQueue;
using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Mix.Queue.Models.QueueSetting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mix.Queue.Engines
{
    public abstract class SubscriberBase : IHostedService
    {
        private readonly IQueueSubscriber _subscriber;
        private readonly IConfiguration _configuration;
        private readonly MixMemoryMessageQueue<MessageQueueModel> _queueService;
        private readonly string _topicId;

        public SubscriberBase(
            string topicId,
            string moduleName,
            IConfiguration configuration,
            MixMemoryMessageQueue<MessageQueueModel> queueService)
        {
            _configuration = configuration;
            _queueService = queueService;
            _topicId = topicId;
            _subscriber = CreateSubscriber(_topicId, $"{_topicId}_{moduleName}");
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(async () =>
            {
                if (_subscriber != null)
                {
                    await _subscriber.ProcessQueue(cancellationToken);
                }
            }, cancellationToken);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private IQueueSubscriber CreateSubscriber(string topicId, string subscriptionId)
        {
            try
            {
                var providerSetting = _configuration["MessageQueueSetting:Provider"];
                var provider = Enum.Parse<MixQueueProvider>(providerSetting);

                switch (provider)
                {
                    case MixQueueProvider.AZURE:
                        var azureSettingPath = _configuration.GetSection("MessageQueueSetting:AzureServiceBus");
                        var azureSetting = new AzureQueueSetting();
                        azureSettingPath.Bind(azureSetting);
                        return QueueEngineFactory.CreateSubscriber(
                            provider, azureSetting, topicId, subscriptionId, MesageHandler, _queueService);
                    case MixQueueProvider.GOOGLE:
                        var googleSettingPath = _configuration.GetSection("MessageQueueSetting:GoogleQueueSetting");
                        var googleSetting = new GoogleQueueSetting();
                        googleSettingPath.Bind(googleSetting);
                        googleSetting.CredentialFile = googleSetting.CredentialFile;
                        return QueueEngineFactory.CreateSubscriber(
                            provider, googleSetting, topicId, subscriptionId, MesageHandler, _queueService);
                    case MixQueueProvider.MIX:
                        var mixSettingPath = _configuration.GetSection("MessageQueueSetting:Mix");
                        var mixSetting = new MixQueueSetting();
                        mixSettingPath.Bind(mixSetting);
                        return QueueEngineFactory.CreateSubscriber(
                           provider, mixSetting, topicId, subscriptionId, MesageHandler, _queueService);
                }
            }
            catch (Exception ex)
            {
                throw new MixException(Heart.Enums.MixErrorStatus.ServerError, ex);
            }

            return default;
        }

        public Task MesageHandler(MessageQueueModel data)
        {
            try
            {
                if (_topicId != data.TopicId)
                {
                    return Task.CompletedTask;
                }
                return Handler(data);
            }
            catch (Exception ex)
            {
                throw new MixException(Heart.Enums.MixErrorStatus.ServerError, ex);
            }
        }

        public abstract Task Handler(MessageQueueModel model);
    }
}
