using Mix.Database.Services.MixGlobalSettings;
using Mix.Mq.Lib.Models;
using Mix.Mqtt.Lib.Helpers;
using Mix.Mqtt.Lib.Models;
using Mix.Queue.Interfaces;
using MQTTnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Mix.Queue.Engines.Mqtt
{
    public class MqttPublisher<T> : IQueuePublisher<T>
        where T : MessageQueueModel
    {
        private readonly IMqttClient _mqttClient;
        private string _topicId;
        private readonly MixEndpointService _mixEndpointService;
        private MqttClientOptions _mqttClientOptions;
        public MqttPublisher(MQTTSetting? queueSetting, string topicName, MixEndpointService mixEndpointService)
        {
            _topicId = topicName;
            _mixEndpointService = mixEndpointService;
            var mqttFactory = new MqttClientFactory();
            _mqttClient = mqttFactory.CreateMqttClient();
            _mqttClientOptions = MqttHelper.GetClientOptions(queueSetting);

        }

        public async Task SendMessage(T message)
        {
            try
            {
                if (!_mqttClient.IsConnected)
                {
                    await _mqttClient.ConnectAsync(_mqttClientOptions, CancellationToken.None);
                }
                if (message.Id == default)
                {
                    message.Id = Guid.NewGuid();
                }
                message.CreatedDate = DateTime.UtcNow;
                await _mqttClient.PublishAsync(new MqttApplicationMessageBuilder().WithTopic(_topicId).WithPayload(JsonSerializer.SerializeToUtf8Bytes(message)).Build());
                await _mqttClient.DisconnectAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Cannot publish message to {_mixEndpointService.MixMq}");
                Console.WriteLine(ex);
            }
        }

        public async Task SendMessages(IList<T> messages)
        {
            var publishTasks =
               messages.Select(async message =>
               {
                   await SendMessage(message);
               });
            await Task.WhenAll(publishTasks);
        }

        public async Task StopAsync()
        {
            await _mqttClient.DisconnectAsync();
        }
    }
}
