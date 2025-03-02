using Microsoft.AspNetCore.SignalR.Client;
using Mix.SignalR.Enums;
using Mix.SignalR.Models;
using System.Threading.Tasks;

namespace Mix.SignalR.Interfaces
{
    public interface IHubClientService
    {
        HubConnection Connection { get; set; }
        public Task SendMessageAsync(string title, string description, object data, MessageType messageType = MessageType.Info);

        public Task SendMessageAsync(SignalRMessageModel message);
        public Task SendPrivateMessageAsync(SignalRMessageModel message, string connectionId, bool selfReceive = false);
        Task StartConnection();
    }
}
