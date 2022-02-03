using Microsoft.AspNetCore.SignalR;
using Mix.Cms.Lib.Models.Messenger;
using Mix.Heart.Infrastructure.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.SignalR.Hubs
{
    public class EditFileHub : Hub
    {
        public static Dictionary<string, List<HubUser>> Rooms = new Dictionary<string, List<HubUser>>();
        public async Task JoinRoom(string roomName, HubUser user)
        {
            if (!Rooms.ContainsKey(roomName))
            {
                Rooms.Add(roomName, new List<HubUser>());
            }

            var users = Rooms[roomName];
            user.ConnectionId = Context.ConnectionId;
            var currentUser = users.Find(u => u.Username == user.Username);
            if (currentUser == null)
            {
                users.Add(user);
                await SendMessageToGroup(new { type = "MemberJoin", data = user }, roomName);
                Rooms[roomName] = users;
            }
            else
            {
                currentUser.ConnectionId = Context.ConnectionId;
            }
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
            await SendMessageToGroup(new { type = "MemberList", data = users }, roomName);
        }

        public async Task LeaveRoom(string roomName, HubUser user)
        {
            if (Rooms.ContainsKey(roomName))
            {

                var users = Rooms[roomName] ?? new List<HubUser>();
                var currentUser = users.First(u => u.Username == user.Username);
                if (currentUser != null)
                {
                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName).ConfigureAwait(true);
                    users.Remove(currentUser);
                    Rooms[roomName] = users;
                    await SendMessageToGroup(new { type = "MemberList", data = users }, roomName);
                }
            }
        }

        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync(Constants.HubMethods.ReceiveMethod, message).ConfigureAwait(false);
        }

        public Task SendMessageToCaller(string message)
        {
            return Clients.Caller.SendAsync(Constants.HubMethods.ReceiveMethod, message);
        }

        public Task SendMessageToGroup(object message, string groupName)
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
            foreach (var roomName in Rooms.Keys)
            {
                Rooms[roomName].RemoveAll(m => m.ConnectionId == Context.ConnectionId);
                await SendMessageToGroup(new { type = "MemberList", data = Rooms[roomName]}, roomName);
            }
        }
    }
}