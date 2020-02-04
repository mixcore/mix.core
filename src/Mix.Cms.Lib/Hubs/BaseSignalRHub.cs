// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.SignalR;
using Mix.Domain.Core.Models;
using Mix.Domain.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mix.UI.Core.SignalR
{
    /// <summary>
    /// Base SignalR Hub
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.SignalR.Hub"/>
    public abstract class BaseSignalRHub : Hub
    {
        /// <summary>
        /// The users
        /// </summary>
        protected static readonly List<SignalRClient> Users = new List<SignalRClient>();

        /// <summary>
        /// Called when [connected asynchronous].
        /// </summary>
        /// <returns></returns>
        public override Task OnConnectedAsync()
        {
            UpdateGroupConnection();
            return base.OnConnectedAsync();
        }

        /// <summary>
        /// Called when [disconnected asynchronous].
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns></returns>
        public override Task OnDisconnectedAsync(Exception exception)
        {
            // Remove the user
            Users.RemoveAll(u => u.ConnectionId == Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// Updates the group connection.
        /// </summary>
        public virtual void UpdateGroupConnection()
        {
            var user = Users.Find(p => p.ConnectionId == Context.ConnectionId);
            if (user != null)
            {
                // Loop Group to Update UserId
                //player.ConnectionId = Context.ConnectionId;
            }
        }

        /// <summary>
        /// Updates the player connection identifier asynchronous.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        public virtual void UpdatePlayerConnectionIdAsync(string playerId)
        {
            // TODO: is Async method? If not => Remove Async from method name!
            var player = Users.Find(p => p.UserId == playerId);
            if (player != null && player.ConnectionId != Context.ConnectionId)
            {
                //Missing Update current groups user connId

                player.ConnectionId = Context.ConnectionId;
                //player.SaveModel();
            }
        }

        /// <summary>
        /// Fails the result.
        /// </summary>
        /// <param name="objData">The object data.</param>
        /// <param name="errorMsg">The error MSG.</param>
        private void FailResult(dynamic objData, string errorMsg)//AccessTokenViewModel accessToken
        {
            const string responseKey = "Failed";
            const int status = 0;
            ApiResult<dynamic> result = new ApiResult<dynamic>()
            {
                ResponseKey = responseKey,
                Status = status,
                Data = objData,
                //authData = accessToken,
            };
            Clients.Client(Context.ConnectionId).SendAsync("receiveMessage", result);
        }
    }
}