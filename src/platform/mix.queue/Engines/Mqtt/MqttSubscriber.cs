using Mix.Mq.Lib.Models;
using Mix.Queue.Interfaces;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Packets;
using System.Text.Json;
using Mix.Mqtt.Lib.Helpers;
using Mix.Mqtt.Lib.Models;

namespace Mix.Queue.Engines.Mqtt
{
    public class MqttSubscriber<T> : IQueueSubscriber
       where T : MessageQueueModel
    {
        public string SubscriptionId { get; set; }
        public bool IsProcessing { get; private set; }
        private readonly Func<T, Task> _messageHandler;
        private readonly IMqttClient _mqttClient;
        private string _topicId;
        private MqttTopicFilter _topic;
        private MqttClientFactory _mqttFactory;
        private MqttClientOptions _mqttClientOptions;
        private MqttClientSubscribeOptions _mqttSubscribeOptions;

        public MqttSubscriber(MQTTSetting? queueSetting, string topicName, Func<T, Task> handler)
        {
            _topicId = topicName;
            _messageHandler = handler;
            _topic = new MqttTopicFilterBuilder().WithTopic(_topicId)
                .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce)
                .Build();
            _mqttFactory = new MqttClientFactory();
            _mqttClient = _mqttFactory.CreateMqttClient();
            _mqttClientOptions = MqttHelper.GetClientOptions(queueSetting);
            _mqttSubscribeOptions = _mqttFactory.CreateSubscribeOptionsBuilder().WithTopicFilter(_topic).Build();
        }

        /// <summary>
        /// Process message queue
        /// </summary>
        /// <returns></returns>
        public async Task ProcessQueue(CancellationToken cancellationToken = default)
        {
            try
            {
                _mqttClient.ApplicationMessageReceivedAsync += async e =>
                {
                    if (e.ApplicationMessage.Payload.Length > 0)
                    {
                        Console.WriteLine($"{SubscriptionId} Received application message.");
                        var msg = JsonSerializer.Deserialize<T>(Encoding.UTF8.GetString(e.ApplicationMessage.Payload));
                        await _messageHandler(msg);
                        await e.AcknowledgeAsync(cancellationToken);
                    }
                };

                while (!cancellationToken.IsCancellationRequested)
                {
                    if (!_mqttClient.IsConnected)
                    {
                        await _mqttClient.ConnectAsync(_mqttClientOptions, CancellationToken.None);
                        Console.WriteLine($"MQTT client subscribed to topic: {_topicId}.");
                    }
                    await _mqttClient.SubscribeAsync(_mqttSubscribeOptions, cancellationToken);
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task Disconnect(CancellationToken cancellationToken = default)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                await _mqttClient.DisconnectAsync();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
            }
        }

    }
}
