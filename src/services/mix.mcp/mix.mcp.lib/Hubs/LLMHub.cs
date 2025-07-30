using Mix.Lib.Interfaces;
using Mix.MCP.Lib.Agents;
using Mix.Service.Interfaces;
using Mix.SignalR.Enums;
using Mix.SignalR.Hubs;
using Mix.SignalR.Models;
using System.Threading;

namespace Mix.MCP.Lib.Hubs
{
    public class LLMHub : BaseSignalRHub
    {
        private readonly RoutingAgent _routingAgent;
        public LLMHub(IMixTenantService mixTenantService, RoutingAgent routingAgent)
            : base(mixTenantService)
        {
            _routingAgent = routingAgent;
        }

        public virtual async Task AskAI(string msg)
        {
            _ = await _routingAgent.ProcessInputAsync(msg, GetCurrentUser().UserName, Context.ConnectionId);
        }

    }
}