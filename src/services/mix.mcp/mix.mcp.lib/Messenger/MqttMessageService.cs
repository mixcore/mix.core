using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Mix.Mqtt.Lib.Helpers;
using Mix.Mqtt.Lib.Models;
using MQTTnet;
using System.Text;

namespace Mix.MCP.Lib.Messenger
{
    public class MqttMessageService : IMqttMessageService
    {
        private readonly IMqttClient _mqttClient;
        private readonly MqttClientOptions _mqttClientOptions;

        public bool IsConnected => _mqttClient.IsConnected;

        public MqttMessageService(IConfiguration configuration)
        {
            var queueSetting = configuration.GetSection("MessageQueueSettings:MQTT").Get<MQTTSetting>();
            var factory = new MqttClientFactory();
            _mqttClient = factory.CreateMqttClient();
            _mqttClientOptions = MqttHelper.GetClientOptions(queueSetting);
        }

        public async Task ConnectAsync(CancellationToken cancellationToken = default)
        {
            if (!_mqttClient.IsConnected)
            {
                await _mqttClient.ConnectAsync(_mqttClientOptions, cancellationToken);
            }
        }

        public async Task SubscribeAsync(string topic, Func<string, Task> messageHandler, CancellationToken cancellationToken = default)
        {
            await ConnectAsync(cancellationToken);
            var topicFilter = new MqttTopicFilterBuilder()
                .WithTopic(topic)
                .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce)
                .Build();

            _mqttClient.ApplicationMessageReceivedAsync += async e =>
            {
                if (e.ApplicationMessage.Payload.Length > 0)
                {
                    var payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                    await messageHandler(payload);
                }
            };

            await _mqttClient.SubscribeAsync(topicFilter, cancellationToken);
        }

        public async Task PublishAsync(string topic, string payload, CancellationToken cancellationToken = default)
        {
            await ConnectAsync(cancellationToken);
            var message = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(payload)
                .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce)
                .Build();
            await _mqttClient.PublishAsync(message, cancellationToken);
        }

        public async Task DisconnectAsync(CancellationToken cancellationToken = default)
        {
            if (_mqttClient.IsConnected)
            {
                await _mqttClient.DisconnectAsync();
            }
        }
    }
}