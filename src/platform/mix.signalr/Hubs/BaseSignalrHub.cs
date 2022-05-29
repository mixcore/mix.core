using Microsoft.AspNetCore.SignalR;
using Mix.SignalR.Constants;
using Mix.SignalR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mix.SignalR.Hubs
{
    public abstract class BaseSignalRHub : Hub
    {
        public static Dictionary<string, List<HubUserModel>> Rooms = new Dictionary<string, List<HubUserModel>>();
        public virtual async Task JoinRoom(string roomName)
        {
            var msg = new SignalRMessageModel()
            {
                Title = $"New member joined {roomName}",
                Message = $"New member {Context.ConnectionId}"
            };

            await AddUserToRoom(roomName);
            await SendMessageToGroups(msg, roomName);
        }

        public virtual async Task SendMessage(SignalRMessageModel message)
        {
            await Clients.All.SendAsync(HubMethods.ReceiveMethod, message);
        }

        public virtual Task SendMessageToCaller(SignalRMessageModel message)
        {
            return Clients.Caller.SendAsync(HubMethods.ReceiveMethod, message);
        }
        
        public virtual Task SendMessageToGroups(SignalRMessageModel message, string groupName, bool exceptCaller = true)
        {
            return exceptCaller
                ? Clients.GroupExcept(groupName, Context.ConnectionId).SendAsync(HubMethods.ReceiveMethod, message)
                : Clients.Group(groupName).SendAsync(HubMethods.ReceiveMethod, message);
        }

        #region Private
        private async Task AddUserToRoom(string roomName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
            if (!Rooms.ContainsKey(roomName))
            {
                Rooms.Add(roomName, new List<HubUserModel>());
            }

            var users = Rooms[roomName];
            if (!users.Any(u => u.ConnectionId == Context.ConnectionId))
            {
                users.Add(GetCurrentUser());
                Rooms[roomName] = users;
            }
        }


        private HubUserModel GetCurrentUser()
        {
            return new()
            {
                ConnectionId = Context.ConnectionId,
                Username = Context.User.Identity?.Name
            };
        }
        #endregion

        #region Override

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync().ConfigureAwait(false);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception).ConfigureAwait(false);
        }
        #endregion
    }
}