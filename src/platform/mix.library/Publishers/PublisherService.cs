using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Mix.Heart.Exceptions;
using Mix.Queue.Engines;
using Mix.Queue.Engines.MixQueue;
using Mix.Queue.Models.QueueSetting;
using Mix.Shared.Enums;

namespace Mix.Lib.Publishers
{
    public abstract class PublisherService : IHostedService
    {
        private readonly IQueueService<MessageQueueModel> _queueService;
        private readonly List<IQueuePublisher<MessageQueueModel>> _publishers;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        private const int MAX_CONSUME_LENGTH = 100;
        private readonly string _topicId;

        public PublisherService(
            string topicId,
            IQueueService<MessageQueueModel> queueService,
            IConfiguration configuration, IWebHostEnvironment environment, 
            MixMemoryMessageQueue<MessageQueueModel> queue)
        {
            _queueService = queueService;
            _configuration = configuration;
            _environment = environment;
            _topicId = topicId;
            _publishers = CreatePublisher(_topicId, queue);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(() =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var inQueueItems = _queueService.ConsumeQueue(MAX_CONSUME_LENGTH);

                    if (inQueueItems.Any() && _publishers != null)
                    {
                        Parallel.ForEach(_publishers, async publisher => { await publisher.SendMessages(inQueueItems); });
                    }
                }
            }, cancellationToken);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private List<IQueuePublisher<MessageQueueModel>> CreatePublisher(string topicName, 
            MixMemoryMessageQueue<MessageQueueModel> queue)
        {
            try
            {
                List<IQueuePublisher<MessageQueueModel>> queuePublishers = 
                    new List<IQueuePublisher<MessageQueueModel>>();
                var providerSetting = _configuration["MessageQueueSetting:Provider"];
                var settingPath = _configuration.GetSection("MessageQueueSetting:GoogleQueueSetting");
                var provider = Enum.Parse<MixQueueProvider>(providerSetting);

                switch (provider)
                {
                    case MixQueueProvider.GOOGLE:

                        var googleSetting = new GoogleQueueSetting();
                        settingPath.Bind(googleSetting);
                        googleSetting.CredentialFile = Path.Combine(_environment.ContentRootPath, googleSetting.CredentialFile);

                        queuePublishers.Add(
                            QueueEngineFactory.CreatePublisher<MessageQueueModel>(
                                provider, googleSetting, topicName));
                        break;
                    case MixQueueProvider.MIX:
                        var mixSetting = new MixQueueSetting();
                        settingPath.Bind(mixSetting);
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

    }
}
