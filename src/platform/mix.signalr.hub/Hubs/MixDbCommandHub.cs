using Mix.Constant.Constants;
using Mix.Heart.Helpers;
using Mix.Lib.Interfaces;
using Mix.Mq.Lib.Models;
using Mix.Queue.Interfaces;
using Mix.Service.Models;

namespace Mix.SignalR.Hubs
{
    public class MixDbCommandHub : BaseSignalRHub
    {
        private readonly IMemoryQueueService<MessageQueueModel> _queueService;
        public MixDbCommandHub(IAuditLogService auditLogService, IMixTenantService mixTenantService, IMemoryQueueService<MessageQueueModel> queueService) : base(auditLogService, mixTenantService)
        {
            _queueService = queueService;
        }

        public virtual void CreateData(string message)
        {
            var obj = ReflectionHelper.ParseStringToObject<MixDbCommandModel>(message);
            obj.MixTenantId = CurrentUser?.TenantId ?? 1;
            obj.RequestedBy = Context.User?.Identity?.Name;
            obj.ConnectionId = Context.ConnectionId;
            _queueService.PushMemoryQueue(obj.MixTenantId, MixQueueTopics.MixDbCommand, MixDbCommandQueueActions.Create, obj);
        }

        public virtual void UpdateData(string message)
        {
            var obj = ReflectionHelper.ParseStringToObject<MixDbCommandModel>(message);
            obj.RequestedBy = Context.User?.Identity?.Name;
            obj.ConnectionId = Context.ConnectionId;
            _queueService.PushMemoryQueue(obj.MixTenantId, MixQueueTopics.MixDbCommand, MixDbCommandQueueActions.Update, obj);
        }
    }
}