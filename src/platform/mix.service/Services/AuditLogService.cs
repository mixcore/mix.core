using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Mix.Constant.Constants;
using Mix.Database.Entities.AuditLog;
using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Mix.Shared.Commands;
using Mix.Shared.Models;
using Newtonsoft.Json.Linq;
using System.Text;

namespace Mix.Service.Services
{
    public class AuditLogService
    {
        private AuditLogDbContext? _dbContext;
        private readonly IServiceProvider servicesProvider;
        protected readonly IQueueService<MessageQueueModel> _queueService;
        public AuditLogService(IServiceProvider servicesProvider, IQueueService<MessageQueueModel> queueService)
        {
            this.servicesProvider = servicesProvider;
            _queueService = queueService;
        }
        public void SaveToDatabase(string createdBy, ParsedRequestModel request, bool isSucceed, Exception exception)
        {
            try
            {
                using var scope = servicesProvider.CreateScope();
                _dbContext = scope.ServiceProvider.GetService<AuditLogDbContext>();
                if (_dbContext != null)
                {
                    var pendingMigrations = _dbContext.Database.GetPendingMigrations();
                    if (pendingMigrations.Count() > 0)
                    {
                        _dbContext.Database.Migrate();
                    }

                    string body = request.Body;
                    var msg = new AuditLog()
                    {
                        Id = Guid.NewGuid(),
                        Body = body,
                        CreatedDateTime = DateTime.UtcNow,
                        RequestIp = request.RequestIp,
                        Endpoint = request.Endpoint,
                        Method = request.Method,
                        CreatedBy = createdBy
                    };
                    if (exception != null)
                    {
                        msg.Exception = JObject.FromObject(exception).ToString(Newtonsoft.Json.Formatting.None);
                    }
                    msg.Success = isSucceed;
                    _dbContext.AuditLog.Add(msg);
                    _dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                MixService.LogException(ex);
            }
        }

        public void LogRequest(HttpContext context)
        {
            context.Request.EnableBuffering();
            var request = new ParsedRequestModel(
                $"{context.Request.HttpContext.Connection.RemoteIpAddress} - {context.Request.Headers.Referer}",
                context.Request.Path,
                context.Request.Method,
                GetBody(context.Request)
                );
            LogRequest(context.User.Identity?.Name, request);
            context.Request.Body.Seek(0, SeekOrigin.Begin);
        }

        public void LogRequest(string? createdBy, ParsedRequestModel request)
        {
            var cmd = new LogAuditLogCommand(createdBy, request);
            _queueService.PushQueue(MixQueueTopics.MixBackgroundTasks, MixQueueActions.AuditLog, cmd);
        }

        private string GetBody(HttpRequest request)
        {
            var bodyStr = "";
            // Arguments: Stream, Encoding, detect encoding, buffer size 
            // AND, the most important: keep stream opened
            using (StreamReader reader
                      = new StreamReader(request.BodyReader.AsStream(), Encoding.UTF8, true, 1024, true))
            {
                bodyStr = reader.ReadToEndAsync().GetAwaiter().GetResult();
            }

            return bodyStr;
        }
    }
}
