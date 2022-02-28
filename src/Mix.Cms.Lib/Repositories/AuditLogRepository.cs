using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Mix.Cms.Lib.Models;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Mix.Cms.Lib.Repositories
{
    public class AuditLogRepository
    {
        private readonly AuditContext _dbContext;

        public AuditLogRepository(AuditContext dbContext)
        {
            _dbContext = dbContext;
            var pendingMigrations = _dbContext.Database.GetPendingMigrations();
            if (pendingMigrations.Count() > 0)
            {
                _dbContext.Database.Migrate();
            }
        }
        public void Log(string createdBy, HttpRequest request, bool isSucceed, Exception exception)
        {
            Guid.TryParse(request.Headers["RequestId"], out Guid id);
            var msg = _dbContext.AuditLog.Find(id);
            if (msg == null)
            {
                string body = GetBody(request);
                msg = new AuditLog()
                {
                    Id = id,
                    Body = body,
                    CreatedDateTime = DateTime.UtcNow,
                    RequestIp = $"{request.Headers.Referer} - {request.HttpContext.Connection.RemoteIpAddress}",
                    Endpoint = request.Path,
                    Method = request.Method,
                    CreatedBy = createdBy
                };
                _dbContext.AuditLog.Add(msg);
            }
            if (exception != null)
            {
                msg.Exception = JObject.FromObject(exception).ToString(Newtonsoft.Json.Formatting.None);
            }
            msg.Success = isSucceed;
            _dbContext.SaveChanges();
        }
        private string GetBody(HttpRequest request)
        {
            var bodyStr = "";
            string id = request.Headers["RequestId"];
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
