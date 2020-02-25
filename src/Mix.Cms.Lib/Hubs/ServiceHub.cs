using Microsoft.AspNetCore.SignalR;
using Mix.Cms.Lib;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Messenger.Models;
using Mix.Domain.Core.ViewModels;
using Mix.UI.Core.SignalR;
using Newtonsoft.Json.Linq;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static Mix.Cms.Messenger.MixChatEnums;

namespace Mix.Cms.Hub
{
    public class ServiceHub : BaseSignalRHub
    {
        // TODO Handle Join/Leave group
        public Task HandleRequest(HubRequest<JObject> request)
        {
            switch (request.Action)
            {
                case MixConstants.ServiceHub.SaveData:
                    return SaveData(request);

                case MixConstants.ServiceHub.JoinGroup:
                    return JoinGroup(request);

                default:
                    return SendToCaller(MixConstants.ServiceHub.UnknowErrorMsg, MessageReponseKey.Error);
            }
        }

        private Task SaveData(HubRequest<JObject> request)
        {
            var data = new Lib.ViewModels.MixAttributeSetDatas.HubViewModel()
            {
                AttributeSetName = request.AttributeSetName,
                Data = request.Data
            };
            throw new NotImplementedException();
        }

        private Task JoinGroup(HubRequest<JObject> request)
        {
            var groupMembers = GetGroupMembers(request);
            var connection = request.Data.ToObject<MessengerConnection>();
            // Set connection Id
            connection.ConnectionId = Context.ConnectionId;
            connection.DeviceId = connection.DeviceId ?? MixConstants.ServiceHub.DefaultDevice;
            // Mapping connecting user to db  models
            var user = new Messenger.ViewModels.MixMessengerUsers.ConnectViewModel(connection)
            {
                CreatedDate = DateTime.UtcNow
            };
            // Save user and current device to db
            return user.Join().ContinueWith((task) =>
            {
                //  save success
                if (task.Result.IsSucceed)
                {
                    // Get Online users
                    var getAvailableUsers = Messenger.ViewModels.MixMessengerUsers.DefaultViewModel.Repository.GetModelListBy(u => u.Status == (int)Messenger.MixChatEnums.OnlineStatus.Connected);
                    var hubMsg = new RepositoryResponse<MessengerConnection>()
                    {
                        Status = 200,
                        Data = connection
                    };
                    return Task.WhenAll(new Task[]
                    {
                        Groups.AddToGroupAsync(Context.ConnectionId, request.Room),
                        SendToCaller(getAvailableUsers.Data, MessageReponseKey.ConnectSuccess),
                        SendToGroup(connection, MessageReponseKey.NewMember, request.Room),
                        // Announce every one there's new member
                        SendToAll(user, MessageReponseKey.NewMember, false)
                    });
                }
                else
                {
                    //  Send failed msg to caller
                    return SendToConnection(task.Result, MessageReponseKey.ConnectFailed, Context.ConnectionId, false);
                }
            });
        }

        private object GetGroupMembers(HubRequest<JObject> request)
        {
            Expression<Func<MixAttributeSetValue, bool>> predicate = m => m.Specificulture == request.Specificulture
                 && m.AttributeSetName == MixConstants.ServiceHub.HubMemberName && m.AttributeFieldName == request.Room;
            var data = Lib.ViewModels.MixAttributeSetDatas.HubViewModel.FilterByValue(predicate);
            return data;
        }

        #region Send Methods

        public void SendMessage(JObject request)
        {
            var data = request.ToObject<HubRequest<HubMessage>>();
            data.Data.CreatedDate = DateTime.UtcNow;
            SendToAll(data.Data, MessageReponseKey.NewMessage, true);
        }

        private Task SendToConnection<T>(T message, MessageReponseKey action, string connectionId, bool isMySelf)
        {
            if (!string.IsNullOrEmpty(connectionId))
            {
                HubResponse<T> result = new HubResponse<T>()
                {
                    IsSucceed = true,
                    Data = message,
                    ResponseKey = GetResponseKey(action)
                };

                if (isMySelf)
                {
                    return Clients.Client(connectionId).SendAsync(MixConstants.ServiceHub.ReceiveMethod, JObject.FromObject(result));
                }
                else
                {
                    return Clients.OthersInGroup(connectionId).SendAsync(MixConstants.ServiceHub.ReceiveMethod, JObject.FromObject(result));
                }
            }
            else
            {
                return SendToCaller(MixConstants.ServiceHub.UnknowErrorMsg, MessageReponseKey.Error);
            }
        }

        private Task SendToCaller<T>(T message, MessageReponseKey action)
        {
            HubResponse<T> result = new HubResponse<T>()
            {
                IsSucceed = true,
                Data = message,
                ResponseKey = GetResponseKey(action)
            };
            return Clients.Caller.SendAsync(MixConstants.ServiceHub.ReceiveMethod, JObject.FromObject(result));
        }

        private Task SendToGroup<T>(T message, MessageReponseKey action, string groupName, bool isMySelf = false)
        {
            if (!string.IsNullOrEmpty(groupName))
            {
                HubResponse<T> result = new HubResponse<T>()
                {
                    IsSucceed = true,
                    Data = message,
                    ResponseKey = GetResponseKey(action)
                };

                if (isMySelf)
                {
                    return Clients.Group(groupName).SendAsync(MixConstants.ServiceHub.ReceiveMethod, JObject.FromObject(result));
                }
                else
                {
                    return Clients.OthersInGroup(groupName).SendAsync(MixConstants.ServiceHub.ReceiveMethod, JObject.FromObject(result));
                }
            }
            else
            {
                return SendToCaller(MixConstants.ServiceHub.UnknowErrorMsg, MessageReponseKey.Error);
            }
        }

        private Task SendToAll<T>(T message, MessageReponseKey action, bool isMySelf)
        {
            HubResponse<T> result = new HubResponse<T>()
            {
                IsSucceed = true,
                Data = message,
                ResponseKey = GetResponseKey(action)
            };

            if (isMySelf)
            {
                return Clients.All.SendAsync(MixConstants.ServiceHub.ReceiveMethod, result);
            }
            else
            {
                return Clients.Others.SendAsync(MixConstants.ServiceHub.ReceiveMethod, result);
            }
        }

        private string GetResponseKey<T>(T e)
        {
            return Enum.GetName(typeof(T), e);
        }

        #endregion Send Methods

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}