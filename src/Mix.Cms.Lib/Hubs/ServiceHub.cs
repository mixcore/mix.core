using Microsoft.AspNetCore.SignalR;
using Mix.UI.Core.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mix.Cms.Hub
{
    public class ServiceHub : BaseSignalRHub
    {
        // TODO Handle Join/Leave group
        public void JoinGroup(string groupName, string username)
        {           
            AddUserToGroup(groupName, username);           
        }

        public Task LeaveGroup(string groupName)
        {
            string username = "test";
            string msg = $"new user {username}";
            string groupMsg = $"new user {username}";
            return Task.WhenAll(
               new Task[]{
                    SendMessageToCaller(msg),
                    SendMessageToGroups(groupMsg)
               }
           );
        }

        private void RemoveUserFromGroup(string groupName)
        {
            throw new NotImplementedException();
        }

        private Task AddUserToGroup(string groupName, string username)
        {
            string msg = $"new user {username}";
            string groupMsg = $"new user {username}";
            return Task.WhenAll(
               new Task[]{
                    SendMessageToCaller(msg),
                    SendMessageToGroups(groupMsg)
               }
           );
        }
        public Task SendMessageToConnectionId(string connectionId, string message)
        {
            return Clients.Client(connectionId).SendAsync("ReceiveMessage", message);
        }

        public async Task SendMessageToAll(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
        public Task SendMessageToCaller(string message)
        {
            return Clients.Caller.SendAsync("ReceiveMessage", message);
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
