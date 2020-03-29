using Microsoft.AspNetCore.SignalR;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Messenger.Models;
using Mix.Domain.Core.ViewModels;
using Newtonsoft.Json.Linq;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mix.Cms.Service.SignalR.Hubs
{
    public class ServiceHub : BaseSignalRHub
    {
        #region Hub Methods

        // TODO Handle Join/Leave group
        public Task HandleRequest(string data)
        {
            var request = JObject.Parse(data).ToObject<HubRequest<JObject>>();
            switch (request.Action)
            {
                case Constants.HubMethods.SaveData:
                    return SaveData(request);

                case Constants.HubMethods.JoinGroup:
                    return JoinGroup(request);
                case Constants.HubMethods.SendMessage:
                    return SendToAll(request, Constants.Enums.MessageReponseKey.NewMessage, true);
                default:
                    SendToCaller(data, Constants.Enums.MessageReponseKey.Error);
                    return SendToCaller(Constants.HubMessages.UnknowErrorMsg, Constants.Enums.MessageReponseKey.Error);
            }
        }

        #endregion

        #region Handler

        
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
            connection.DeviceId = connection.DeviceId ?? Constants.DefaultDevice;
            // Mapping connecting user to db  models
            var user = new ViewModels.MixMessengerUsers.ConnectViewModel(connection)
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
                    var getAvailableUsers = ViewModels.MixMessengerUsers.DefaultViewModel.Repository.GetModelListBy(u => u.Status == (int)Constants.Enums.OnlineStatus.Connected);
                    var hubMsg = new RepositoryResponse<MessengerConnection>()
                    {
                        Status = 200,
                        Data = connection
                    };
                    return Task.WhenAll(new Task[]
                    {
                        Groups.AddToGroupAsync(Context.ConnectionId, request.Room),
                        SendToCaller(getAvailableUsers.Data, Constants.Enums.MessageReponseKey.ConnectSuccess),
                        SendToGroup(connection, Constants.Enums.MessageReponseKey.NewMember, request.Room),
                        // Announce every one there's new member
                        SendToAll(user, Constants.Enums.MessageReponseKey.NewMember, false)
                    });
                }
                else
                {
                    //  Send failed msg to caller
                    return SendToConnection(task.Result, Constants.Enums.MessageReponseKey.ConnectFailed, Context.ConnectionId, false);
                }
            });
        }

        private object GetGroupMembers(HubRequest<JObject> request)
        {
            Expression<Func<MixAttributeSetValue, bool>> predicate = m => m.Specificulture == request.Specificulture
                 && m.AttributeSetName == Constants.HubMessages.HubMemberName && m.AttributeFieldName == request.Room;
            var data = Lib.ViewModels.MixAttributeSetDatas.HubViewModel.FilterByValue(predicate);
            return data;
        }

        #region Send Methods

        private Task SendToConnection<T>(T message, Constants.Enums.MessageReponseKey action, string connectionId, bool isMySelf)
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
                    return Clients.Client(connectionId).SendAsync(Constants.HubMethods.ReceiveMethod, JObject.FromObject(result));
                }
                else
                {
                    return Clients.OthersInGroup(connectionId).SendAsync(Constants.HubMethods.ReceiveMethod, JObject.FromObject(result));
                }
            }
            else
            {
                return SendToCaller(Constants.HubMessages.UnknowErrorMsg, Constants.Enums.MessageReponseKey.Error);
            }
        }

        private Task SendToCaller<T>(T message, Constants.Enums.MessageReponseKey action)
        {
            HubResponse<T> result = new HubResponse<T>()
            {
                IsSucceed = true,
                Data = message,
                ResponseKey = GetResponseKey(action)
            };
            return Clients.Caller.SendAsync(Constants.HubMethods.ReceiveMethod, JObject.FromObject(result).ToString(Newtonsoft.Json.Formatting.None));
        }

        private Task SendToGroup<T>(T message, Constants.Enums.MessageReponseKey action, string groupName, bool isMySelf = false)
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
                    return Clients.Group(groupName).SendAsync(Constants.HubMethods.ReceiveMethod, JObject.FromObject(result).ToString(Newtonsoft.Json.Formatting.None));
                }
                else
                {
                    return Clients.OthersInGroup(groupName).SendAsync(Constants.HubMethods.ReceiveMethod, JObject.FromObject(result).ToString(Newtonsoft.Json.Formatting.None));
                }
            }
            else
            {
                return SendToCaller(Constants.HubMessages.UnknowErrorMsg, Constants.Enums.MessageReponseKey.Error);
            }
        }

        private Task SendToAll<T>(T message, Constants.Enums.MessageReponseKey action, bool isMySelf)
        {
            HubResponse<T> result = new HubResponse<T>()
            {
                IsSucceed = true,
                Data = message,
                ResponseKey = GetResponseKey(action)
            };

            if (isMySelf)
            {
                return Clients.All.SendAsync(Constants.HubMethods.ReceiveMethod, JObject.FromObject(result).ToString(Newtonsoft.Json.Formatting.None));
            }
            else
            {
                return Clients.Others.SendAsync(Constants.HubMethods.ReceiveMethod, JObject.FromObject(result).ToString(Newtonsoft.Json.Formatting.None));
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
            var getDevice = ViewModels.MixMessengerUserDevices.DefaultViewModel.Repository.GetSingleModel(m => m.ConnectionId == Context.ConnectionId);
            if (getDevice.IsSucceed)
            {
                getDevice.Data.Status = Constants.Enums.DeviceStatus.Disconnected;
                getDevice.Data.SaveModel(false);
            }

            return base.OnDisconnectedAsync(exception);
        }
        #endregion
    }
}