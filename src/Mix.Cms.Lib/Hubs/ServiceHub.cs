using Microsoft.AspNetCore.SignalR;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Core.ViewModels;
using Mix.UI.Core.SignalR;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mix.Cms.Hub
{
    public class ServiceHub : BaseSignalRHub
    {
        private const string receiveMethod = "ReceiveMessage";
        private const string hubMemberName = "hub_member";
        private const string hubMemberFieldName = "hub_name";
        // TODO Handle Join/Leave group
        public Task JoinGroup(string groupName, string username)
        {
            string msg = $"new user {username}";
            AddUserToGroup(groupName, username);
            return Clients.Group(groupName).SendAsync(receiveMethod, msg );
        }

        private Task AddUserToGroup(string groupName, string username)
        {
            var groupMembers = GetGroupMembers(groupName);
            return Task.WhenAll(new Task[]
            {
                SendMessageToCaller(groupMembers)
            });
        }

        private object GetGroupMembers(string groupName)
        {
            Expression<Func<MixAttributeSetValue, bool>> predicate= m => 
                m.AttributeSetName == hubMemberName && m.AttributeFieldName == hubMemberFieldName;
            throw new NotImplementedException();
        }
        #region Send Methods
        public async Task SendMessage(string user, dynamic message)
        {
            RepositoryResponse<dynamic> msg = new RepositoryResponse<dynamic>()
            {
                IsSucceed = true,
                Data = message
            };
            await Clients.All.SendAsync(receiveMethod, user, msg);
        }

        public Task SendMessageToCaller(dynamic message)
        {
            RepositoryResponse<dynamic> msg = new RepositoryResponse<dynamic>()
            {
                IsSucceed = true,
                Data = message
            };
            return Clients.Caller.SendAsync(receiveMethod, msg);
        }


        public Task SendMessageToGroup(dynamic message, string groupName)
        {
            RepositoryResponse<dynamic> msg = new RepositoryResponse<dynamic>()
            {
                IsSucceed = true,
                Data = message
            };
            return Clients.Group(groupName).SendAsync(receiveMethod, msg);
        }

        #endregion


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
