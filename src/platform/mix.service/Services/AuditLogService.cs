using Google.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Mix.Constant.Constants;
using Mix.Database.Entities.AuditLog;
using Mix.Heart.Enums;
using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Mix.Service.Interfaces;
using Mix.Shared.Commands;
using Mix.Shared.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Text;

namespace Mix.Service.Services
{
    public class AuditLogService : IAuditLogService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IQueueService<MessageQueueModel> _queueService;

        public AuditLogService(IServiceScopeFactory serviceScopeFactory, IQueueService<MessageQueueModel> queueService)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _queueService = queueService;
        }

        private static string GetBodyAsync(HttpRequest request)
        {
            var bodyStr = string.Empty;

            // Arguments: Stream, Encoding, detect encoding, buffer size 
            // AND, the most important: keep stream opened
            try
            {
                using (var reader = new StreamReader(request.BodyReader.AsStream(), Encoding.UTF8, true, 1024, true))
                {
                    bodyStr = reader.ReadToEnd();
                }
            }
            catch
            {
                Console.WriteLine($"{nameof(AuditLogService)}: Cannot read body request");
            }

            return bodyStr;
        }

        public async Task SaveRequestAsync(Guid id, string createdBy, ParsedRequestModel request)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetService<AuditLogDbContext>();
                    if (dbContext is null)
                    {
                        return;
                    }

                    if (dbContext.Database.GetPendingMigrations().Any())
                    {
                        dbContext.Database.Migrate();
                    }

                    var log = new AuditLog
                    {
                        Id = id,
                        Body = request.Body,
                        CreatedDateTime = DateTime.UtcNow,
                        RequestIp = request.RequestIp,
                        Endpoint = request.Endpoint,
                        Method = request.Method,
                        CreatedBy = createdBy,
                        Status = MixContentStatus.Published,
                        //Success = isSucceed,
                        //Exception = exception is not null ? JObject.FromObject(exception).ToString(Newtonsoft.Json.Formatting.None) : null
                    };

                    dbContext.AuditLog.Add(log);
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                MixLogService.LogExceptionAsync(ex).GetAwaiter().GetResult();
            }
        }

        public async Task SaveResponseAsync(Guid id, int statusCode, Exception ex)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetService<AuditLogDbContext>();
                    if (dbContext is null)
                    {
                        return;
                    }

                    if (dbContext.Database.GetPendingMigrations().Any())
                    {
                        dbContext.Database.Migrate();
                    }

                    var request = dbContext.AuditLog.FirstOrDefault(m => m.Id == id);
                    if (request is null)
                    {
                        Console.Error.WriteLine($"Request not found: {id}");
                        return;
                    }

                    request.Success = 200 <= statusCode && statusCode <= 299;
                    request.Exception = ex is not null ? JObject.FromObject(ex).ToString(Newtonsoft.Json.Formatting.None) : null;
                    dbContext.Update(request);
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception exception)
            {
                MixLogService.LogExceptionAsync(exception).GetAwaiter().GetResult();
            }
        }
        #region Helpers
        public void LogRequest(Guid id, HttpContext context)
        {
            context.Request.EnableBuffering();
            var request = new ParsedRequestModel(
                context.Request.HttpContext?.Connection?.RemoteIpAddress?.ToString(),
                context.Request.Path,
                context.Request.Method,
                GetBodyAsync(context.Request)
            );

            LogRequest(id, context.User?.Identity?.Name, request);
            context.Request.Body.Seek(0, SeekOrigin.Begin);
        }

        public void LogResponse(Guid id, HttpResponse response, Exception? ex)
        {
            // Log response after executing the action
            var cmd = new LogAuditLogCommand(id, response.StatusCode, ex);
            _queueService.PushQueue(MixQueueTopics.MixBackgroundTasks, MixQueueActions.AuditLog, cmd);
        }

        public void LogRequest(Guid id, string createdBy, ParsedRequestModel request)
        {
            var cmd = new LogAuditLogCommand(id, createdBy, request);
            _queueService.PushQueue(MixQueueTopics.MixBackgroundTasks, MixQueueActions.AuditLog, cmd);
        }

        #endregion
    }
}
