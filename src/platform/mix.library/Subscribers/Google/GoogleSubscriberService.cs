using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Mix.Heart.Exceptions;
using Mix.Lib.Models;
using Mix.Queue.Engines;
using Mix.Queue.Interfaces;
using Mix.Queue.Models.QueueSetting;
using Mix.Shared.Enums;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mix.Lib.Subscribers.Google
{
    public abstract class GoogleSubscriberService<T> : IHostedService
    {
        private readonly IQueueSubscriber _subscriber;
        private readonly IConfiguration _configuration;
        private readonly string modelName;

        public GoogleSubscriberService(
            string subscriptionId,
            IConfiguration configuration)
        {
            _configuration = configuration;
            modelName = typeof(T).FullName;
            _subscriber = CreateSubscriber(subscriptionId);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(async () =>
            {
                if (_subscriber != null)
                {
                    await _subscriber.ProcessQueue();
                }
            }, cancellationToken);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private IQueueSubscriber CreateSubscriber(string subscriptionName)
        {
            try
            {
                var providerSetting = _configuration["MessageQueueSetting:Provider"];
                var provider = Enum.Parse<MixQueueProvider>(providerSetting);

                switch (provider)
                {
                    case MixQueueProvider.GOOGLE:

                        var googleSetting = new GoogleQueueSetting();
                        var settingPath = _configuration.GetSection(
                                            "MessageQueueSetting:GoogleQueueSetting");
                        settingPath.Bind(googleSetting);

                        return QueueEngineFactory.CreateGoogleSubscriber(
                            provider, googleSetting, subscriptionName, MesageHandler);
                }
            }
            catch (Exception ex)
            {
                throw new MixException(Heart.Enums.MixErrorStatus.ServerError, ex);
            }

            return default;
        }

        public Task MesageHandler(string body)
        {
            try
            {
                var data = JsonConvert.DeserializeObject<QueueMessageModel>(body);
                
                if (modelName != data.FullName)
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

        public abstract Task Handler(QueueMessageModel model);
    }
}
