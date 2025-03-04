using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Mix.Constant.Constants;
using Mix.Database.Entities.AuditLog;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Heart.Enums;
using Mix.Heart.Helpers;
using Mix.Log.Lib.Commands;
using Mix.Log.Lib.Interfaces;
using Mix.Log.Lib.Models;
using Mix.Mq.Lib.Models;
using Mix.Queue.Interfaces;
using Mix.Service.Models;
using Mix.Service.Services;
using Mix.SignalR.Enums;
using Mix.SignalR.Interfaces;
using Mix.SignalR.Models;
using Mix.Shared.Extensions;

namespace Mix.Log.Lib.Services
{
    public class AuditLogService : IAuditLogService
    {
        private readonly DatabaseService _databaseService;
        private readonly ILogStreamHubClientService _logStreamHub;
        private readonly IMemoryQueueService<MessageQueueModel> _queueService;
        private AuditLogDbContext _dbContext;
        protected ISession? Session;
        private MixTenantSystemModel _currentTenant;
        public MixTenantSystemModel CurrentTenant
        {
            get
            {
                if (_currentTenant == null)
                {
                    _currentTenant = Session?.Get<MixTenantSystemModel>(MixRequestQueryKeywords.Tenant) ?? new MixTenantSystemModel()
                    {
                        Id = 1
                    };
                }
                return _currentTenant;
            }
        }
        public AuditLogService(IHttpContextAccessor httpContextAccessor, IMemoryQueueService<MessageQueueModel> queueService, ILogStreamHubClientService logStreamHub, DatabaseService databaseService)
        {
            Session = httpContextAccessor?.HttpContext?.Session;
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
            _queueService.PushMemoryQueue(CurrentTenant.Id, MixQueueTopics.MixLog, MixQueueActions.AuditLog, cmd);
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
                From = new("LogSettings Stream Service"),
                Data = obj?.ToString(Newtonsoft.Json.Formatting.None),
                Message = ex == null ? message : ex!.Message
            };
            await _logStreamHub.SendMessageAsync(msg);
        }

        #endregion
    }
}
