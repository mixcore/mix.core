using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Mix.Constant.Constants;
using Mix.Database.Services.MixGlobalSettings;
using Mix.MCP.Lib.Models;
using Mix.Mq.Lib.Models;
using Mix.Mqtt.Lib.Helpers;
using Mix.Mqtt.Lib.Models;
using Mix.Queue.Engines;
using Mix.Queue.Engines.Mqtt;
using Mix.Queue.Interfaces;
using Mix.Queue.Models.QueueSetting;
using MQTTnet;
using MQTTnet.Channel;
using MQTTnet.Packets;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Mix.MCP.Lib.Messenger
{
    public abstract class MqttService : BackgroundService
    {
        protected readonly IMqttClient _mqttClient;
        protected MqttTopicFilter _topic;
        protected MqttClientFactory _mqttFactory;
        protected MqttClientOptions _mqttClientOptions;
        protected MqttClientSubscribeOptions _mqttSubscribeOptions;

        public MqttService(
            string topicId,
            string moduleName,
            int timeout,
            IServiceProvider servicesProvider,
            IConfiguration configuration,
            IMemoryQueueService<MessageQueueModel> queueService,
            ILogger<SubscriberBase> logger,
            IPooledObjectPolicy<IChannel>? rabbitMQObjectPolicy = null)

        {
            _topic = new MqttTopicFilterBuilder().WithTopic(topicId)
               .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce)
               .Build();
            var queueSetting = configuration.GetSection($"{MixAppSettingsSection.MessageQueueSettings}:MQTT").Get<MQTTSetting>();
            _mqttFactory = new MqttClientFactory();
            _mqttClient = _mqttFactory.CreateMqttClient();
            _mqttClientOptions = MqttHelper.GetClientOptions(queueSetting);
            _mqttSubscribeOptions = _mqttFactory.CreateSubscribeOptionsBuilder().WithTopicFilter(_topic).Build();
        }
    }
}
