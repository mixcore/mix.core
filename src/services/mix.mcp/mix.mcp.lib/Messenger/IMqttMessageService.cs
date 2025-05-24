using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mix.MCP.Lib.Messenger
{
    public interface IMqttMessageService
    {
        Task ConnectAsync(CancellationToken cancellationToken = default);
        Task SubscribeAsync(string topic, Func<string, Task> messageHandler, CancellationToken cancellationToken = default);
        Task PublishAsync(string topic, string payload, CancellationToken cancellationToken = default);
        Task DisconnectAsync(CancellationToken cancellationToken = default);
        bool IsConnected { get; }
    }
}