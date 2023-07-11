using Azure.Core;
using Google.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mix.Heart.Exceptions;
using Mix.Heart.Services;
using Mix.Queue.Engines.MixQueue;
using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Mix.Queue.Models.QueueSetting;
using Mix.Shared.Helpers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mix.Queue.Engines
{
    public abstract class SubscriberBase : IHostedService
    {
        private readonly IQueueSubscriber _subscriber;
        private readonly IConfiguration _configuration;
        private readonly MixQueueMessages<MessageQueueModel> _queueService;
        private readonly string _topicId;

        protected MixCacheService CacheService;
        protected readonly IServiceProvider ServicesProvider;
        protected IServiceScope ServiceScope { get; set; }
        protected SubscriberBase(
            string topicId,
            string moduleName,
            IServiceProvider servicesProvider,
            IConfiguration configuration,
            MixQueueMessages<MessageQueueModel> queueService)
        {
            _configuration = configuration;
            _queueService = queueService;
            _topicId = topicId;
            _subscriber = CreateSubscriber(_topicId, $"{_topicId}_{moduleName}");
            ServicesProvider = servicesProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken = default)
        {
            try
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
            catch (Exception ex)
            {
                throw new MixException(Heart.Enums.MixErrorStatus.ServerError, ex);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken = default)
        {
            Console.Error.WriteLine($"{_subscriber.SubscriptionId} stopped at {DateTime.UtcNow}");
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

        protected T GetRequiredService<T>()
        {
            ServiceScope ??= ServicesProvider.CreateScope();
            return ServiceScope.ServiceProvider.GetRequiredService<T>();
        }

        public async Task MessageHandler(MessageQueueModel data)
        {

            try
            {
                if (_topicId != data.TopicId)
                {
                    return;
                }

                CacheService ??= GetRequiredService<MixCacheService>();
                await Handler(data);
            }
            catch (Exception ex)
            {
                await HandleException(ex);
            }
        }

        public virtual Task HandleException(Exception ex)
        {
            Console.Error.WriteLine(ex);
            return Task.CompletedTask;
        }

        public abstract Task Handler(MessageQueueModel model);
    }
}
