using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Mix.Cms.Lib.Models;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

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
        internal void Log(string createdBy, HttpRequest request, bool isSucceed, Exception exception)
        {
            string body = GetBody(request);
            var msg = new AuditLog()
            {
                Id = Guid.NewGuid(),
                Exception = JsonSerializer.Serialize(exception),
                Body = body,
                CreatedDateTime = DateTime.UtcNow,
                Endpoint = request.Path,
                Method = request.Method,
                Success = isSucceed,
                CreatedBy = createdBy
            };
            _dbContext.AuditLog.Add(msg);
            _dbContext.SaveChanges();
        }
        private string GetBody(HttpRequest request)
        {
            var bodyStr = "";

            // Arguments: Stream, Encoding, detect encoding, buffer size 
            // AND, the most important: keep stream opened
            using (StreamReader reader
                      = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true))
            {
                bodyStr = reader.ReadToEndAsync().GetAwaiter().GetResult();
            }

            return bodyStr;
        }
    }
}
