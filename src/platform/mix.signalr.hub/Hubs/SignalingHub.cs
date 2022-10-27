using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Mix.Database.Entities.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Mix.Signalr.Hub.Hubs
{
    [AllowAnonymous]
    public class SignalingHub : Microsoft.AspNetCore.SignalR.Hub
    {
        public static Dictionary<string, List<string>> ConnectedClients = new Dictionary<string, List<string>>();

        public async Task SendMessage(object message, string roomName)
        {
            await EmitLog("Client " + Context.ConnectionId + " said: " + message, roomName);

            await Clients.OthersInGroup(roomName).SendAsync("message", message);
        }

        public async Task CreateOrJoinRoom(string roomName)
        {
            await EmitLog("Received request to create or join room " + roomName + " from a client " + Context.ConnectionId, roomName);

            if (!ConnectedClients.ContainsKey(roomName))
            {
                ConnectedClients.Add(roomName, new List<string>());
            }

            if (!ConnectedClients[roomName].Contains(Context.ConnectionId))
            {
                ConnectedClients[roomName].Add(Context.ConnectionId);
            }

            await EmitJoinRoom(roomName);

            var numberOfClients = ConnectedClients[roomName].Count;

            if (numberOfClients == 1)
            {
                await EmitCreated();
                await EmitLog("Client " + Context.ConnectionId + " created the room " + roomName, roomName);
            }
            else
            {
                await EmitJoined(roomName);
                await EmitLog("Client " + Context.ConnectionId + " joined the room " + roomName, roomName);
            }

            await EmitLog("Room " + roomName + " now has " + numberOfClients + " client(s)", roomName);
        }

        public async Task LeaveRoom(string roomName)
        {
            await EmitLog("Received request to leave the room " + roomName + " from a client " + Context.ConnectionId, roomName);

            if (ConnectedClients.ContainsKey(roomName) && ConnectedClients[roomName].Contains(Context.ConnectionId))
            {
                ConnectedClients[roomName].Remove(Context.ConnectionId);
                await EmitLog("Client " + Context.ConnectionId + " left the room " + roomName, roomName);

                if (ConnectedClients[roomName].Count == 0)
                {
                    ConnectedClients.Remove(roomName);
                    await EmitLog("Room " + roomName + " is now empty - resetting its state", roomName);
                }
            }

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
        }

        private async Task EmitJoinRoom(string roomName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
        }

        private async Task EmitCreated()
        {
            await Clients.Caller.SendAsync("created");
        }

        private async Task EmitJoined(string roomName)
        {
            await Clients.Group(roomName).SendAsync("joined");
        }

        private async Task EmitLog(string message, string roomName)
        {
            await Clients.Group(roomName).SendAsync("log", "[Server]: " + message);
        }
    }
}
