using Mix.Constant.Constants;
using Mix.Database.Entities.AuditLog;
using Mix.Database.Services;
using Mix.Heart.Enums;
using Mix.Heart.Helpers;
using Mix.Log.Lib.Commands;
using Mix.Log.Lib.Interfaces;
using Mix.Log.Lib.Models;
using Mix.Mq.Lib.Models;
using Mix.Queue.Interfaces;
using Mix.Service.Services;
using Mix.SignalR.Enums;
using Mix.SignalR.Interfaces;
using Mix.SignalR.Models;

namespace Mix.Log.Lib.Services
{
    public class AuditLogService : IAuditLogService
    {
        private readonly DatabaseService _databaseService;
        private readonly ILogStreamHubClientService _logStreamHub;
        private readonly IMemoryQueueService<MessageQueueModel> _queueService;
        private AuditLogDbContext _dbContext;
        public int TenantId { get; set; }
        public AuditLogService(IMemoryQueueService<MessageQueueModel> queueService, ILogStreamHubClientService logStreamHub, DatabaseService databaseService)
        {
            _queueService = queueService;
            _logStreamHub = logStreamHub;
            _databaseService = databaseService;
        }

        public async Task SaveRequestAsync(AuditLogDataModel request)
        {
            try
            {
                using (_dbContext = _databaseService.GetAuditLogDbContext())
                {
                    var log = new AuditLog()
                    {
                        Id = Guid.NewGuid(),
                        Success = request.Exception == null,
                        CreatedDateTime = request.CreatedAt,
                        Status = MixContentStatus.Published
                    };
                    ReflectionHelper.Map(request, log);
                    log.Success = request.StatusCode < 300;
                    _dbContext.AuditLog.Add(log);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                await MixLogService.LogExceptionAsync(ex);
            }
        }

        public void QueueRequest(AuditLogDataModel request)
        {
            request.CreatedAt = DateTime.UtcNow;
            var cmd = new LogAuditLogCommand(request);
            _queueService.PushMemoryQueue(TenantId, MixQueueTopics.MixLog, MixQueueActions.AuditLog, cmd);
        }

        #region Helpers
        public async Task LogStream(string? message, object? data = default, Exception? ex = null, bool isSuccess = false)
        {
            MessageType msgType = isSuccess ? MessageType.Success : MessageType.Error;
            var obj = ReflectionHelper.ParseObject(data ?? ex);
            SignalRMessageModel msg = new()
            {
                Action = MessageAction.NewMessage,
                Type = msgType,
                Title = message,
                From = new("Log Stream Service"),
                Data = obj?.ToString(Newtonsoft.Json.Formatting.None),
                Message = ex == null ? message : ex!.Message
            };
            await _logStreamHub.SendMessageAsync(msg);
        }

        #endregion
    }
}
