using Microsoft.EntityFrameworkCore;
using Mix.Constant.Constants;
using Mix.Database.Entities.AuditLog;
using Mix.Database.Entities.Queue;
using Mix.Heart.Enums;
using Mix.Heart.Helpers;
using Mix.Log.Lib.Interfaces;
using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Mix.Service.Commands;
using Mix.Service.Models;
using Mix.Service.Services;
using Mix.SignalR.Enums;
using Mix.SignalR.Interfaces;
using Mix.SignalR.Models;

namespace Mix.Log.Lib.Services
{
    public class MixQueueLogService : IMixQueueLog
    {
        private MixQueueDbContext _dbContext;
        public int TenantId { get; set; }
        public MixQueueLogService()
        {
        }

        
        public async Task SaveRequestAsync(MixQueueMessageLog log)
        {
            try
            {
                using (_dbContext = new())
                {
                    InitDbContext();

                    _dbContext.MixQueueMessage.Add(log);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                await MixLogService.LogExceptionAsync(ex);
            }
        }

        private void InitDbContext()
        {
            if (_dbContext.Database.GetPendingMigrations().Any())
            {
                _dbContext.Database.Migrate();
            }
        }

    }
}
