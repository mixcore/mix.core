using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Mix.Constant.Constants;
using Mix.Database.Services.MixGlobalSettings;
using Mix.MCP.Lib.Constants;
using Mix.MCP.Lib.Models;
using Mix.Mq.Lib.Models;
using Mix.Mqtt.Lib.Helpers;
using Mix.Mqtt.Lib.Models;
using Mix.Queue.Engines;
using Mix.Queue.Interfaces;
using Mix.Queue.Models.QueueSetting;
using MQTTnet;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Mix.MCP.Lib.Messenger
{
    public sealed class ChatPublisher : MqttPublisher
    {
        public ChatPublisher(
            IMemoryQueueService<MessageQueueModel> queueService,
            IConfiguration configuration,
            MixEndpointService mixEndpointService,
            ILogger<PublisherBase> logger) : base(LLMChatTopics.LLMChat, queueService, configuration, mixEndpointService, logger)
        {
        }
    }
}
