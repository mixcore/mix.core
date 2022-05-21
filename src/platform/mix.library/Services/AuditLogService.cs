using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Mix.Database.Entities.AuditLog;
using Mix.Lib.Models;

namespace Mix.Lib.Services
{
    public class AuditLogService
    {
        private AuditLogDbContext _dbContext;
        private readonly IServiceProvider servicesProvider;

        public AuditLogService(IServiceProvider servicesProvider)
        {
            
            
            this.servicesProvider = servicesProvider;
        }
        public void Log(string createdBy, ParsedRequestModel request, bool isSucceed, Exception exception)
        {
            try
            {
                using var scope = servicesProvider.CreateScope();
                _dbContext = scope.ServiceProvider.GetService<AuditLogDbContext>();
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
            catch (Exception ex)
            {
                MixService.LogException(ex);
            }
        }
    }
}
