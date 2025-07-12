using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Mix.MCP.Lib.Agents;
using Mix.MCP.Lib.Constants;
using Mix.MCP.Lib.Models;
using Mix.Mq.Lib.Models;
using Mix.Queue.Interfaces;
using MQTTnet;
using Newtonsoft.Json.Linq;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mix.MCP.Lib.Messenger
{
    public class LLMChatHostedService : Microsoft.Extensions.Hosting.BackgroundService
    {
        private readonly RoutingAgent _routingAgent;
        private readonly IMqttMessageService _mqttService;
        private readonly ILogger<LLMChatHostedService> _logger;
        private bool _isSubscribed = false;

        public LLMChatHostedService(
            IServiceProvider servicesProvider,
            IConfiguration configuration,
            IMemoryQueueService<MessageQueueModel> queueService,
            ILogger<LLMChatHostedService> logger,
            RoutingAgent routingAgent)
        {
            _routingAgent = routingAgent;
            _mqttService = new MqttMessageService(configuration);
            _logger = logger;

        }

        public async Task Handler(LLMMessage msg, CancellationToken cancellationToken)
        {
            var result = await _routingAgent.ProcessInputAsync(msg.Data.Content, msg.DeviceId, msg.SessionId, msg.ServiceType, cancellationToken);
            await SendMessage(msg.DeviceId, result, cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("LLMChatHostedService is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await SubscribeLLMChatAsync(stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    _logger.LogInformation("LLMChatHostedService is stopping due to cancellation request.");
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in LLMChatHostedService. Retrying in 5 seconds...");
                    _isSubscribed = false;
                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                }
            }
            await _mqttService.DisconnectAsync(stoppingToken);
            _logger.LogInformation("LLMChatHostedService is stopping.");
        }

        private async Task SubscribeLLMChatAsync(CancellationToken stoppingToken)
        {
            if (!_mqttService.IsConnected || !_isSubscribed)
            {
                _logger.LogInformation("Attempting to connect and subscribe to MQTT broker...");
                await _mqttService.SubscribeAsync(LLMChatTopics.LLMChat, async payload =>
                {
                    try
                    {
                        var msg = JObject.Parse(payload).ToObject<LLMMessage>();
                        if (msg != null)
                        {
                            await Handler(msg, stoppingToken);
                        }
                        else
                        {
                            _logger.LogWarning("Received null message from MQTT topic: {Topic}", LLMChatTopics.LLMChat);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to process MQTT message: {Payload}", payload);
                    }
                }, stoppingToken);
                _isSubscribed = true;
                _logger.LogInformation("Successfully subscribed to MQTT topic: {Topic}", LLMChatTopics.LLMChat);
            }
        }

        public async Task SendMessage(string deviceId, string content, CancellationToken cancellationToken)
        {
            try
            {
                await _mqttService.PublishAsync(deviceId, content, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cannot publish message to {DeviceId}", deviceId);
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("LLMChatHostedService is stopping...");

            try
            {
                await _mqttService.DisconnectAsync(cancellationToken);
                _logger.LogInformation("Successfully disconnected from MQTT broker.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error disconnecting from MQTT broker.");
            }

            await base.StopAsync(cancellationToken);
        }

        public async Task Disconnect(CancellationToken cancellationToken = default)
        {
            try
            {
                await _mqttService.DisconnectAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error disconnecting MQTT client.");
            }
        }
    }
}
