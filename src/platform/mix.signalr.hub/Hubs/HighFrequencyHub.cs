using Microsoft.AspNetCore.SignalR;
using Mix.Lib.Interfaces;
using Mix.SignalR.Constants;
using Mix.SignalR.Models;
using Newtonsoft.Json.Linq;

namespace Mix.SignalR.Hubs
{
    public class HighFrequencyHub : BaseSignalRHub
    {
        public HighFrequencyHub(IAuditLogService auditLogService, IMixTenantService mixTenantService)
            : base(auditLogService, mixTenantService)
        {
        }
        public async Task UploadStream(IAsyncEnumerable<string> stream, string room)
        {
            await foreach (var message in stream)
            {
                try
                {
                    var msg = JObject.Parse(message).ToObject<SignalRMessageModel>();
                    if (msg != null)
                    {

                        msg.From ??= GetCurrentUser();
                        _ = Clients.GroupExcept(room, Context.ConnectionId).SendAsync(HubMethods.ReceiveMethod, msg.ToString());
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
    }
}