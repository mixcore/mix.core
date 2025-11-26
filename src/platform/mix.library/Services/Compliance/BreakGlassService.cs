using Microsoft.EntityFrameworkCore;
using Mix.Database.Entities.Compliance;
using Mix.Database.Services.MixGlobalSettings;

namespace Mix.Lib.Services.Compliance
{
    public class BreakGlassService : IBreakGlassService
    {
        private readonly DatabaseService _databaseService;

        public BreakGlassService(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<BreakGlassAccess> RequestEmergencyAccess(int tenantId, Guid userId, string reason, string justification, int durationMinutes = 30)
        {
            using var context = _databaseService.GetDbContext();
            
            var access = new BreakGlassAccess
            {
                TenantId = tenantId,
                UserId = userId,
                AccessReason = reason,
                Justification = justification,
                AccessStartTime = DateTime.UtcNow,
                AccessEndTime = DateTime.UtcNow.AddMinutes(durationMinutes),
                Status = BreakGlassStatus.Requested,
                DisplayName = $"Emergency Access Request - {reason}",
                Description = justification
            };

            context.Add(access);
            await context.SaveChangesAsync();
            return access;
        }

        public async Task<BreakGlassAccess> ApproveAccess(int tenantId, int accessId, string approvedBy)
        {
            using var context = _databaseService.GetDbContext();
            
            var access = await context.Set<BreakGlassAccess>()
                .FirstOrDefaultAsync(x => x.Id == accessId && x.TenantId == tenantId);

            if (access == null)
                throw new InvalidOperationException("Break glass access request not found");

            if (access.AccessEndTime < DateTime.UtcNow)
                throw new InvalidOperationException("Break glass access request has expired");

            access.Status = BreakGlassStatus.Approved;
            access.ApprovedBy = approvedBy;
            access.AccessStartTime = DateTime.UtcNow;

            await context.SaveChangesAsync();
            return access;
        }

        public async Task<BreakGlassAccess> RevokeAccess(int tenantId, int accessId, string revokedBy)
        {
            using var context = _databaseService.GetDbContext();
            
            var access = await context.Set<BreakGlassAccess>()
                .FirstOrDefaultAsync(x => x.Id == accessId && x.TenantId == tenantId);

            if (access == null)
                throw new InvalidOperationException("Break glass access not found");

            access.Status = BreakGlassStatus.Revoked;
            access.ActualEndTime = DateTime.UtcNow;

            await context.SaveChangesAsync();
            return access;
        }

        public async Task<bool> HasActiveBreakGlassAccess(int tenantId, Guid userId)
        {
            using var context = _databaseService.GetDbContext();
            
            return await context.Set<BreakGlassAccess>()
                .AnyAsync(x => x.TenantId == tenantId && 
                              x.UserId == userId && 
                              (x.Status == BreakGlassStatus.Approved || x.Status == BreakGlassStatus.Active) && 
                              x.AccessEndTime > DateTime.UtcNow);
        }

        public async Task<IEnumerable<BreakGlassAccess>> GetActiveAccesses(int tenantId)
        {
            using var context = _databaseService.GetDbContext();
            
            return await context.Set<BreakGlassAccess>()
                .Where(x => x.TenantId == tenantId && 
                           (x.Status == BreakGlassStatus.Approved || x.Status == BreakGlassStatus.Active) && 
                           x.AccessEndTime > DateTime.UtcNow)
                .ToListAsync();
        }

        public async Task LogBreakGlassAction(int accessId, string action, string entityType, string entityId, bool phiAccessed)
        {
            using var context = _databaseService.GetDbContext();
            
            var audit = new BreakGlassAudit
            {
                BreakGlassAccessId = accessId,
                Action = action,
                EntityType = entityType,
                EntityId = entityId,
                PhiAccessed = phiAccessed,
                ActionTimestamp = DateTime.UtcNow,
                DisplayName = $"Break Glass Action - {action}",
                Description = $"Action performed on {entityType}:{entityId}"
            };

            context.Add(audit);
            await context.SaveChangesAsync();
        }

        public async Task<BreakGlassMetrics> GetBreakGlassMetrics(int tenantId, DateTime? fromDate = null, DateTime? toDate = null)
        {
            using var context = _databaseService.GetDbContext();
            
            fromDate ??= DateTime.UtcNow.AddDays(-30);
            toDate ??= DateTime.UtcNow;

            var accesses = await context.Set<BreakGlassAccess>()
                .Where(x => x.TenantId == tenantId && 
                           x.CreatedDateTime >= fromDate && 
                           x.CreatedDateTime <= toDate)
                .ToListAsync();

            var audits = await context.Set<BreakGlassAudit>()
                .Where(x => x.BreakGlassAccess.TenantId == tenantId &&
                           x.ActionTimestamp >= fromDate &&
                           x.ActionTimestamp <= toDate)
                .ToListAsync();

            var metrics = new BreakGlassMetrics
            {
                TotalRequests = accesses.Count,
                ApprovedRequests = accesses.Count(x => x.Status == BreakGlassStatus.Approved || x.Status == BreakGlassStatus.Active),
                RejectedRequests = accesses.Count(x => x.Status == BreakGlassStatus.Revoked),
                ActiveSessions = accesses.Count(x => (x.Status == BreakGlassStatus.Approved || x.Status == BreakGlassStatus.Active) && x.AccessEndTime > DateTime.UtcNow),
                PhiAccessEvents = audits.Count(x => x.PhiAccessed),
                RequestsByReason = accesses.GroupBy(x => x.AccessReason)
                    .ToDictionary(g => g.Key, g => g.Count()),
                AverageSessionDuration = accesses.Where(x => x.Status == BreakGlassStatus.Approved && x.ActualEndTime.HasValue)
                    .Any() ? accesses.Where(x => x.Status == BreakGlassStatus.Approved && x.ActualEndTime.HasValue)
                    .Average(x => (x.ActualEndTime.Value - x.AccessStartTime).TotalMinutes) : 0,
                GeneratedAt = DateTime.UtcNow
            };

            return metrics;
        }
    }
}