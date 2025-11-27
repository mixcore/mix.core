using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Mix.Constant.Constants;
using Mix.Constant.Enums;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Heart.Exceptions;
using Mix.Mq.Lib.Models;
using Mix.Queue.Engines;
using Mix.Queue.Interfaces;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.MCP.Lib.Messenger
{
    public abstract class MqttPublisher : PublisherBase
    {
        protected MqttPublisher(string topicId,
            IMemoryQueueService<MessageQueueModel> queueService,
            IConfiguration configuration,
            MixEndpointService mixEndpointService,
            ILogger<PublisherBase> logger,
            IPooledObjectPolicy<IChannel>? rabbitMQObjectPolicy = null)
            : base(topicId, queueService, configuration, mixEndpointService, logger, rabbitMQObjectPolicy)
        {
        }

        public override List<IQueuePublisher<MessageQueueModel>>? CreatePublisher(string topicId)
        {
            try
            {
                var queuePublishers = new List<IQueuePublisher<MessageQueueModel>>
                {
                    QueueEngineFactory.CreatePublisher<MessageQueueModel>(
                       MixQueueProvider.MQTT,
                       Configuration,
                       topicId,
                       MixEndpointService)
                };
                return queuePublishers;
            }
            catch (Exception ex)
            {
                throw new MixException(Heart.Enums.MixErrorStatus.ServerError, ex);
            }
        }
    }
}
