﻿using Microsoft.AspNetCore.SignalR;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Messenger.Models.Data;
using Mix.Cms.Service.SignalR.Models;
using Mix.Domain.Core.ViewModels;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Mix.Cms.Service.SignalR.Domain.Enums;
using Mix.Cms.Service.SignalR.Domain.Constants;

namespace Mix.Cms.Service.SignalR.Hubs
{
    public class ServiceHub : BaseSignalRHub
    {
        private readonly MixChatServiceContext _context;
        private readonly MixCmsContext _msgContext;
        public ServiceHub(MixChatServiceContext context, MixCmsContext msgContext) : base()
        {
            _context = context;
            _msgContext = msgContext;
        }
        #region Hub Methods

        // TODO Handle Join/Leave group
        public Task HandleRequest(string data)
        {
            var request = JObject.Parse(data).ToObject<HubRequest<JObject>>();
            switch (request.Action)
            {
                case MixHubMethods.SaveData:
                    return SaveData(request);
                case MixHubMethods.JoinGroup:
                    return JoinGroup(request);
                case MixHubMethods.SendMessage:
                    return SendToAll(request, MessageReponseKey.NewMessage, request.IsMySelf);
                case MixHubMethods.SendGroupMessage:
                    if (request.IsSave)
                    {
                        _ = SaveData(request);
                    }
                    return SendToGroup(request, MessageReponseKey.NewMessage, request.Room, request.IsMySelf);
                default:
                    return SendToCaller(MixHubMessages.UnknowErrorMsg, MessageReponseKey.Error);
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
                Obj = request.Data,
                CreatedBy = request.Uid
            };
            var saveData = await data.SaveModelAsync(true);
        }

        private async Task JoinGroup(HubRequest<JObject> request)
        {
            var connection = request.Data.ToObject<MessengerConnection>();
            // Set connection Id
            connection.ConnectionId = Context.ConnectionId;
            connection.DeviceId = connection.DeviceId ?? MixHubConstants.DefaultDevice;

            // Add User to group connections
            await Groups.AddToGroupAsync(Context.ConnectionId, request.Room);
            // Announce Other Group member there is new user
            await SendToGroup(connection, MessageReponseKey.NewMember, request.Room);

            // Get Online users
            var getAvailableUsers = ViewModels.MixMessengerUsers.DefaultViewModel.Repository.GetModelListBy(
                u => u.Status == MixOnlineStatus.Connected.ToString());
            if (getAvailableUsers.IsSucceed)
            {
                // Announce User Connected to Group and list available users
                await SendToCaller(getAvailableUsers.Data, MessageReponseKey.ConnectSuccess);
            }
            var getPreviousMsgs = Mix.Cms.Service.SignalR.ViewModels.MixAttributeSetDatas.ReadViewModel.Repository.GetModelListBy(
                        m => m.AttributeSetName == request.Room && m.Specificulture == request.Specificulture
                        , "CreatedDateTime", Heart.Enums.MixHeartEnums.DisplayDirection.Desc, 10, 0);
            // Get previous messages
            if (getPreviousMsgs.IsSucceed)
            {
                getPreviousMsgs.Data.Items = getPreviousMsgs.Data.Items.OrderBy(m => m.CreatedDateTime).ToList();
                await SendToCaller(getPreviousMsgs.Data, MessageReponseKey.PreviousMessages);
            }
            var groupMembers = GetGroupMembersAsync(request);


            var result = new RepositoryResponse<bool>();
            // Mapping connecting user to db  models
            var user = new ViewModels.MixMessengerUsers.ConnectViewModel(connection)
            {
                CreatedDate = DateTime.UtcNow
            };
            result = user.Join();

        }

        private async Task<object> GetGroupMembersAsync(HubRequest<JObject> request)
        {
            Expression<Func<MixAttributeSetValue, bool>> predicate = m => m.Specificulture == request.Specificulture
                 && m.AttributeSetName == MixHubMessages.HubMemberName && m.AttributeFieldName == request.Room;
            var data = await Lib.ViewModels.MixAttributeSetDatas.Helper.FilterByKeywordAsync<Lib.ViewModels.MixAttributeSetDatas.FormViewModel>(
                request.Specificulture, request.Room, null, null);
            return data;
        }

        #region Send Methods

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
                    return Clients.Client(connectionId).SendAsync(MixHubMethods.ReceiveMethod, JObject.FromObject(result));
                }
                else
                {
                    return Clients.OthersInGroup(connectionId).SendAsync(MixHubMethods.ReceiveMethod, JObject.FromObject(result));
                }
            }
            else
            {
                return SendToCaller(MixHubMessages.UnknowErrorMsg, MessageReponseKey.Error);
            }
        }

        private async Task SendToCaller<T>(T message, MessageReponseKey action)
        {
            HubResponse<T> result = new HubResponse<T>()
            {
                IsSucceed = true,
                Data = message,
                ResponseKey = GetResponseKey(action)
            };
            await Clients.Caller.SendAsync(MixHubMethods.ReceiveMethod, JObject.FromObject(result).ToString(Newtonsoft.Json.Formatting.None));
        }

        private async Task SendToGroup<T>(T message, MessageReponseKey action, string groupName, bool isMySelf = false)
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
                    await Clients.Group(groupName).SendAsync(MixHubMethods.ReceiveMethod, JObject.FromObject(result).ToString(Newtonsoft.Json.Formatting.None));
                }
                else
                {
                    await Clients.OthersInGroup(groupName).SendAsync(MixHubMethods.ReceiveMethod, JObject.FromObject(result).ToString(Newtonsoft.Json.Formatting.None));
                }
            }
            else
            {
                await SendToCaller(MixHubMessages.UnknowErrorMsg, MessageReponseKey.Error);
            }
        }

        private async Task SendToAll<T>(T message, MessageReponseKey action, bool isMySelf)
        {
            HubResponse<T> result = new HubResponse<T>()
            {
                IsSucceed = true,
                Data = message,
                ResponseKey = GetResponseKey(action)
            };

            if (isMySelf)
            {
                await Clients.All.SendAsync(MixHubMethods.ReceiveMethod, JObject.FromObject(result).ToString(Newtonsoft.Json.Formatting.None));
            }
            else
            {
                await Clients.Others.SendAsync(MixHubMethods.ReceiveMethod, JObject.FromObject(result).ToString(Newtonsoft.Json.Formatting.None));
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
            var getDevice = ViewModels.MixMessengerUserDevices.DefaultViewModel.Repository.GetSingleModel(
                    m => m.ConnectionId == Context.ConnectionId);
            if (getDevice.IsSucceed)
            {
                getDevice.Data.Status = MixDeviceStatus.Disconnected;
                getDevice.Data.SaveModel(false);
                var getUser = ViewModels.MixMessengerUsers.DefaultViewModel.Repository.GetSingleModel(m => m.Id == getDevice.Data.UserId);
                if (getUser.IsSucceed)
                {
                    if (ViewModels.MixMessengerUserDevices.DefaultViewModel.Repository.Count(
                            m => m.UserId == getUser.Data.Id && m.Status == (int)MixDeviceStatus.Actived).Data == 0)
                    {
                        getUser.Data.Status = MixOnlineStatus.Disconnected;
                        getUser.Data.SaveModel(false);
                    }

                }
            }

            return base.OnDisconnectedAsync(exception);
        }
        #endregion
    }
}