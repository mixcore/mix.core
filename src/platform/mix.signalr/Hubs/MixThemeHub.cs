using Microsoft.AspNetCore.SignalR;
using Mix.SignalR.Constants;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace Mix.SignalR.Hubs
{
    public class MixThemeHub : Hub
    {
        public async Task JoinRoom(string room)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, room);
            var obj = new JObject(new JProperty("message", $"New member {Context.ConnectionId}"));
            await SendMessageToGroups(obj.ToString(), room);
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