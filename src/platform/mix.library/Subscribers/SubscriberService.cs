using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Mix.Heart.Exceptions;
using Mix.Queue.Engines;
using Mix.Queue.Engines.MixQueue;
using Mix.Queue.Models.QueueSetting;
using Mix.Shared.Enums;

namespace Mix.Lib.Subscribers
{
    public abstract class SubscriberService : IHostedService
    {
        private readonly IQueueSubscriber _subscriber;
        private readonly IConfiguration _configuration;
        private readonly MixMemoryMessageQueue<MessageQueueModel> _queueService;
        private readonly string _topicId;

        public SubscriberService(
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
                var settingPath = _configuration.GetSection(
                                            "MessageQueueSetting:GoogleQueueSetting");
                switch (provider)
                {
                    case MixQueueProvider.GOOGLE:
                        var googleSetting = new GoogleQueueSetting();
                        settingPath.Bind(googleSetting);
                        return QueueEngineFactory.CreateSubscriber(
                            provider, googleSetting,topicId, subscriptionId, MesageHandler, _queueService);
                    case MixQueueProvider.MIX:
                        var mixSetting = new MixQueueSetting();
                        settingPath.Bind(mixSetting);
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
                if (_topicId != data.FullName)
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
