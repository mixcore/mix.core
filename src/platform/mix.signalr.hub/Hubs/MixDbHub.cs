﻿using Google.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Mix.Auth.Constants;
using Mix.Constant.Constants;
using Mix.Constant.Enums;
using Mix.Database.Entities.Cms;
using Mix.Heart.Helpers;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Interfaces;
using Mix.Mq.Lib.Models;
using Mix.Queue.Interfaces;
using Mix.Service.Interfaces;
using Mix.Service.Models;
using Mix.Signalr.Hub.Models;
using Mix.SignalR.Constants;
using Mix.SignalR.Enums;
using Mix.SignalR.Models;
using Newtonsoft.Json.Linq;
using System.Security.Claims;

namespace Mix.SignalR.Hubs
{
    [Authorize]
    public class MixDbHub : BaseSignalRHub
    {
        private readonly IMixMemoryCacheService _memoryCache;
        private readonly MixCmsContext _ctx;
        private readonly IMemoryQueueService<MessageQueueModel> _queueService;
        public MixDbHub(IMixTenantService mixTenantService, IMemoryQueueService<MessageQueueModel> queueService, IMixMemoryCacheService memoryCache, MixCmsContext ctx) 
            : base(mixTenantService)
        {
            _queueService = queueService;
            _memoryCache = memoryCache;
            _ctx = ctx;
        }

        #region Methods
        
        public async void JoinRooms(string[] roomNames)
        {
            foreach (var tableName in roomNames)
            {
                if (await CheckUserPermission(tableName, "GET"))
                {
                    await AddUserToRoom($"mixdb-{tableName}");
                }
            }
        }

        public virtual async Task CreateDataAsync(string tableName, JObject data)
        {

            if (await CheckUserPermission(tableName, "POST"))
            {
                var obj = new MixDbCommandModel()
                {
                    MixDbName = tableName,
                    Body = data,
                    ConnectionId = Context.ConnectionId,
                    MixTenantId = 1,
                    RequestedBy = GetCurrentUser().Username
                };
                _queueService.PushMemoryQueue(obj.MixTenantId, MixQueueTopics.MixDbCommand, MixDbCommandQueueAction.Create.ToString(), obj);
            }
        }
        
        public virtual async Task DeleteDataAsync(string tableName, JObject data)
        {

            if (await CheckUserPermission(tableName, "DELETE"))
            {
                var obj = new MixDbCommandModel()
                {
                    MixDbName = tableName,
                    Body = data,
                    ConnectionId = Context.ConnectionId,
                    MixTenantId = 1,
                    RequestedBy = GetCurrentUser().Username
                };
                _queueService.PushMemoryQueue(obj.MixTenantId, MixQueueTopics.MixDbCommand, MixDbCommandQueueAction.Delete.ToString(), obj);
            }
        }

        public virtual async Task UpdateData(string tableName, JObject data)
        {
            if (await CheckUserPermission(tableName, "PUT"))
            {
                var obj = new MixDbCommandModel()
                {
                    MixDbName = tableName,
                    Body = data,
                    ConnectionId = Context.ConnectionId,
                    MixTenantId = 1,
                    RequestedBy = GetCurrentUser().Username
                };
                _queueService.PushMemoryQueue(obj.MixTenantId, MixQueueTopics.MixDbCommand, MixDbCommandQueueAction.Update.ToString(), obj);
            }
        }
        #endregion
        #region Private

        private async Task<bool> CheckUserPermission(string tableName, string action)
        {
            try
            {
                var tbl = await GetMixDatabase(tableName);
                if (tbl == null)
                {
                    await SendErrorMessageToCaller($"Invalid table name {tableName}");
                    return false;
                }

                if (CheckByPassAuthenticate(action, tbl))
                {
                    return true;
                }

                if (Context.User == null)
                {
                    await SendErrorMessageToCaller($"Unauthorized");
                    return false;
                }
                
                if (!IsInRoles(action, tbl))
                {
                    await SendErrorMessageToCaller($"You don't have permission to access {tableName}");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                await SendErrorMessageToCaller(ex.Message);
                return false;
            }

        }

        private bool IsInRoles(string method, MixDatabase database)
        {

            var userRoles = GetClaim(Context.User!, MixClaims.Role).Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(r => r.Trim()).ToArray();

            if (userRoles.Any(r => r == MixRoles.SuperAdmin || r == $"{MixRoles.Owner}-1"))
            {
                return true;
            }

            switch (method)
            {
                case "GET": return CheckUserInRoles(database.ReadPermissions, userRoles);
                case "POST":
                    return CheckUserInRoles(database.CreatePermissions, userRoles);
                case "PATCH":
                case "PUT": return CheckUserInRoles(database.UpdatePermissions, userRoles);
                case "DELETE": return CheckUserInRoles(database.DeletePermissions, userRoles);
                default:
                    return false;
            }
        }

        private bool CheckUserInRoles(List<string> allowedRoles, string[] userRoles)
        {
            return allowedRoles == null || allowedRoles.Count == 0 || allowedRoles.Any(r => userRoles.Any(ur => ur == $"{r}-1"));
        }

        private string GetClaim(ClaimsPrincipal User, string claimType)
        {
            if (User == null)
            {
                return null;
            }
            return string.Join(',', User.Claims.Where(c => c.Type == claimType).Select(m => m.Value));
        }

        private bool CheckByPassAuthenticate(string method, MixDatabase database)
        {
            return method switch
            {
                "GET" => database.ReadPermissions == null
                        || database.ReadPermissions.Count == 0,
                "POST" => database.CreatePermissions == null
                        || database.CreatePermissions.Count == 0,
                "PUT" => database.UpdatePermissions == null
                        || database.UpdatePermissions.Count == 0,
                "PATCH" => database.UpdatePermissions == null
                        || database.UpdatePermissions.Count == 0,
                "DELETE" => database.DeletePermissions == null
                        || database.DeletePermissions.Count == 0,
                _ => false
            };
        }

        private async Task SendErrorMessageToCaller(string errMsg)
        {
            await SendMessageToCaller(new SignalRMessageModel()
            {
                Type = MessageType.Error,
                Message = errMsg,
            });
        }

        private async Task<MixDatabase?> GetMixDatabase(string tableName)
        {
            return await _memoryCache.TryGetValueAsync<MixDatabase?>(
                tableName,
                cache =>
                {
                    cache.SlidingExpiration = TimeSpan.FromSeconds(20);
                    var db = _ctx.MixDatabase.SingleOrDefault(m => m.SystemName == tableName);
                    return Task.FromResult(db);
                }
                );
        }
        #endregion
    }
}