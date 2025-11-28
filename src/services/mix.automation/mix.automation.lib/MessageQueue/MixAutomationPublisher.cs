using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Mq.Lib.Models;
using Mix.Queue.Engines;
using Mix.Queue.Interfaces;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Automation.Lib.MessageQueue
{
    public sealed class MixAutomationPublisher : PublisherBase
    {
        public MixAutomationPublisher(IMemoryQueueService<MessageQueueModel> queueService, IConfiguration configuration, MixEndpointService mixEndpointService, ILogger<PublisherBase> logger, IPooledObjectPolicy<IChannel>? rabbitMQObjectPolicy = null) 
            : base(MixAutomationConstants.Topics.MixAutomation, queueService, configuration, mixEndpointService, logger, rabbitMQObjectPolicy)
        {
        }
    }
}
