using Microsoft.AspNetCore.SignalR;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.SignalR.Constants;
using Mix.Cms.Lib.SignalR.Enums;
using Mix.Cms.Lib.SignalR.Models;
using Mix.Heart.Infrastructure.SignalR;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.Hubs
{
    public class MixChatHub : BaseSignalRHub
    {
        private const string defaultRoom = "public";
        private const string defaultDevice = "website";

        public void Join(HubRequest<MessengerConnection> request)
        {
            // Set connection Id
            request.Data.ConnectionId = Context.ConnectionId;
            request.Data.DeviceId = request.Data.DeviceId ?? defaultDevice;
            // Mapping connecting user to db  models
            var user = new Service.ViewModels.SignalR.MixMessengerUsers.ConnectViewModel(request.Data);

            user.CreatedDate = DateTime.UtcNow;
            // Save user and current device to db
            var result = user.Join();

            //  save success
            if (result.IsSucceed)
            {
                //  Send success msg to caller
                var getAvailableUsers = Service.ViewModels.SignalR.MixMessengerUsers.DefaultViewModel.Repository.GetModelListBy(u => u.Status == MixContentStatus.Published.ToString());
                SendToCaller(getAvailableUsers.Data, MessageReponseKey.ConnectSuccess);
                // Announce every one there's new member
                SendToAll(user, MessageReponseKey.NewMember, false);
            }
            else
            {
                //  Send failed msg to caller
                SendToConnection(result, MessageReponseKey.ConnectFailed, Context.ConnectionId, false);
            }
        }

        public void SendMessage(JObject request)
        {
            var data = request.ToObject<HubRequest<HubMessage>>();
            data.Data.CreatedDate = DateTime.UtcNow;
            SendToAll(data.Data, MessageReponseKey.NewMessage, true);
        }

        public Task SendMessageToCaller(string message)
        {
            return Clients.Caller.SendAsync(HubMethods.ReceiveMethod, message);
        }

        public Task SendMessageToGroups(string message)
        {
            List<string> groups = new List<string>() { "SignalR Users" };
            return Clients.Groups(groups).SendAsync(HubMethods.ReceiveMethod, message);
        }

        private void SendToConnection<T>(T message, MessageReponseKey action, string connectionId, bool isMySelf)
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
                    Clients.Client(connectionId).SendAsync(HubMethods.ReceiveMethod, JObject.FromObject(result));
                }
                else
                {
                    Clients.OthersInGroup(connectionId).SendAsync(HubMethods.ReceiveMethod, JObject.FromObject(result));
                }
            }
        }

        private void SendToCaller<T>(T message, MessageReponseKey action)
        {
            HubResponse<T> result = new HubResponse<T>()
            {
                IsSucceed = true,
                Data = message,
                ResponseKey = GetResponseKey(action)
            };
            Clients.Caller.SendAsync(HubMethods.ReceiveMethod, JObject.FromObject(result));
        }

        private void SendToGroup<T>(T message, MessageReponseKey action, string groupName, bool isMySelf)
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
                    Clients.Group(groupName).SendAsync(HubMethods.ReceiveMethod, JObject.FromObject(result));
                }
                else
                {
                    Clients.OthersInGroup(groupName).SendAsync(HubMethods.ReceiveMethod, JObject.FromObject(result));
                }
            }
        }

        private void SendToAll<T>(T message, MessageReponseKey action, bool isMySelf)
        {
            HubResponse<T> result = new HubResponse<T>()
            {
                IsSucceed = true,
                Data = message,
                ResponseKey = GetResponseKey(action)
            };

            if (isMySelf)
            {
                Clients.All.SendAsync(HubMethods.ReceiveMethod, result);
            }
            else
            {
                Clients.Others.SendAsync(HubMethods.ReceiveMethod, result);
            }
        }

        private string GetResponseKey<T>(T e)
        {
            return Enum.GetName(typeof(T), e);
        }

        #region Team - TODO Write these functions

        /*
        [HubMethodName("getTeam")]
        public async System.Threading.Tasks.Task GetTeam(JObject request)
        {
            string errorMsg = string.Empty;
            int status = 0;
            MessengerHubResponse<ChatTeamViewModel> result = null;
            string action = Enum.GetName(typeof(TeamMessageReponseKey), TeamMessageReponseKey.GetTeam);
            //PaginationModel<ChatTeamViewModel> currentMessages = new PaginationModel<ChatTeamViewModel>();
            try
            {
                if (TTXTeamMemberRepository<FETeamMemberViewModel>.Instance.CheckIsExists(m => m.TeamId == request.TeamId && m.MemberId == request.UserId && m.Status == (int)MemberStatus.Membered))
                {
                    ChatTeamViewModel team = await TTXTeamRepository<ChatTeamViewModel>.Instance.GetSingleModelAsync(t => t.Id == request.TeamId);

                    team.IsNewMessage = TTXTeamMessageRepository<TeamMessage>.Instance.CountUnseenMessage(team.Id, request.UserId) > 0;

                    if (team != null)
                    {
                        team.IsAdmin = request.UserId == team.HostId;
                        if (team.IsAdmin)
                        {
                            team.TotalRequest = TTXTeamMemberRepository<FETeamMemberViewModel>.Instance.CountRequests(request.TeamId);
                        }

                        result = new MessengerHubResponse<ChatTeamViewModel>()
                        {
                            status = 1,
                            responseKey = string.Format("{0}", action),
                            data = team,
                            errors = null
                        };
                    }
                    else
                    {
                        result = new MessengerHubResponse<ChatTeamViewModel>()
                        {
                            status = 0,
                            responseKey = TeamMessageReponseKey.NotMembered.ToString(),
                            data = null,
                            errors = null
                        };
                    }
                }
                else
                {
                    result = new MessengerHubResponse<ChatTeamViewModel>()
                    {
                        status = 0,
                        responseKey = GameResponseKey.NotAuthorized.ToString(),
                        data = null,
                        errors = new List<string>() { "You are not membered of this team" }
                    };
                    Clients.Caller.receiveMessage(result);
                }
            }
            catch (Exception ex)
            {
                result = new MessengerHubResponse<ChatTeamViewModel>()
                {
                    status = 0,
                    responseKey = string.Format("{0}", action),
                    data = null,
                    error = ex.StackTrace
                };
            }
            finally
            {
                Clients.Caller.receiveMessage(result);
                UpdatePlayerConnectionIdAsync(request.UserId);
            }
        }

        [HubMethodName("getTeamMessages")]
        public async System.Threading.Tasks.Task GetTeamMessages(ApiGetTeamMemberViewMdoel request)
        {
            string errorMsg = string.Empty;
            int status = 0;
            MessengerHubResponse<ChatTeamViewModel> result = null;
            string action = Enum.GetName(typeof(TeamMessageReponseKey), TeamMessageReponseKey.GetTeamMessages);
            //PaginationModel<ChatTeamViewModel> currentMessages = new PaginationModel<ChatTeamViewModel>();
            try
            {
                if (TTXTeamMemberRepository<FETeamMemberViewModel>.Instance.CheckIsExists(m => m.TeamId == request.TeamId && m.MemberId == request.UserId && m.Status == (int)MemberStatus.Membered))
                {
                    //var messages = await TTXTeamRepository<ChatTeamViewModel>.Instance.GetModelListByAsync(m => m.Id == request.TeamId, m => m.CreatedDate, "desc", 0, 10);
                    ChatTeamViewModel team = await TTXTeamRepository<ChatTeamViewModel>.Instance.GetSingleModelAsync
                        (t => t.Id == request.TeamId);
                    TTXTeamMemberRepository<FETeamMemberViewModel>.Instance.UpdateLastSeenMessages(request.UserId, request.TeamId);
                    result = new MessengerHubResponse<ChatTeamViewModel>()
                    {
                        status = 1,
                        responseKey = string.Format("{0}", action),
                        data = team,
                        errors = null
                    };
                }
                else
                {
                    result = new MessengerHubResponse<ChatTeamViewModel>()
                    {
                        status = 0,
                        responseKey = GameResponseKey.NotAuthorized.ToString(),
                        data = null,
                        errors = new List<string>() { "You are not membered of this team" }
                    };
                    Clients.Caller.receiveMessage(result);
                }
            }
            catch (Exception ex)
            {
                result = new MessengerHubResponse<ChatTeamViewModel>()
                {
                    status = 0,
                    responseKey = string.Format("{0}", action),
                    data = null,
                    error = ex.StackTrace
                };
            }
            finally
            {
                Clients.Caller.receiveMessage(result);
                UpdatePlayerConnectionIdAsync(request.UserId);
            }
        }

        [HubMethodName("seenTeamMessages")]
        public async Task LeaveTeamMessagesAsync(ApiGetTeamMemberViewMdoel request)
        {
            if (TTXTeamMemberRepository<FETeamMemberViewModel>.Instance.CheckIsExists(m => m.TeamId == request.TeamId && m.MemberId == request.UserId && m.Status == (int)MemberStatus.Membered))
            {
                TTXTeamMemberRepository<FETeamMemberViewModel>.Instance.UpdateLastSeenMessages(request.UserId, request.TeamId);
                MessengerHubResponse<bool> result = new MessengerHubResponse<bool>()
                {
                    status = 1,
                    responseKey = Enum.GetName(typeof(ApiResponseKey), ApiResponseKey.Succeed),
                    data = true
                };
                Clients.Caller.receiveMessage(result);
            }
            UpdatePlayerConnectionIdAsync(request.UserId);
        }

        [HubMethodName("getTeamNotifications")]
        public async System.Threading.Tasks.Task GetTeamNotifications(ApiGetTeamMemberViewMdoel request)
        {
            string errorMsg = string.Empty;
            string action = Enum.GetName(typeof(TeamMessageReponseKey), TeamMessageReponseKey.GetTeamNotifications);
            try
            {
                switch (request.MemberStatus)
                {
                    case MemberStatus.Requested:
                        var requests = await TTXTeamMemberRepository<FETeamMemberViewModel>.Instance.GetModelListByAsync(m => m.TTX_Team.HostId == request.UserId && m.TeamId == request.TeamId && m.Status == (int)MemberStatus.Requested);
                        if (requests.Count > 0)
                        {
                            var lastSeenTeamRequest = DAL.TTXApp.TTXUserLogDAL.Instance.GetSingleModel(l => l.UserId == request.UserId
                        && l.Action == TTXUserLogActions.LastSeenTeamRequest.ToString());
                            requests.ForEach(i => i.IsNew = lastSeenTeamRequest == null || lastSeenTeamRequest.LastUpdate.Value < i.CreatedDate);

                            if (lastSeenTeamRequest == null)
                            {
                                lastSeenTeamRequest = new TTX_User_Log()
                                {
                                    Id = Guid.NewGuid(),
                                    Action = TTXUserLogActions.LastSeenTeamRequest.ToString(),
                                    UserId = request.UserId,
                                    CreatedDate = DateTime.UtcNow,
                                    LastUpdate = DateTime.UtcNow
                                };
                            }
                            else
                            {
                                lastSeenTeamRequest.LastUpdate = DateTime.UtcNow;
                            }
                            TTXUserLogDAL.Instance.SaveModel(lastSeenTeamRequest);
                        }

                        MessengerHubResponse<List<FETeamMemberViewModel>> result = new MessengerHubResponse<List<FETeamMemberViewModel>>()
                        {
                            status = 1,
                            responseKey = string.Format("{0}", action),
                            data = requests.OrderByDescending(r => r.CreatedDate).ToList(),
                            errors = null
                        };

                        Clients.Caller.receiveMessage(result);

                        break;

                    case MemberStatus.Invited:
                        var invitations = await TTXTeamMemberRepository<InvitationViewModel>.Instance.GetModelListByAsync(m => m.MemberId == request.UserId && m.Status == (int)MemberStatus.Invited);
                        var lastSeenTeamInvitation = DAL.TTXApp.TTXUserLogDAL.Instance.GetSingleModel(l => l.UserId == request.UserId
                        && l.Action == TTXUserLogActions.LastSeenTeamInvitation.ToString());
                        invitations.ForEach(i => i.IsNew = lastSeenTeamInvitation == null || lastSeenTeamInvitation.LastUpdate.Value < i.CreatedDate);

                        if (lastSeenTeamInvitation == null)
                        {
                            lastSeenTeamInvitation = new TTX_User_Log()
                            {
                                Id = Guid.NewGuid(),
                                Action = TTXUserLogActions.LastSeenTeamInvitation.ToString(),
                                UserId = request.UserId,
                                CreatedDate = DateTime.UtcNow,
                                LastUpdate = DateTime.UtcNow
                            };
                        }
                        else
                        {
                            lastSeenTeamInvitation.LastUpdate = DateTime.UtcNow;
                        }
                        TTXUserLogDAL.Instance.SaveModel(lastSeenTeamInvitation);

                        MessengerHubResponse<List<InvitationViewModel>> inviteResult = new MessengerHubResponse<List<InvitationViewModel>>()
                        {
                            status = 1,
                            responseKey = string.Format("{0}", action),
                            data = invitations.OrderByDescending(r => r.CreatedDate).ToList(),
                            errors = null
                        };

                        Clients.Caller.receiveMessage(inviteResult);

                        break;

                    case MemberStatus.AdminRejected:
                        break;

                    case MemberStatus.MemberRejected:
                        break;

                    case MemberStatus.Banned:
                        break;

                    case MemberStatus.Membered:
                        break;

                    case MemberStatus.AdminRemoved:
                        break;

                    case MemberStatus.MemberCanceled:
                        break;

                    case MemberStatus.Guest:
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                UpdatePlayerConnectionIdAsync(request.UserId);
            }
        }

        [HubMethodName("getMyTeams")]
        public async System.Threading.Tasks.Task GetMyTeams(RequestPaging request)
        {
            string errorMsg = string.Empty;
            int status = 0;
            MessengerHubResponse<PaginationModel<ChatTeamViewModel>> result = null;
            string action = "GetMyTeams";
            //PaginationModel<ChatTeamViewModel> currentMessages = new PaginationModel<ChatTeamViewModel>();
            try
            {
                PaginationModel<ChatTeamViewModel> teams = await TTXTeamRepository<ChatTeamViewModel>.Instance.GetModelListByAsync(
                    t => t.TTX_Team_Member.Count(m => m.MemberId == request.UserId && m.Status == (int)MemberStatus.Membered) > 0,
                    t => t.CreatedDate, "desc", request.PageIndex, request.PageSize
                        );
                result = new MessengerHubResponse<PaginationModel<ChatTeamViewModel>>()
                {
                    status = 1,
                    responseKey = string.Format("{0}", action),
                    data = teams,
                    errors = null
                };
            }
            catch (Exception ex)
            {
                result = new MessengerHubResponse<PaginationModel<ChatTeamViewModel>>()
                {
                    status = 0,
                    responseKey = action,
                    data = null,
                    error = ex.StackTrace
                };
            }
            finally
            {
                Clients.Caller.receiveMessage(result);
                UpdatePlayerConnectionIdAsync(request.UserId);
            }
        }

        [HubMethodName("removeTeam")]
        public async System.Threading.Tasks.Task RemoveTeam(ApiGetTeamMemberViewMdoel request)
        {
            string errorMsg = string.Empty;
            int status = 0;
            MessengerHubResponse<bool> result = null;
            string action = Enum.GetName(typeof(TeamMessageReponseKey), TeamMessageReponseKey.RemovedTeam);
            //PaginationModel<ChatTeamViewModel> currentMessages = new PaginationModel<ChatTeamViewModel>();
            try
            {
                if (TTXTeamMemberRepository<FETeamMemberViewModel>.Instance.CheckIsExists(m => m.TeamId == request.TeamId && m.MemberId == request.UserId && m.Status == (int)MemberStatus.Membered))
                {
                    var removeResult = await TTXTeamRepository<ChatTeamViewModel>.Instance.RemoveModelAsync(t => t.Id == request.TeamId);

                    if (removeResult.IsSucceed)
                    {
                        result = new MessengerHubResponse<bool>()
                        {
                            status = 1,
                            responseKey = string.Format("{0}", action),
                            data = removeResult.IsSucceed,
                            errors = null
                        };
                    }
                    else
                    {
                        result = new MessengerHubResponse<bool>()
                        {
                            status = 0,
                            responseKey = TeamMessageReponseKey.NotMembered.ToString(),
                            data = false,
                            errors = null
                        };
                    }
                }
                else
                {
                    result = new MessengerHubResponse<bool>()
                    {
                        status = 0,
                        responseKey = GameResponseKey.NotAuthorized.ToString(),
                        data = false,
                        errors = new List<string>() { "You are not membered of this team" }
                    };
                    Clients.Caller.receiveMessage(result);
                }
            }
            catch (Exception ex)
            {
                result = new MessengerHubResponse<bool>()
                {
                    status = 0,
                    responseKey = string.Format("{0}", action),
                    data = false,
                    error = ex.StackTrace
                };
            }
            finally
            {
                Clients.Caller.receiveMessage(result);
                UpdatePlayerConnectionIdAsync(request.UserId);
            }
        }
        */

        #endregion Team - TODO Write these functions

        #region Overrides

        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, defaultRoom);
            await base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            // Get current disconnected device
            var getUserDevice = Service.ViewModels.SignalR.MixMessengerUserDevices.DefaultViewModel.Repository.GetSingleModel(u => u.ConnectionId == Context.ConnectionId);
            if (getUserDevice.IsSucceed)
            {
                getUserDevice.Data.Status = DeviceStatus.Disconnected;
                var countOnlineDevice = Service.ViewModels.SignalR.MixMessengerUserDevices.DefaultViewModel.Repository.Count(d => d.UserId == getUserDevice.Data.UserId && d.DeviceId != getUserDevice.Data.DeviceId && d.Status == 1).Data;
                if (countOnlineDevice == 0)
                {
                    SendToAll(getUserDevice.Data.UserId, MessageReponseKey.MemberOffline, false);
                }
                var getUser = Service.ViewModels.SignalR.MixMessengerUsers.DefaultViewModel.Repository.GetSingleModel(u => u.Id == getUserDevice.Data.UserId);
                if (getUser.IsSucceed)
                {
                    getUser.Data.Status = OnlineStatus.Disconnected;
                    getUser.Data.SaveModel();
                    SendToAll(getUserDevice.Data.UserId, MessageReponseKey.MemberOffline, false);
                }
            }
            //SendToAll(user, MessageReponseKey.RemoveMember, false);

            return base.OnDisconnectedAsync(exception);
        }

        #endregion Overrides
    }
}