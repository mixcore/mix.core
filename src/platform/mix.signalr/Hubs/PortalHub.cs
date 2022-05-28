using Microsoft.AspNetCore.Authorization;
using Mix.SignalR.Models;
using System.Threading.Tasks;

namespace Mix.SignalR.Hubs
{
    [Authorize]
    public class PortalHub : BaseSignalrHub
    {
        public override async Task JoinRoom(string room)
        {
            
            var msg = new SignalRMessageModel<object>()
            {
                Title = $"{Context.User.Identity.Name} joined {room}",
                Message = $"New member {Context.ConnectionId}"
            };
            await Groups.AddToGroupAsync(Context.ConnectionId, room);
            await SendMessageToGroups(msg.ToString(), room);
        }
    }
}