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
using Newtonsoft.Json.Linq;
using System.Text;

namespace Mix.Service.Services
{
    public class AuditLogService : IAuditLogService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IQueueService<MessageQueueModel> _queueService;
        private AuditLogDbContext _dbContext;

        public AuditLogService(IServiceScopeFactory serviceScopeFactory, IQueueService<MessageQueueModel> queueService)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _queueService = queueService;
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

                    if (request.Exception != null)
                    {
                        await MixLogService.LogMessageAsync(request.Endpoint, request.Exception, msgType: SignalR.Enums.MessageType.Error);
                    }
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



        #endregion
    }
}
