using Mix.SignalR.Enums;
using Mix.SignalR.Models;
using System.Threading.Tasks;

namespace Mix.SignalR.Interfaces
{
    public interface IHubClientService
    {
        public Task SendMessageAsync(string title, string description, object data, MessageType messageType = MessageType.Info);

        public Task SendMessageAsync(SignalRMessageModel message);
    }
}
