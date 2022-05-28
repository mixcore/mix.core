using Microsoft.AspNetCore.SignalR;
using Mix.SignalR.Constants;
using Mix.SignalR.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace Mix.SignalR.Hubs
{
    public abstract class BaseSignalrHub: Hub
    {
        public async Task JoinRoom(string room)
        {
            var msg = new SignalRMessageModel<object>()
            {
                Title = $"New member joined {room}",
                Message = $"New member {Context.ConnectionId}"
            };
            await Groups.AddToGroupAsync(Context.ConnectionId, room);
            await SendMessageToGroups(msg.ToString(), room);
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
            return Clients.Group(groupName).SendAsync(HubMethods.ReceiveMethod, message);
        }


    }
}