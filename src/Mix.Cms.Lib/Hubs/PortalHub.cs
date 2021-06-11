using Microsoft.AspNetCore.SignalR;
using Mix.Heart.Infrastructure.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.SignalR.Hubs
{
    public class PortalHub : BaseSignalRHub
    {
        public async Task JoinRoom(string room)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, room);
            await SendMessageToGroups($"New member {Context.ConnectionId}", room);
        }

        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync(Constants.HubMethods.ReceiveMethod, message).ConfigureAwait(false);
        }

        public Task SendMessageToCaller(string message)
        {
            return Clients.Caller.SendAsync(Constants.HubMethods.ReceiveMethod, message);
        }

        public Task SendMessageToGroups(string message, string groupName)
        {
            return Clients.Group(groupName).SendAsync(Constants.HubMethods.ReceiveMethod, message);
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