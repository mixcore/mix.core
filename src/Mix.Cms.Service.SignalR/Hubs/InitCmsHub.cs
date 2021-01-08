using Microsoft.AspNetCore.SignalR;
using Mix.Cms.Service.SignalR.Domain.Constants;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Mix.Cms.Service.SignalR.Hubs
{
    public class InitCmsHub : BaseSignalRHub
    {
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync(MixHubMethods.ReceiveMethod, message).ConfigureAwait(false);
        }

        public Task SendMessageToCaller(string message)
        {
            return Clients.Caller.SendAsync(MixHubMethods.ReceiveMethod, message);
        }

        public Task SendMessageToGroups(string message)
        {
            List<string> groups = new List<string>() { "SignalR Users" };
            return Clients.Groups(groups).SendAsync(MixHubMethods.ReceiveMethod, message);
        }

        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "SignalR Users").ConfigureAwait(true);
            await base.OnConnectedAsync().ConfigureAwait(false);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "SignalR Users").ConfigureAwait(true);

            await base.OnDisconnectedAsync(exception).ConfigureAwait(false);
        }
    }
}