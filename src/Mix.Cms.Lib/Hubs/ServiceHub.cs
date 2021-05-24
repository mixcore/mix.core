using Microsoft.AspNetCore.SignalR;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.SignalR;
using Mix.Cms.Lib.SignalR.Constants;
using Mix.Cms.Lib.SignalR.Enums;
using Mix.Cms.Lib.SignalR.Models;
using Mix.Cms.Messenger.Models.Data;
using Mix.Heart.Enums;
using Mix.Heart.Infrastructure.SignalR;
using Mix.Heart.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.Hubs
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
                case HubMethods.SaveData:
                    return SaveData(request);

                case HubMethods.JoinGroup:
                    return JoinGroup(request);

                case HubMethods.SendMessage:
                    return SendToAll(request, MessageReponseKey.NewMessage, request.IsMySelf);

                case HubMethods.SendGroupMessage:
                    if (request.IsSave)
                    {
                        _ = SaveData(request);
                    }
                    return SendToGroup(request, MessageReponseKey.NewMessage, request.Room, request.IsMySelf);

                default:
                    return SendToCaller(HubMessages.UnknowErrorMsg, MessageReponseKey.Error);
            }
        }

        #endregion Hub Methods

        #region Handler

        private async Task SaveData(HubRequest<JObject> request)
        {
            var data = new Lib.ViewModels.MixDatabaseDatas.FormViewModel()
            {
                Specificulture = request.Specificulture,
                MixDatabaseName = request.Room,
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
            connection.DeviceId = connection.DeviceId ?? SignalRConstants.DefaultDevice;

            // Add User to group connections
            await Groups.AddToGroupAsync(Context.ConnectionId, request.Room);
            // Announce Other Group member there is new user
            await SendToGroup(connection, MessageReponseKey.NewMember, request.Room);

            // Get Online users
            var getAvailableUsers = Service.ViewModels.SignalR.MixMessengerUsers.DefaultViewModel.Repository.GetModelListBy(
                u => u.Status == OnlineStatus.Connected.ToString());
            if (getAvailableUsers.IsSucceed)
            {
                // Announce User Connected to Group and list available users
                await SendToCaller(getAvailableUsers.Data, MessageReponseKey.ConnectSuccess);
            }
            var getPreviousMsgs = ViewModels.MixDatabaseDatas.ReadViewModel.Repository.GetModelListBy(
                        m => m.MixDatabaseName == request.Room && m.Specificulture == request.Specificulture
                        , "CreatedDateTime", DisplayDirection.Desc, 10, 0);
            // Get previous messages
            if (getPreviousMsgs.IsSucceed)
            {
                getPreviousMsgs.Data.Items = getPreviousMsgs.Data.Items.OrderBy(m => m.CreatedDateTime).ToList();
                await SendToCaller(getPreviousMsgs.Data, MessageReponseKey.PreviousMessages);
            }
            var groupMembers = GetGroupMembersAsync(request);

            var result = new RepositoryResponse<bool>();
            // Mapping connecting user to db  models
            var user = new Service.ViewModels.SignalR.MixMessengerUsers.ConnectViewModel(connection)
            {
                CreatedDate = DateTime.UtcNow
            };
            result = user.Join();
        }

        private async Task<object> GetGroupMembersAsync(HubRequest<JObject> request)
        {
            Expression<Func<MixDatabaseDataValue, bool>> predicate = m => m.Specificulture == request.Specificulture
                 && m.MixDatabaseName == HubMessages.HubMemberName && m.MixDatabaseColumnName == request.Room;
            var data = await Lib.ViewModels.MixDatabaseDatas.Helper.FilterByKeywordAsync<Lib.ViewModels.MixDatabaseDatas.FormViewModel>(
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
                    return Clients.Client(connectionId).SendAsync(HubMethods.ReceiveMethod, JObject.FromObject(result));
                }
                else
                {
                    return Clients.OthersInGroup(connectionId).SendAsync(HubMethods.ReceiveMethod, JObject.FromObject(result));
                }
            }
            else
            {
                return SendToCaller(HubMessages.UnknowErrorMsg, MessageReponseKey.Error);
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
            await Clients.Caller.SendAsync(HubMethods.ReceiveMethod, JObject.FromObject(result).ToString(Newtonsoft.Json.Formatting.None));
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
                    await Clients.Group(groupName).SendAsync(HubMethods.ReceiveMethod, JObject.FromObject(result).ToString(Newtonsoft.Json.Formatting.None));
                }
                else
                {
                    await Clients.OthersInGroup(groupName).SendAsync(HubMethods.ReceiveMethod, JObject.FromObject(result).ToString(Newtonsoft.Json.Formatting.None));
                }
            }
            else
            {
                await SendToCaller(HubMessages.UnknowErrorMsg, MessageReponseKey.Error);
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
                await Clients.All.SendAsync(HubMethods.ReceiveMethod, JObject.FromObject(result).ToString(Newtonsoft.Json.Formatting.None));
            }
            else
            {
                await Clients.Others.SendAsync(HubMethods.ReceiveMethod, JObject.FromObject(result).ToString(Newtonsoft.Json.Formatting.None));
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
            var getDevice = Service.ViewModels.SignalR.MixMessengerUserDevices.DefaultViewModel.Repository.GetSingleModel(m => m.ConnectionId == Context.ConnectionId);
            if (getDevice.IsSucceed)
            {
                getDevice.Data.Status = DeviceStatus.Disconnected;
                getDevice.Data.SaveModel(false);
                var getUser = Service.ViewModels.SignalR.MixMessengerUsers.DefaultViewModel.Repository.GetSingleModel(m => m.Id == getDevice.Data.UserId);
                if (getUser.IsSucceed)
                {
                    if (Service.ViewModels.SignalR.MixMessengerUserDevices.DefaultViewModel.Repository.Count(m => m.UserId == getUser.Data.Id && m.Status == (int)DeviceStatus.Actived).Data == 0)
                    {
                        getUser.Data.Status = OnlineStatus.Disconnected;
                        getUser.Data.SaveModel(false);
                    }
                }
            }

            return base.OnDisconnectedAsync(exception);
        }

        #endregion Handler
    }
}