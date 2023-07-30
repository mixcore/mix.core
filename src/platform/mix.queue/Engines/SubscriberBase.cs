using Azure.Core;
using Google.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mix.Heart.Exceptions;
using Mix.Heart.Helpers;
using Mix.Heart.Services;
using Mix.Queue.Engines.MixQueue;
using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Mix.Queue.Models.QueueSetting;
using Mix.Queue.Services;
using Mix.Shared.Helpers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mix.Queue.Engines
{
    public abstract class SubscriberBase : IHostedService
    {
        private IQueueService<MessageQueueModel> _queueService;
        private readonly IQueueSubscriber _subscriber;
        private readonly IConfiguration _configuration;
        private readonly MixQueueMessages<MessageQueueModel> _mixQueueService;
        private readonly string _topicId;
        private readonly int _timeout;
        protected MixCacheService CacheService;
        protected readonly IServiceProvider ServicesProvider;
        protected IServiceScope ServiceScope { get; set; }


        protected SubscriberBase(
            string topicId,
            string moduleName,
            int timeout,
            IServiceProvider servicesProvider,
            IConfiguration configuration,
            MixQueueMessages<MessageQueueModel> mixQueueService,
            IQueueService<MessageQueueModel> queueService)
        {
            _timeout = timeout;
            _configuration = configuration;
            _mixQueueService = mixQueueService;
            _topicId = topicId;
            _subscriber = CreateSubscriber(_topicId, $"{_topicId}_{moduleName}");
            ServicesProvider = servicesProvider;
            _queueService = queueService;
        }

        public virtual Task StartAsync(CancellationToken cancellationToken = default)
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
                throw new MixException(_subscriber.SubscriptionId, ex);
            }
        }

        public virtual Task StopAsync(CancellationToken cancellationToken = default)
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
                            provider, azureSetting, topicId, subscriptionId, MessageHandler, _mixQueueService);
                    case MixQueueProvider.GOOGLE:
                        var googleSettingPath = _configuration.GetSection("MessageQueueSetting:GoogleQueueSetting");
                        var googleSetting = new GoogleQueueSetting();
                        googleSettingPath.Bind(googleSetting);
                        googleSetting.CredentialFile = googleSetting.CredentialFile;
                        return QueueEngineFactory.CreateSubscriber(
                            provider, googleSetting, topicId, subscriptionId, MessageHandler, _mixQueueService);
                    case MixQueueProvider.MIX:
                        var mixSettingPath = _configuration.GetSection("MessageQueueSetting:Mix");
                        var mixSetting = new MixQueueSetting();
                        mixSettingPath.Bind(mixSetting);
                        return QueueEngineFactory.CreateSubscriber(
                           provider, mixSetting, topicId, subscriptionId, MessageHandler, _mixQueueService);
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
                if (Handler(data).Wait(TimeSpan.FromSeconds(_timeout)))
                {
                    return;
                }
                else
                    await HandleDeadLetter(data);
            }
            catch (Exception ex)
            {
                await HandleException(ex);
            }
        }

        public virtual Task HandleDeadLetter(MessageQueueModel message)
        {
            _queueService.PushQueue(new MessageQueueModel(1)
            {
                Action = MixQueueActions.DeadLetter,
                TopicId = MixQueueTopics.MixBackgroundTasks,
                Data = ReflectionHelper.ParseObject(message).ToString(),
                Success = false
            });
            return Task.CompletedTask;
        }

        public virtual Task HandleException(Exception ex)
        {
            _queueService.PushQueue(new MessageQueueModel(1)
            {
                Action = MixQueueActions.ExceptionLog,
                TopicId = MixQueueTopics.MixBackgroundTasks,
                Data = ReflectionHelper.ParseObject(ex).ToString(),
                Success = false
            });
            return Task.CompletedTask;
        }

        public abstract Task Handler(MessageQueueModel model);
    }
}
