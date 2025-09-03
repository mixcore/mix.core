using Microsoft.EntityFrameworkCore;
using Mix.Database.Entities.Compliance;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Heart.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mix.Lib.Services.Compliance
{
    public class ConsentService : IConsentService
    {
        private readonly DatabaseService _databaseService;

        public ConsentService(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<ConsentEvent> RecordConsent(int tenantId, Guid userId, int purposeId, bool granted, string method, string? ipAddress, string? userAgent, string version = "1.0")
        {
            using var context = _databaseService.GetDbContext();
            
            var consentEvent = new ConsentEvent
            {
                TenantId = tenantId,
                UserId = userId,
                PurposeId = purposeId,
                Granted = granted,
                Method = method,
                IpAddress = ipAddress,
                UserAgent = userAgent,
                Version = version,
                ConsentTimestamp = DateTime.UtcNow,
                DisplayName = $"Consent for Purpose {purposeId}",
                CreatedDateTime = DateTime.UtcNow,
                LastModified = DateTime.UtcNow,
                Priority = 5
            };

            context.Add(consentEvent);
            await context.SaveChangesAsync();
            return consentEvent;
        }

        public async Task<bool> HasActiveConsent(int tenantId, Guid userId, int purposeId)
        {
            using var context = _databaseService.GetDbContext();
            
            var latestConsent = await context.Set<ConsentEvent>()
                .Where(x => x.TenantId == tenantId && x.UserId == userId && x.PurposeId == purposeId)
                .OrderByDescending(x => x.ConsentTimestamp)
                .FirstOrDefaultAsync();

            return latestConsent?.Granted == true;
        }

        public async Task<IEnumerable<ConsentEvent>> GetUserConsentHistory(int tenantId, Guid userId)
        {
            using var context = _databaseService.GetDbContext();
            
            return await context.Set<ConsentEvent>()
                .Include(x => x.Purpose)
                .Where(x => x.TenantId == tenantId && x.UserId == userId)
                .OrderByDescending(x => x.ConsentTimestamp)
                .ToListAsync();
        }

        public async Task<ConsentEvent> WithdrawConsent(int tenantId, Guid userId, int purposeId, string method, string? ipAddress)
        {
            return await RecordConsent(tenantId, userId, purposeId, false, method, ipAddress, null);
        }

        public async Task<ConsentSummary> GetConsentSummary(int tenantId, Guid userId)
        {
            using var context = _databaseService.GetDbContext();
            
            var latestConsents = await context.Set<ConsentEvent>()
                .Include(x => x.Purpose)
                .Where(x => x.TenantId == tenantId && x.UserId == userId)
                .GroupBy(x => x.PurposeId)
                .Select(g => g.OrderByDescending(x => x.ConsentTimestamp).First())
                .ToListAsync();

            return new ConsentSummary
            {
                UserId = userId,
                LastUpdated = latestConsents.Any() ? latestConsents.Max(x => x.ConsentTimestamp) : DateTime.MinValue,
                Consents = latestConsents.Select(x => new ConsentStatus
                {
                    PurposeId = x.PurposeId,
                    PurposeName = x.Purpose?.Name ?? "Unknown",
                    Granted = x.Granted,
                    ConsentDate = x.ConsentTimestamp,
                    Method = x.Method
                }).ToList()
            };
        }

        public async Task<IEnumerable<ConsentEvent>> GetExpiredConsents(int tenantId, int maxAgeDays)
        {
            using var context = _databaseService.GetDbContext();
            var cutoffDate = DateTime.UtcNow.AddDays(-maxAgeDays);
            
            return await context.Set<ConsentEvent>()
                .Where(x => x.TenantId == tenantId && x.ConsentTimestamp < cutoffDate)
                .ToListAsync();
        }

        public async Task<ConsentMetrics> GetConsentMetrics(int tenantId, DateTime? fromDate = null, DateTime? toDate = null)
        {
            using var context = _databaseService.GetDbContext();
            
            var query = context.Set<ConsentEvent>().Where(x => x.TenantId == tenantId);
            
            if (fromDate.HasValue)
                query = query.Where(x => x.ConsentTimestamp >= fromDate.Value);
            
            if (toDate.HasValue)
                query = query.Where(x => x.ConsentTimestamp <= toDate.Value);

            var consents = await query.ToListAsync();

            return new ConsentMetrics
            {
                TotalConsents = consents.Count,
                GrantedConsents = consents.Count(x => x.Granted),
                WithdrawnConsents = consents.Count(x => !x.Granted),
                ConsentsByPurpose = consents.GroupBy(x => x.PurposeId).ToDictionary(g => g.Key, g => g.Count()),
                ConsentsByMethod = consents.GroupBy(x => x.Method).ToDictionary(g => g.Key, g => g.Count()),
                GeneratedAt = DateTime.UtcNow
            };
        }
    }
}