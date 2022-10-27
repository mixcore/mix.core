using Microsoft.AspNetCore.SignalR;
using Microsoft.Azure.ServiceBus;
using Mix.Heart.Helpers;
using Mix.Service.Services;
using Mix.SignalR.Constants;
using Mix.SignalR.Models;
using Newtonsoft.Json.Linq;
using System.Runtime.CompilerServices;

namespace Mix.SignalR.Hubs
{
    public class HighFrequencyHub : BaseSignalRHub
    {
        public HighFrequencyHub(AuditLogService auditLogService) : base(auditLogService)
        {
        }
        public async Task UploadStream(IAsyncEnumerable<string> stream, string room)
        {
            await foreach (var message in stream)
            {
                try
                {
                    var msg = JObject.Parse(message).ToObject<SignalRMessageModel>();
                    msg.From ??= GetCurrentUser();
                    _ = Clients.GroupExcept(room, Context.ConnectionId).SendAsync(HubMethods.ReceiveMethod, msg.ToString());
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
    }
}