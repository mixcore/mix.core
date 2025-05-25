using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Mix.MCP.Lib.Agents;
using Mix.MCP.Lib.Constants;
using Mix.MCP.Lib.Models;
using Mix.Mq.Lib.Models;
using Mix.Queue.Interfaces;
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
            await _mqttService.SubscribeAsync(LLMChatTopics.LLMChat, async payload =>
            {
                try
                {
                    var msg = JObject.Parse(payload).ToObject<LLMMessage>();
                    await Handler(msg, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to process MQTT message.");
                }
            }, stoppingToken);
        }

        public async Task SendMessage(string deviceId, string content, CancellationToken cancellationToken)
        {
            try
            {
                await _mqttService.PublishAsync(deviceId, content, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Cannot publish message to {deviceId}");
            }
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
