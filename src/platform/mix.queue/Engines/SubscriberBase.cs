using Azure.Core;
using Google.Api;
using Google.Apis.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Mix.Heart.Enums;
using Mix.Heart.Exceptions;
using Mix.Heart.Helpers;
using Mix.Heart.Services;
using Mix.Mq.Lib.Models;
using Mix.Queue.Engines.MixQueue;
using Mix.Queue.Engines.RabitMQ;
using Mix.Queue.Interfaces;
using Mix.Queue.Models.QueueSetting;
using Mix.Queue.Services;
using Mix.Shared.Helpers;
using Mix.Shared.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mix.Queue.Engines
{
    public abstract class SubscriberBase : BackgroundService
    {
        protected IMemoryQueueService<MessageQueueModel> _memQueueService;
        protected IQueueSubscriber _subscriber;
        protected readonly IConfiguration _configuration;
        protected readonly string _topicId;
        protected readonly string _moduleName;
        protected readonly int _timeout;
        protected MixCacheService CacheService;
        protected readonly IServiceProvider ServicesProvider;
        protected IServiceScope ServiceScope { get; set; }
        protected ILogger<SubscriberBase> _logger { get; set; }
        private readonly IPooledObjectPolicy<RabbitMQ.Client.IModel> _rabbitMqObjectPolicy;

        protected SubscriberBase(
            string topicId,
            string moduleName,
            int timeout,
            IServiceProvider servicesProvider,
            IConfiguration configuration,
            IMemoryQueueService<MessageQueueModel> queueService,
            ILogger<SubscriberBase> logger,
            IPooledObjectPolicy<RabbitMQ.Client.IModel> rabbitMqObjectPolicy)
        {
            _timeout = timeout;
            _configuration = configuration;
            _topicId = topicId;
            _moduleName = moduleName;
            _memQueueService = queueService;
            ServicesProvider = servicesProvider;
            _logger = logger;
            _rabbitMqObjectPolicy = rabbitMqObjectPolicy;
        }
        protected async override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            _subscriber = CreateSubscriber(_topicId, $"{_topicId}.{_moduleName}");
            if (_subscriber is not RabitMQSubscriber<MessageQueueModel>)
            {
                await StartProcessQueue(cancellationToken);
            }
        }

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            Console.Error.WriteLine($"{_subscriber.SubscriptionId} stopped at {DateTime.UtcNow}");
            if (_subscriber is MixQueueSubscriber<MessageQueueModel>)
            {
                await (_subscriber as MixQueueSubscriber<MessageQueueModel>).Disconnect();
            }
        }

        #region Privates
        private async Task StartProcessQueue(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"StartProcessQueue: {_subscriber.SubscriptionId} started at {DateTime.UtcNow.AddHours(7)}");
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {

                    if (_subscriber != null)
                    {
                        
                        await _subscriber.ProcessQueue(cancellationToken);
                    }
                    await Task.Delay(1000, cancellationToken);
                    if (_subscriber is MixQueueSubscriber<MessageQueueModel>)
                    {
                        await (_subscriber as MixQueueSubscriber<MessageQueueModel>).Disconnect();
                    }
                }
                catch (Exception ex)
                {
                    if (_subscriber is MixQueueSubscriber<MessageQueueModel>)
                    {
                        await (_subscriber as MixQueueSubscriber<MessageQueueModel>).Disconnect();
                    }

                    _logger.LogError($"StartProcessQueue: {_subscriber.SubscriptionId} is broken at {DateTime.UtcNow.AddHours(7)}, Trying to reconnect from client: {ex.Message}", ex);

                    await Task.Delay(2000, cancellationToken);
                    _subscriber = CreateSubscriber(_topicId, $"{_topicId}_{_moduleName}");
                    await StartProcessQueue(cancellationToken);
                }
            }
            _logger.LogInformation($"StartProcessQueue: {_subscriber.SubscriptionId} stopped at {DateTime.UtcNow.AddHours(7)}");
        }

        private IQueueSubscriber CreateSubscriber(string topicId, string subscriptionId)
        {
            try
            {
                var providerSetting = _configuration["MessageQueueSetting:Provider"];
                if (string.IsNullOrEmpty(providerSetting))
                {
                    return default;
                }

                var provider = Enum.Parse<MixQueueProvider>(providerSetting);
                var mixEndpointService = GetRequiredService<MixEndpointService>();
                switch (provider)
                {
                    case MixQueueProvider.AZURE:
                        var azureSettingPath = _configuration.GetSection("MessageQueueSetting:AzureServiceBus");
                        var azureSetting = new AzureQueueSetting();
                        azureSettingPath.Bind(azureSetting);
                        return QueueEngineFactory.CreateSubscriber<MessageQueueModel>(
                            provider, azureSetting, topicId, subscriptionId, MessageHandler, _memQueueService, mixEndpointService);
                    case MixQueueProvider.GOOGLE:
                        var googleSettingPath = _configuration.GetSection("MessageQueueSetting:GoogleQueueSetting");
                        var googleSetting = new GoogleQueueSetting();
                        googleSettingPath.Bind(googleSetting);
                        googleSetting.CredentialFile = googleSetting.CredentialFile;
                        return QueueEngineFactory.CreateSubscriber<MessageQueueModel>(
                            provider, googleSetting, topicId, subscriptionId, MessageHandler, _memQueueService, mixEndpointService);
                    case MixQueueProvider.RABBITMQ:
                        return QueueEngineFactory.CreateRabbitMQSubscriber<MessageQueueModel>(_rabbitMqObjectPolicy, topicId,subscriptionId, MessageHandler);
                    case MixQueueProvider.MIX:
                        if (string.IsNullOrEmpty(mixEndpointService.MixMq))
                        {
                            return default;
                        }

                        var mixSettingPath = _configuration.GetSection("MessageQueueSetting:Mix");
                        var mixSetting = new MixQueueSetting();
                        mixSettingPath.Bind(mixSetting);
                        return QueueEngineFactory.CreateSubscriber<MessageQueueModel>(
                           provider, mixSetting, topicId, subscriptionId, MessageHandler, _memQueueService, mixEndpointService);
                }
            }
            catch (Exception ex)
            {
                throw new MixException(Heart.Enums.MixErrorStatus.ServerError, ex);
            }

            return default;
        }

        protected T? GetRequiredService<T>()
        {
            ServiceScope ??= ServicesProvider.CreateScope();
            return ServiceScope.ServiceProvider.GetRequiredService<T?>();
        }

        public async Task MessageHandler(MessageQueueModel data)
        {
            try
            {
                if (_topicId != data.TopicId)
                {
                    return;
                }

                if (Handler(data).Wait(TimeSpan.FromSeconds(_timeout)))
                {
                    return;
                }
                else
                    await HandleDeadLetter(data);
            }
            catch (Exception ex)
            {
                await HandleException(data, ex);
            }
            finally
            {
                ServiceScope?.Dispose();
                ServiceScope = null;
            }
        }

        public virtual Task HandleDeadLetter(MessageQueueModel message)
        {
            _memQueueService.PushMemoryQueue(new MessageQueueModel(1)
            {
                Action = MixQueueActions.DeadLetter,
                TopicId = MixQueueTopics.MixLog,
                Data = ReflectionHelper.ParseObject(message).ToString(Newtonsoft.Json.Formatting.None),
                Success = false
            });
            return Task.CompletedTask;
        }

        public virtual Task HandleException(MessageQueueModel data, Exception ex)
        {
            _memQueueService.PushMemoryQueue(new MessageQueueModel(1)
            {
                Action = MixQueueActions.QueueFailed,
                TopicId = MixQueueTopics.MixLog,
                Id = data.Id,
                Sender = _subscriber.SubscriptionId,
                Data = ReflectionHelper.ParseObject(data).ToString(Newtonsoft.Json.Formatting.None),
                //Exception = ex,
                Success = false
            });
            return Task.CompletedTask;
        }

        public abstract Task Handler(MessageQueueModel model);
        #endregion
    }
}
