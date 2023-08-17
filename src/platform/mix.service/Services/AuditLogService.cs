using MessagePack;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Mix.Constant.Constants;
using Mix.Database.Entities.AuditLog;
using Mix.Heart.Enums;
using Mix.Heart.Helpers;
using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Mix.Service.Commands;
using Mix.Service.Interfaces;
using Mix.Service.Models;
using Mix.Shared.Services;
using Mix.SignalR.Enums;
using Mix.SignalR.Interfaces;
using Mix.SignalR.Models;
using Newtonsoft.Json.Linq;
using System.Text;

namespace Mix.Service.Services
{
    public class AuditLogService : IAuditLogService
    {
        private readonly ILogStreamHubClientService _logStreamHub;
        private readonly IQueueService<MessageQueueModel> _queueService;
        private AuditLogDbContext _dbContext;
        public int TenantId { get; set; }
        public AuditLogService(IQueueService<MessageQueueModel> queueService, ILogStreamHubClientService logStreamHub)
        {
            _queueService = queueService;
            _logStreamHub = logStreamHub;
        }

        public async Task SaveRequestAsync(AuditLogDataModel request)
        {
            try
            {
                using (_dbContext = new())
                {
                    if (_dbContext is null)
                    {
                        return;
                    }

                    if (_dbContext.Database.GetPendingMigrations().Any())
                    {
                        _dbContext.Database.Migrate();
                    }

                    var log = new AuditLog()
                    {
                        Id = Guid.NewGuid(),
                        Success = request.Exception == null,
                        CreatedDateTime = DateTime.UtcNow,
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
            var cmd = new LogAuditLogCommand(request);
            _queueService.PushQueue(TenantId, MixQueueTopics.MixBackgroundTasks, MixQueueActions.AuditLog, cmd);
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
