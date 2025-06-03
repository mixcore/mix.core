using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Heart.Exceptions;
using Mix.Heart.Helpers;
using Mix.Heart.Services;
using Mix.Mq.Lib.Models;
using Mix.Queue.Engines.MixQueue;
using Mix.Queue.Engines.RabbitMQ;
using Mix.Queue.Interfaces;
using Mix.Queue.Models.QueueSetting;
using Mix.Shared.Services;
using RabbitMQ.Client;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mix.Queue.Engines
{
    public abstract class SubscriberBase : BackgroundService
    {
        protected readonly IMemoryQueueService<MessageQueueModel> _memoryQueueService;
        protected readonly IConfiguration _configuration;
        protected readonly string _topicId;
        protected readonly string _moduleName;
        protected readonly int _timeout;
        protected readonly IServiceProvider ServicesProvider;
        protected readonly ILogger<SubscriberBase> Logger;

        protected MixCacheService CacheService;
        protected IQueueSubscriber? _subscriber;
        protected IServiceScope? ServiceScope { get; set; }

        private readonly IPooledObjectPolicy<IChannel>? _rabbitMQObjectPolicy;

        protected SubscriberBase(
            string topicId,
            string moduleName,
            int timeout,
            IServiceProvider servicesProvider,
            IConfiguration configuration,
            IMemoryQueueService<MessageQueueModel> queueService,
            ILogger<SubscriberBase> logger,
            IPooledObjectPolicy<IChannel>? rabbitMQObjectPolicy = null)
        {
            _timeout = timeout;
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _topicId = topicId ?? throw new ArgumentNullException(nameof(topicId));
            _moduleName = moduleName?.ToLower() ?? throw new ArgumentNullException(nameof(moduleName));
            _memoryQueueService = queueService ?? throw new ArgumentNullException(nameof(queueService));
            _rabbitMQObjectPolicy = rabbitMQObjectPolicy;

            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            ServicesProvider = servicesProvider ?? throw new ArgumentNullException(nameof(servicesProvider));
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            _subscriber = CreateSubscriber(_topicId, $"{_topicId}.{_moduleName}");

            if (_subscriber == null)
            {
                throw new InvalidOperationException("Subscriber is not set");
            }

            if (_subscriber is not RabbitMQSubscriber<MessageQueueModel>)
            {
                await StartProcessQueue(cancellationToken).ConfigureAwait(false);
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_subscriber == null)
            {
                return;
            }

            Logger.LogInformation("{SubscriptionId} stopped at {DateTime}", _subscriber.SubscriptionId, DateTime.UtcNow);

            if (_subscriber is MixQueueSubscriber<MessageQueueModel> mixQueueSubscriber)
            {
                await mixQueueSubscriber.Disconnect(cancellationToken).ConfigureAwait(false);
            }

            await base.StopAsync(cancellationToken).ConfigureAwait(false);
        }

        private async Task StartProcessQueue(CancellationToken cancellationToken)
        {
            if (_subscriber == null)
            {
                throw new InvalidOperationException("Subscriber is not initialized");
            }

            Logger.LogInformation("StartProcessQueue: {SubscriptionId} starting at {DateTime}",
                _subscriber.SubscriptionId, DateTime.UtcNow.AddHours(7));

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    await _subscriber.ProcessQueue(cancellationToken).ConfigureAwait(false);
                    Logger.LogInformation("StartProcessQueue: {SubscriptionId} started at {DateTime}",
                        _subscriber.SubscriptionId, DateTime.UtcNow.AddHours(7));
                }
                catch (Exception ex)
                {
                    await HandleSubscriberError(ex, cancellationToken).ConfigureAwait(false);
                }
            }

            await Task.Delay(1000, cancellationToken).ConfigureAwait(false);

            if (_subscriber is MixQueueSubscriber<MessageQueueModel> mixQueueSubscriber)
            {
                await mixQueueSubscriber.Disconnect().ConfigureAwait(false);
            }

            Logger.LogInformation("StartProcessQueue: {SubscriptionId} stopped at {DateTime}",
                _subscriber.SubscriptionId, DateTime.UtcNow.AddHours(7));
        }

        private async Task HandleSubscriberError(Exception ex, CancellationToken cancellationToken)
        {
            if (_subscriber == null)
            {
                return;
            }

            if (_subscriber is MixQueueSubscriber<MessageQueueModel> mixQueueSubscriber)
            {
                await mixQueueSubscriber.Disconnect(cancellationToken).ConfigureAwait(false);
            }

            Logger.LogError(ex, "StartProcessQueue: {SubscriptionId} is broken at {DateTime}, Trying to reconnect from client: {Message}",
                _subscriber.SubscriptionId, DateTime.UtcNow.AddHours(7), ex.Message);

            await Task.Delay(2000, cancellationToken).ConfigureAwait(false);
            _subscriber = CreateSubscriber(_topicId, _subscriber.SubscriptionId);
            await StartProcessQueue(cancellationToken).ConfigureAwait(false);
        }

        public virtual IQueueSubscriber? CreateSubscriber(string topicId, string subscriptionId)
        {
            try
            {
                var providerSetting = _configuration["MessageQueueSettings:Provider"];
                if (string.IsNullOrEmpty(providerSetting))
                {
                    return null;
                }

                var provider = Enum.Parse<MixQueueProvider>(providerSetting);
                var mixEndpointService = GetRequiredService<MixEndpointService>();

                return provider switch
                {
                    MixQueueProvider.AZURE or MixQueueProvider.GOOGLE or MixQueueProvider.MIX or MixQueueProvider.MQTT
                        => QueueEngineFactory.CreateSubscriber<MessageQueueModel>(
                            provider, _configuration, topicId, subscriptionId, MessageHandler, _memoryQueueService, mixEndpointService),
                    MixQueueProvider.RABBITMQ
                        => QueueEngineFactory.CreateRabbitMQSubscriber<MessageQueueModel>(
                            _rabbitMQObjectPolicy, topicId, subscriptionId, MessageHandler),
                    _ => null
                };
            }
            catch (Exception ex)
            {
                throw new MixException(Heart.Enums.MixErrorStatus.ServerError, ex);
            }
        }

        protected T? GetRequiredService<T>()
        {
            ServiceScope ??= ServicesProvider.CreateScope();
            return ServiceScope.ServiceProvider.GetRequiredService<T>();
        }

        public virtual async Task MessageHandler(MessageQueueModel data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            try
            {
                using var timeoutCancellationSource = new CancellationTokenSource(TimeSpan.FromSeconds(_timeout));
                {
                    if (_topicId != data.TopicId)
                    {
                        return;
                    }
                    await Handler(data, timeoutCancellationSource.Token).ConfigureAwait(false);
                }
            }
            catch (OperationCanceledException ex)
            {
                await HandleDeadLetter(data).ConfigureAwait(false);
                await HandleException(data, ex).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await HandleException(data, ex).ConfigureAwait(false);
            }
            finally
            {
                ServiceScope?.Dispose();
                ServiceScope = null;
            }
        }

        public virtual Task HandleDeadLetter(MessageQueueModel message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            _memoryQueueService.PushMemoryQueue(new MessageQueueModel(1)
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
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (ex == null)
            {
                throw new ArgumentNullException(nameof(ex));
            }

            if (_subscriber == null)
            {
                throw new InvalidOperationException("Subscriber is not initialized");
            }

            _memoryQueueService.PushMemoryQueue(new MessageQueueModel(1)
            {
                Action = MixQueueActions.QueueFailed,
                TopicId = MixQueueTopics.MixLog,
                Id = data.Id,
                Sender = _subscriber.SubscriptionId,
                Data = ReflectionHelper.ParseObject(data).ToString(Newtonsoft.Json.Formatting.None),
                Success = false
            });
            return Task.CompletedTask;
        }

        public abstract Task Handler(MessageQueueModel model, CancellationToken cancellationToken);
    }
}
