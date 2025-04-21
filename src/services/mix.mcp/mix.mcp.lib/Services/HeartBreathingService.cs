using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Mix.Mcp.Lib.Entities;
using Mix.Mcp.Lib.Models;
using Mix.Mq.Lib;
using Mix.Mq.Lib.Models;
using Mix.Mqtt.Lib.Helpers;
using Mix.Mqtt.Lib.Models;
using Mix.Shared.Services;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;

namespace Mix.Mcp.Lib.Services
{
    public class HeartBreathingService : IHeartBreathingService
    {
        private readonly ILogger<HeartBreathingService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IMixMemoryCacheService _cacheService;
        private readonly IMixMqttService _mqttService;
        private readonly IMqttClient _mqttClient;
        private readonly string _topic = "sensors/heartbeat";
        private readonly string _deviceId;
        private readonly MqttClientOptions _mqttClientOptions;
        private readonly MqttClientSubscribeOptions _mqttSubscribeOptions;
        private readonly MqttTopicFilter _topicFilter;

        public HeartBreathingService(
            ILogger<HeartBreathingService> logger,
            IConfiguration configuration,
            IMixMemoryCacheService cacheService,
            IMixMqttService mqttService)
        {
            _logger = logger;
            _configuration = configuration;
            _cacheService = cacheService;
            _mqttService = mqttService;
            _deviceId = _configuration["DeviceId"] ?? "default-device";

            // Khởi tạo MQTT client
            var mqttFactory = new MqttFactory();
            _mqttClient = mqttFactory.CreateMqttClient();

            // Cấu hình MQTT options
            var mqttSettings = new MQTTSetting
            {
                Server = _configuration["Mqtt:Server"],
                Port = int.Parse(_configuration["Mqtt:Port"]),
                ClientId = $"heartbeat_sensor_{_deviceId}",
                CleanSession = true
            };

            _mqttClientOptions = MqttHelper.GetClientOptions(mqttSettings);
            _topicFilter = new MqttTopicFilterBuilder()
                .WithTopic(_topic)
                .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce)
                .Build();

            _mqttSubscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder()
                .WithTopicFilter(_topicFilter)
                .Build();

            // Đăng ký xử lý message
            _mqttClient.ApplicationMessageReceivedAsync += async e =>
            {
                try
                {
                    if (e.ApplicationMessage.Payload.Length > 0)
                    {
                        var message = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                        await ProcessSensorData(message);
                        await e.AcknowledgeAsync(CancellationToken.None);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing MQTT message");
                }
            };
        }

        public async Task StartAsync()
        {
            try
            {
                while (true)
                {
                    if (!_mqttClient.IsConnected)
                    {
                        await _mqttClient.ConnectAsync(_mqttClientOptions, CancellationToken.None);
                        _logger.LogInformation("Connected to MQTT broker");
                    }

                    await _mqttClient.SubscribeAsync(_mqttSubscribeOptions, CancellationToken.None);
                    _logger.LogInformation("Subscribed to topic: {Topic}", _topic);

                    await Task.Delay(TimeSpan.FromSeconds(5), CancellationToken.None);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error starting MQTT client");
                throw;
            }
        }

        private async Task ProcessSensorData(string message)
        {
            try
            {
                var sensorData = JsonSerializer.Deserialize<SensorDataModel>(message);
                if (sensorData == null)
                {
                    _logger.LogWarning("Invalid sensor data format");
                    return;
                }

                // Lấy thông tin người dùng từ target đầu tiên
                var target = sensorData.Human?.Targets?.FirstOrDefault();
                var patientId = target != null ? $"patient_{target.ClusterIndex}" : "unknown";

                var heartBreathing = new HeartBreathing
                {
                    DeviceId = _deviceId,
                    PatientId = patientId,
                    HeartRate = sensorData.Heart.HeartRate,
                    BreathingRate = sensorData.Heart.BreathRate,
                    SensorData = JsonDocument.Parse(JsonSerializer.Serialize(sensorData)),
                    IsAlert = CheckForAlerts(sensorData.Heart),
                    AlertType = GetAlertType(sensorData.Heart)
                };

                // Lưu vào cache để xử lý real-time
                await _cacheService.SetAsync($"heartbeat_{_deviceId}_{patientId}", 
                    heartBreathing, TimeSpan.FromMinutes(5));

                // Gửi message để lưu vào database
                await _mqttService.PublishAsync(new MixPublishMessageRequest
                {
                    TopicId = "heartbeat_data",
                    Data = JsonSerializer.Serialize(heartBreathing)
                });

                _logger.LogInformation("Processed sensor data for device {DeviceId} and patient {PatientId}", 
                    _deviceId, patientId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing sensor data");
            }
        }

        private bool CheckForAlerts(HeartData data)
        {
            // Kiểm tra các ngưỡng cảnh báo
            return data.HeartRate < 60 || data.HeartRate > 100 ||
                   data.BreathRate < 12 || data.BreathRate > 20;
        }

        private string GetAlertType(HeartData data)
        {
            if (data.HeartRate < 60) return "LowHeartRate";
            if (data.HeartRate > 100) return "HighHeartRate";
            if (data.BreathRate < 12) return "LowBreathingRate";
            if (data.BreathRate > 20) return "HighBreathingRate";
            return "Normal";
        }

        public async Task StopAsync()
        {
            try
            {
                await _mqttClient.DisconnectAsync();
                _logger.LogInformation("Disconnected from MQTT broker");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error disconnecting from MQTT broker");
            }
        }
    }

    public interface IHeartBreathingService
    {
        Task StartAsync();
        Task StopAsync();
    }
} 