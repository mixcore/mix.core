using Microsoft.AspNetCore.SignalR;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Service.SignalR.Models;
using Mix.Domain.Core.ViewModels;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
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
                    return SendToAll(request, Constants.Enums.MessageReponseKey.NewMessage, request.IsMySelf);
                case Constants.HubMethods.SendGroupMessage:
                    if (request.IsSave)
                    {
                        _ = SaveData(request);
                    }
                    return SendToGroup(request, Constants.Enums.MessageReponseKey.NewMessage, request.Room, request.IsMySelf);
                default:
                    return SendToCaller(Constants.HubMessages.UnknowErrorMsg, Constants.Enums.MessageReponseKey.Error);
            }
        }

        #endregion

        #region Handler

        
        private async Task SaveData(HubRequest<JObject> request)
        {
            var data = new Lib.ViewModels.MixAttributeSetDatas.FormViewModel()
            {
                Specificulture = request.Specificulture,
                AttributeSetName = request.Room,
                Data = request.Data,
                CreatedBy = request.Uid
            };
            var saveData = await data.SaveModelAsync(true);
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
                    var getPreviousMsgs = Mix.Cms.Service.SignalR.ViewModels.MixAttributeSetDatas.ReadViewModel.Repository.GetModelListBy(
                        m=> m.AttributeSetName == request.Room && m.Specificulture== request.Specificulture
                        , "CreatedDateTime", 1, 10, 0);
                    getPreviousMsgs.Data.Items = getPreviousMsgs.Data.Items.OrderBy(m => m.CreatedDateTime).ToList();
                    var hubMsg = new RepositoryResponse<MessengerConnection>()
                    {
                        Status = 200,
                        Data = connection
                    };
                    return Task.WhenAll(new Task[]
                    {
                        // Add User to group connections
                        Groups.AddToGroupAsync(Context.ConnectionId, request.Room),
                        
                        // Announce User Connected to Group and list available users
                        SendToCaller(getAvailableUsers.Data, Constants.Enums.MessageReponseKey.ConnectSuccess),
                        // Load previous messages
                        SendToCaller(getPreviousMsgs.Data, Constants.Enums.MessageReponseKey.PreviousMessages),
                        

                        // Announce Other Group member there is new user
                        SendToGroup(connection, Constants.Enums.MessageReponseKey.NewMember, request.Room),
                        
                        // Announce every one there's new member
                        //SendToAll(user, Constants.Enums.MessageReponseKey.NewMember, false)
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

        private async Task SendToCaller<T>(T message, Constants.Enums.MessageReponseKey action)
        {
            HubResponse<T> result = new HubResponse<T>()
            {
                IsSucceed = true,
                Data = message,
                ResponseKey = GetResponseKey(action)
            };
            await Clients.Caller.SendAsync(Constants.HubMethods.ReceiveMethod, JObject.FromObject(result).ToString(Newtonsoft.Json.Formatting.None));
        }

        private async Task SendToGroup<T>(T message, Constants.Enums.MessageReponseKey action, string groupName, bool isMySelf = false)
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
                    await Clients.Group(groupName).SendAsync(Constants.HubMethods.ReceiveMethod, JObject.FromObject(result).ToString(Newtonsoft.Json.Formatting.None));
                }
                else
                {
                    await Clients.OthersInGroup(groupName).SendAsync(Constants.HubMethods.ReceiveMethod, JObject.FromObject(result).ToString(Newtonsoft.Json.Formatting.None));
                }
            }
            else
            {
                await SendToCaller(Constants.HubMessages.UnknowErrorMsg, Constants.Enums.MessageReponseKey.Error);
            }
        }

        private async Task SendToAll<T>(T message, Constants.Enums.MessageReponseKey action, bool isMySelf)
        {
            HubResponse<T> result = new HubResponse<T>()
            {
                IsSucceed = true,
                Data = message,
                ResponseKey = GetResponseKey(action)
            };

            if (isMySelf)
            {
                await Clients.All.SendAsync(Constants.HubMethods.ReceiveMethod, JObject.FromObject(result).ToString(Newtonsoft.Json.Formatting.None));
            }
            else
            {
                await Clients.Others.SendAsync(Constants.HubMethods.ReceiveMethod, JObject.FromObject(result).ToString(Newtonsoft.Json.Formatting.None));
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
                var getUser= ViewModels.MixMessengerUsers.DefaultViewModel.Repository.GetSingleModel(m => m.Id == getDevice.Data.UserId);
                if (getUser.IsSucceed)
                {
                    if (ViewModels.MixMessengerUserDevices.DefaultViewModel.Repository.Count(m=>m.UserId == getUser.Data.Id && m.Status == (int)Constants.Enums.DeviceStatus.Actived).Data == 0)
                    {
                        getUser.Data.Status = Constants.Enums.OnlineStatus.Disconnected;
                        getUser.Data.SaveModel(false);
                    }

                }
            }

            return base.OnDisconnectedAsync(exception);
        }
        #endregion
    }
}