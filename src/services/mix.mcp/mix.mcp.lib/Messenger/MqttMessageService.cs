using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Mix.Lib.Extensions;
using Mix.Mqtt.Lib.Helpers;
using Mix.Mqtt.Lib.Models;
using MQTTnet;
using System.Text;

namespace Mix.MCP.Lib.Messenger
{
    public class MqttMessageService : IMqttMessageService
    {
        private IMqttClient _mqttClient;
        private MqttClientOptions _mqttClientOptions;

        public bool IsConnected => _mqttClient?.IsConnected ?? false;

        public IConfiguration Configuration { get; }

        public MqttMessageService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async Task ConnectAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                if (_mqttClient != null && !_mqttClient.IsConnected)
                {
                    await _mqttClient.ConnectAsync(_mqttClientOptions, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                await Task.Delay(2000);
                Console.Error.WriteLine(ex.Message);
            }
        }

        public async Task SubscribeAsync(string topic, Func<string, Task> messageHandler, CancellationToken cancellationToken = default)
        {
            await InitMqttClient(cancellationToken);
            if (string.IsNullOrEmpty(topic) || messageHandler == null)
            {
                throw new ArgumentException("Topic and message handler must be provided.");
            }
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

            if (_mqttClient.IsConnected)
            {
                await _mqttClient.SubscribeAsync(topicFilter, cancellationToken);
            }
        }

        private async Task InitMqttClient(CancellationToken cancellationToken)
        {
            while (string.IsNullOrEmpty(Configuration.BaseUrl()))
            {
                Console.WriteLine("Base URL is not set in configuration. Waiting for it to be available...");
                await Task.Delay(5000, cancellationToken); // Wait for 1 second before retrying
            }

            var queueSetting = Configuration.GetSection("MessageQueueSettings:MQTT").Get<MQTTSetting>();
            if (string.IsNullOrEmpty(queueSetting?.HostName))
            {
                queueSetting = new MQTTSetting(Configuration.MqttWebSocketUrl());
            }
            var factory = new MqttClientFactory();
            if (!string.IsNullOrEmpty(queueSetting.HostName))
            {
                _mqttClient = factory.CreateMqttClient();
                _mqttClientOptions = MqttHelper.GetClientOptions(queueSetting);
            }
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