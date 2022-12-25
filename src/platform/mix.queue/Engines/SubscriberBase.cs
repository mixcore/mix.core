using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

        protected readonly IServiceProvider _servicesProvider;
        private IServiceScope _serviceScope;
        protected IServiceScope ServiceScope { get => GetServiceScope(); set { _serviceScope = value; } }
        protected SubscriberBase(
            string topicId,
            string moduleName,
            IServiceProvider servicesProvider,
            IConfiguration configuration,
            MixMemoryMessageQueue<MessageQueueModel> queueService)
        {
            _configuration = configuration;
            _queueService = queueService;
            _topicId = topicId;
            _subscriber = CreateSubscriber(_topicId, $"{_topicId}_{moduleName}");
            _servicesProvider = servicesProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken = default)
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

        public Task StopAsync(CancellationToken cancellationToken = default)
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
                            provider, azureSetting, topicId, subscriptionId, MessageHandler, _queueService);
                    case MixQueueProvider.GOOGLE:
                        var googleSettingPath = _configuration.GetSection("MessageQueueSetting:GoogleQueueSetting");
                        var googleSetting = new GoogleQueueSetting();
                        googleSettingPath.Bind(googleSetting);
                        googleSetting.CredentialFile = googleSetting.CredentialFile;
                        return QueueEngineFactory.CreateSubscriber(
                            provider, googleSetting, topicId, subscriptionId, MessageHandler, _queueService);
                    case MixQueueProvider.MIX:
                        var mixSettingPath = _configuration.GetSection("MessageQueueSetting:Mix");
                        var mixSetting = new MixQueueSetting();
                        mixSettingPath.Bind(mixSetting);
                        return QueueEngineFactory.CreateSubscriber(
                           provider, mixSetting, topicId, subscriptionId, MessageHandler, _queueService);
                }
            }
            catch (Exception ex)
            {
                throw new MixException(Heart.Enums.MixErrorStatus.ServerError, ex);
            }

            return default;
        }

        protected IServiceScope GetServiceScope()
        {
            if (_serviceScope == null)
            {
                _serviceScope = _servicesProvider.CreateScope();
            }
            return _serviceScope;
        }

        protected T GetScopedService<T>()
        {
            return ServiceScope.ServiceProvider.GetRequiredService<T>();
        }

        public Task MessageHandler(MessageQueueModel data)
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
                Console.WriteLine(ex.Message);
                return Task.CompletedTask;
            }
            finally
            {
                _serviceScope?.Dispose();
            }
        }

        public abstract Task Handler(MessageQueueModel model);
    }
}
