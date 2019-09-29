using Microsoft.AspNetCore.SignalR;
using Mix.UI.Core.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mix.Cms.Hub
{
    public class ServiceHub : BaseSignalRHub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public Task SendMessageToCaller(string message)
        {
            return Clients.Caller.SendAsync("ReceiveMessage", message);
        }

        // TODO Handle Join/Leave group
        public Task JoinGroup(string groupName, string username)
        {
            string msg = $"new user {username}";
            AddUserToGroup(groupName, username);
            return Clients.Group(groupName).SendAsync("ReceiveMessage", msg );
        }

        private void AddUserToGroup(string groupName, string username)
        {
            throw new NotImplementedException();
        }

        public Task SendMessageToGroups(string message)
        {
            List<string> groups = new List<string>() { "SignalR Users" };
            return Clients.Groups(groups).SendAsync("ReceiveMessage", message);
        }

        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "SignalR Users");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "SignalR Users");

            await base.OnDisconnectedAsync(exception);
        }
    }
}
