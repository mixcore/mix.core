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
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IQueueService<MessageQueueModel> _queueService;
        private AuditLogDbContext _dbContext;

        public AuditLogService(IServiceScopeFactory serviceScopeFactory, IQueueService<MessageQueueModel> queueService, ILogStreamHubClientService logStreamHub)
        {
            _serviceScopeFactory = serviceScopeFactory;
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
                        Success = request.Exception == null,
                        CreatedDateTime = DateTime.UtcNow,
                        Status = MixContentStatus.Published
                    };
                    ReflectionHelper.Map(request, log);

                    _dbContext.AuditLog.Add(log);
                    await _dbContext.SaveChangesAsync();
                    var msgType = request.Exception == null ? MessageType.Success : MessageType.Error;
                    await SendMessage(request.Endpoint, request.Exception ?? request.Body, msgType: msgType);
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
            _queueService.PushQueue(MixQueueTopics.MixBackgroundTasks, MixQueueActions.AuditLog, cmd);
        }

        #region Helpers
        private static string GetBodyAsync(HttpRequest request)
        {
            var bodyStr = string.Empty;

            // Arguments: Stream, Encoding, detect encoding, buffer size 
            // AND, the most important: keep stream opened
            try
            {
                if (request.Method != "GET" && request.Method != "DELETE" &&
                    (request.ContentLength != null || !request.ContentType.StartsWith("multipart/form-data")))
                {
                    request.EnableBuffering();
                    using (var reader = new StreamReader(request.BodyReader.AsStream(), Encoding.UTF8, true, 1024, true))
                    {
                        bodyStr = reader.ReadToEnd();
                    }
                    request.Body.Seek(0, SeekOrigin.Begin);
                }
            }
            catch
            {
                Console.WriteLine($"{nameof(AuditLogService)}: Cannot read body request");
            }

            return bodyStr;
        }

        private async Task SendMessage(string? message, object? data = default, Exception? ex = null, MessageType msgType = MessageType.Info)
        {
            if (GlobalConfigService.Instance.IsLogStream)
            {
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
        }

        #endregion
    }
}
