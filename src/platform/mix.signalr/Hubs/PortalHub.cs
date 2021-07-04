using Microsoft.AspNetCore.SignalR;
using Mix.SignalR.Constants;
using System;
using System.Threading.Tasks;

namespace Mix.SignalR.Hubs
{
    public class PortalHub : Hub
    {
        public async Task JoinRoom(string room)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, room);
            await SendMessageToGroups($"New member {Context.ConnectionId}", room);
        }

        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync(HubMethods.ReceiveMethod, message).ConfigureAwait(false);
        }

        public Task SendMessageToCaller(string message)
        {
            return Clients.Caller.SendAsync(HubMethods.ReceiveMethod, message);
        }

        public Task SendMessageToGroups(string message, string groupName)
        {
            return Clients.Group(groupName).SendAsync(HubMethods.ReceiveMethod, message);
        }

        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "SignalR Users").ConfigureAwait(true);
            await base.OnConnectedAsync().ConfigureAwait(false);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "SignalR Users").ConfigureAwait(true);

            await base.OnDisconnectedAsync(exception).ConfigureAwait(false);
        }
    }
}