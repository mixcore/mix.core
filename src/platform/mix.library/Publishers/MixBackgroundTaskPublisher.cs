﻿using Microsoft.Extensions.Configuration;
using Mix.Queue.Engines;
using Mix.Queue.Engines.MixQueue;

namespace Mix.Lib.Publishers
{
    public class MixBackgroundTaskPublisher : PublisherBase
    {
        private const string TopicId = MixQueueTopics.MixBackgroundTasks;

        public MixBackgroundTaskPublisher(
            IQueueService<MessageQueueModel> queueService,
            IConfiguration configuration,
            MixMemoryMessageQueue<MessageQueueModel> queueMessage)
            : base(TopicId, queueService, configuration, queueMessage)
        {
        }
    }
}