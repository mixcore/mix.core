using Grpc.Core;
using Microsoft.Extensions.Hosting;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Heart.Helpers;
using Mix.Mq.Lib.Models;
using Mix.Mq;
using Mix.Queue.Interfaces;
using Mix.Queue.Models.QueueSetting;
using Mix.Queue.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Packets;
using Mix.Mqtt.Lib.Extensions;
using Microsoft.VisualBasic;
using System.Text.Json;
using Google.Protobuf.Compiler;

namespace Mix.Queue.Engines.Mqtt
{
    public class MqttSubscriber<T> : IQueueSubscriber
       where T : MessageQueueModel
    {
        public string SubscriptionId { get; set; }
        public bool IsProcessing { get; private set; }
        private readonly string _subscriptionId;
        private readonly MixQueueSetting _queueSetting;
        private readonly Func<T, Task> _messageHandler;
        private readonly IMemoryQueueService<MessageQueueModel> _memQueues;
        private readonly IMqttClient _mqttClient;
        private string _topicId;
        private MqttTopicFilter _topic;
        private MqttClientFactory _mqttFactory;
        private MqttClientOptions _mqttClientOptions;
        private MqttClientSubscribeOptions _mqttSubscribeOptions;
        private readonly MixEndpointService _mixEndpointService;

        public MqttSubscriber(IQueueSetting queueSetting, string topicName, MixEndpointService mixEndpointService, Func<T, Task> handler)
        {
            _topicId = topicName;
            _messageHandler = handler;
            _topic = new MqttTopicFilterBuilder().WithTopic(_topicId)
                .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce)
                .Build();
            _mixEndpointService = mixEndpointService;
            _mqttFactory = new MqttClientFactory();
            _mqttClient = _mqttFactory.CreateMqttClient();
            _mqttClientOptions = new MqttClientOptionsBuilder()
                .WithTcpServer("localhost", 1883)
                .Build();
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
                    Console.WriteLine($"{SubscriptionId} Received application message.");
                    var msg = JsonSerializer.Deserialize<T>(Encoding.UTF8.GetString(e.ApplicationMessage.Payload));
                    await _messageHandler(msg);
                    await e.AcknowledgeAsync(cancellationToken);
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
