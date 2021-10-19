using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Mix.Heart.Exceptions;
using Mix.Lib.Models;
using Mix.Queue.Engines;
using Mix.Queue.Interfaces;
using Mix.Queue.Models.QueueSetting;
using Mix.Shared.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mix.Lib.Publishers.Google
{
    public class GooglePublisherService : IHostedService
    {
        private readonly IQueueService<QueueMessageModel> _queueService;
        private readonly List<IQueuePublisher<QueueMessageModel>> _publishers;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        private const int MAX_CONSUME_LENGTH = 100;

        public GooglePublisherService(
            IQueueService<QueueMessageModel> queueService,
            IConfiguration configuration, IWebHostEnvironment environment)
        {
            _queueService = queueService;
            _configuration = configuration;
            _environment = environment;
            _publishers = CreatePublisher();
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

        private List<IQueuePublisher<QueueMessageModel>> CreatePublisher()
        {
            try
            {
                List<IQueuePublisher<QueueMessageModel>> queuePublishers = new List<IQueuePublisher<QueueMessageModel>>();
                var providerSetting = _configuration["MessageQueueSetting:Provider"];
                var provider = Enum.Parse<MixQueueProvider>(providerSetting);

                switch (provider)
                {
                    case MixQueueProvider.GOOGLE:

                        var googleSetting = new GoogleQueueSetting();
                        var settingPath = _configuration.GetSection("MessageQueueSetting:GoogleQueueSetting");
                        settingPath.Bind(googleSetting);
                        googleSetting.CredentialFile = Path.Combine(_environment.ContentRootPath, googleSetting.CredentialFile);

                        queuePublishers.Add(QueueEngineFactory.CreateGooglePublisher<QueueMessageModel>(provider, googleSetting, "ModelCreatedQueue"));
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
