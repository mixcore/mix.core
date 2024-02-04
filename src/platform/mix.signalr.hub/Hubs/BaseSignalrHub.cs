using Google.Api;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.SignalR;
using Mix.Auth.Constants;
using Mix.Constant.Constants;
using Mix.Heart.Enums;
using Mix.Heart.Exceptions;
using Mix.Heart.Helpers;
using Mix.Lib.Interfaces;
using Mix.Signalr.Hub.Models;
using Mix.SignalR.Constants;
using Mix.SignalR.Enums;
using Mix.SignalR.Models;
using System.Security.Claims;

namespace Mix.SignalR.Hubs
{
    public abstract class BaseSignalRHub : Hub
    {
        protected IAuditLogService AuditLogService;
        protected IMixTenantService MixTenantService;
        protected HubUserModel? CurrentUser;
        protected int? TenantId;

        protected BaseSignalRHub(IAuditLogService auditLogService, IMixTenantService mixTenantService)
        {
            AuditLogService = auditLogService;
            MixTenantService = mixTenantService;
        }

        public static Dictionary<string, List<HubUserModel>> Rooms = new Dictionary<string, List<HubUserModel>>();
        public virtual async Task Join(string host)
        {
            if (MixTenantService.AllTenants == null)
            {
                await MixTenantService.Reload();
            }
            var currentTenant = MixTenantService.GetTenant(host);
            if (currentTenant != null)
            {
                TenantId = currentTenant.Id;
            }

            var user = GetCurrentUser();
            if (user != null)
            {
                CurrentUser = user;
                await SendMessageToCaller(new(user) { Action = MessageAction.MyConnection });
            }
        }

        public virtual async Task JoinRoom(string roomName)
        {
            await AddUserToRoom(roomName);
        }

        public virtual async Task SendMessage(SignalRMessageModel message)
        {
            message.From ??= GetCurrentUser();
            await Clients.All.SendAsync(HubMethods.ReceiveMethod, message.ToString());
        }

        private string? GetIPAddress()
        {
            var feature = Context.Features.Get<IHttpConnectionFeature>();
            // here you could get your client remote address
            return feature?.RemoteIpAddress?.ToString();
        }

        public virtual async Task SendPrivateMessage(SignalRMessageModel message, string connectionId, bool selfReceive)
        {
            try
            {
                message.From ??= GetCurrentUser();
                await Clients.Client(connectionId).SendAsync(HubMethods.ReceiveMethod, message);
                if (selfReceive)
                {
                    await SendMessageToCaller(message);
                }
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.ServerError, ex);
            }

        }

        public virtual Task SendMessageToCaller(SignalRMessageModel message)
        {
            try
            {
                message.From ??= GetCurrentUser();
                return Clients.Caller.SendAsync(HubMethods.ReceiveMethod, message.ToString());
            }
            catch (Exception ex)
            {
                throw new MixException(Heart.Enums.MixErrorStatus.ServerError, ex);
            }
        }

        public virtual Task SendGroupMessage(SignalRMessageModel message, string groupName, bool exceptCaller = true)
        {
            try
            {
                message.From ??= GetCurrentUser();
                return exceptCaller
                    ? Clients.GroupExcept(groupName, Context.ConnectionId).SendAsync(HubMethods.ReceiveMethod, message.ToString())
                    : Clients.Group(groupName).SendAsync(HubMethods.ReceiveMethod, message.ToString());
            }
            catch (Exception ex)
            {
                throw new MixException(Heart.Enums.MixErrorStatus.ServerError, ex);
            }

        }

        #region Private
        protected async Task AddUserToRoom(string roomName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
            if (!Rooms.ContainsKey(roomName))
            {
                Rooms.Add(roomName, new List<HubUserModel>());
            }

            var users = Rooms[roomName] ?? new();
            if (!users.Any(u => u != null && u.ConnectionId == Context.ConnectionId))
            {
                var user = GetCurrentUser();
                if (user != null)
                {
                    users.Add(user);
                    Rooms[roomName] = users;
                }
                await SendMessageToCaller(new(user) { Action = MessageAction.MyConnection });
                await SendMessageToCaller(new(users) { Action = MessageAction.MemberList });
                await SendGroupMessage(new(user) { Action = MessageAction.NewMember }, roomName);
            }
        }


        protected HubUserModel GetCurrentUser()
        {
            return new()
            {
                TenantId = TenantId,
                ConnectionId = Context.ConnectionId,
                Username = Context.User?.Identity?.Name ?? "Annonymous",
                Avatar = Context.User?.Claims.FirstOrDefault(m => m.Type == MixClaims.Avatar)?.Value ?? MixConstants.CONST_DEFAULT_EXTENSIONS_FILE_PATH,
            };
        }
        #endregion

        #region Override

        public override async Task OnConnectedAsync()
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                CurrentUser = user;
                await SendMessageToCaller(new(user) { Action = MessageAction.MyConnection });
            }
            await base.OnConnectedAsync().ConfigureAwait(false);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            foreach (var room in Rooms)
            {
                var user = room.Value.FirstOrDefault(m => m.ConnectionId == Context.ConnectionId);
                if (user != null)
                {
                    room.Value.Remove(user);
                    await SendGroupMessage(new(user) { Action = MessageAction.MemberOffline }, room.Key);
                }
            }
            await base.OnDisconnectedAsync(exception).ConfigureAwait(false);
        }
        #endregion
    }
}