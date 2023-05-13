using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Mix.Constant.Constants;
using Mix.Heart.Helpers;
using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Mix.Service.Interfaces;
using Mix.Service.Models;
using Mix.Signalr.Hub.Models;
using Mix.SignalR.Constants;
using Mix.SignalR.Models;

namespace Mix.SignalR.Hubs
{
    public class MixDbCommandHub : BaseSignalRHub
    {
        private readonly IQueueService<MessageQueueModel> _queueService;
        public MixDbCommandHub(IAuditLogService auditLogService, IQueueService<MessageQueueModel> queueService) : base(auditLogService)
        {
            _queueService = queueService;
        }

        public virtual void CreateData(string message)
        {
            var obj = ReflectionHelper.ParseStringToObject<MixDbCommandModel>(message);
            obj.RequestedBy = Context.User?.Identity?.Name;
            obj.ConnectionId = Context.ConnectionId;
            _queueService.PushQueue(MixQueueTopics.MixDbCommand, MixDbCommandQueueActions.Create, obj);
        }
    }
}