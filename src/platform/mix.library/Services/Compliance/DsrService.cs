using Microsoft.EntityFrameworkCore;
using Mix.Database.Entities.Compliance;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Heart.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Lib.Services.Compliance
{
    public class DsrService : IDsrService
    {
        private readonly DatabaseService _databaseService;
        private const int DEFAULT_SLA_DAYS = 30; // GDPR requires response within 30 days

        public DsrService(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<DsrRequest> SubmitRequest(int tenantId, Guid userId, DsrRequestType requestType, string? notes = null)
        {
            using var context = _databaseService.GetDbContext();
            
            var request = new DsrRequest
            {
                TenantId = tenantId,
                UserId = userId,
                RequestType = requestType,
                Status = DsrRequestStatus.Pending,
                SubmittedUtc = DateTime.UtcNow,
                DueUtc = DateTime.UtcNow.AddDays(DEFAULT_SLA_DAYS),
                Notes = notes,
                DisplayName = $"DSR {requestType} Request",
                CreatedDateTime = DateTime.UtcNow,
                LastModified = DateTime.UtcNow,
                Priority = 5
            };

            context.Add(request);
            await context.SaveChangesAsync();
            return request;
        }

        public async Task<DsrRequest?> GetRequest(int tenantId, int requestId)
        {
            using var context = _databaseService.GetDbContext();
            return await context.Set<DsrRequest>()
                .FirstOrDefaultAsync(x => x.TenantId == tenantId && x.Id == requestId);
        }

        public async Task<IEnumerable<DsrRequest>> GetUserRequests(int tenantId, Guid userId)
        {
            using var context = _databaseService.GetDbContext();
            return await context.Set<DsrRequest>()
                .Where(x => x.TenantId == tenantId && x.UserId == userId)
                .OrderByDescending(x => x.SubmittedUtc)
                .ToListAsync();
        }

        public async Task<IEnumerable<DsrRequest>> GetOverdueRequests(int tenantId)
        {
            using var context = _databaseService.GetDbContext();
            var now = DateTime.UtcNow;
            
            return await context.Set<DsrRequest>()
                .Where(x => x.TenantId == tenantId 
                    && x.Status != DsrRequestStatus.Completed 
                    && x.Status != DsrRequestStatus.Rejected
                    && x.DueUtc < now)
                .ToListAsync();
        }

        public async Task<DsrRequest> ProcessRequest(int tenantId, int requestId, string processedBy)
        {
            using var context = _databaseService.GetDbContext();
            
            var request = await context.Set<DsrRequest>()
                .FirstOrDefaultAsync(x => x.TenantId == tenantId && x.Id == requestId);
                
            if (request == null)
                throw new InvalidOperationException($"DSR request {requestId} not found for tenant {tenantId}");

            request.Status = DsrRequestStatus.InProgress;
            request.ProcessedBy = processedBy;
            request.LastModified = DateTime.UtcNow;
            
            await context.SaveChangesAsync();
            return request;
        }

        public async Task<DsrRequest> CompleteRequest(int tenantId, int requestId, string processedBy, string? exportFilePath = null)
        {
            using var context = _databaseService.GetDbContext();
            
            var request = await context.Set<DsrRequest>()
                .FirstOrDefaultAsync(x => x.TenantId == tenantId && x.Id == requestId);
                
            if (request == null)
                throw new InvalidOperationException($"DSR request {requestId} not found for tenant {tenantId}");

            request.Status = DsrRequestStatus.Completed;
            request.ProcessedBy = processedBy;
            request.ProcessedUtc = DateTime.UtcNow;
            request.ExportFilePath = exportFilePath;
            request.SlaMetricMet = request.ProcessedUtc <= request.DueUtc;
            request.LastModified = DateTime.UtcNow;
            
            await context.SaveChangesAsync();
            return request;
        }

        public async Task<DsrRequest> RejectRequest(int tenantId, int requestId, string processedBy, string rejectionReason)
        {
            using var context = _databaseService.GetDbContext();
            
            var request = await context.Set<DsrRequest>()
                .FirstOrDefaultAsync(x => x.TenantId == tenantId && x.Id == requestId);
                
            if (request == null)
                throw new InvalidOperationException($"DSR request {requestId} not found for tenant {tenantId}");

            request.Status = DsrRequestStatus.Rejected;
            request.ProcessedBy = processedBy;
            request.ProcessedUtc = DateTime.UtcNow;
            request.Notes = $"{request.Notes}\n\nRejection Reason: {rejectionReason}";
            request.SlaMetricMet = request.ProcessedUtc <= request.DueUtc;
            request.LastModified = DateTime.UtcNow;
            
            await context.SaveChangesAsync();
            return request;
        }

        public async Task<string> ExportUserData(int tenantId, Guid userId)
        {
            using var context = _databaseService.GetDbContext();
            
            var export = new Dictionary<string, object>();
            
            // Get user basic info
            export["RequestTimestamp"] = DateTime.UtcNow;
            export["TenantId"] = tenantId;
            export["UserId"] = userId;
            
            // Get audit logs for this user (only if audit log table exists)
            try
            {
                var auditLogsQuery = context.Database.SqlQuery<object>($@"
                    SELECT Id, CreatedDateTime, Endpoint, Method, RequestIp, StatusCode 
                    FROM mix_audit_log 
                    WHERE TenantId = {tenantId} AND CreatedBy = {userId}
                    ORDER BY CreatedDateTime DESC");
                
                var auditLogs = await auditLogsQuery.ToListAsync();
                export["AuditTrail"] = auditLogs;
            }
            catch (Exception)
            {
                // Audit log table may not exist
                export["AuditTrail"] = new List<object>();
            }
            
            // Get consent history
            try
            {
                var consents = await context.Set<ConsentEvent>()
                    .Include(x => x.Purpose)
                    .Where(x => x.TenantId == tenantId && x.UserId == userId)
                    .OrderByDescending(x => x.ConsentTimestamp)
                    .Select(x => new {
                        x.Id,
                        PurposeName = x.Purpose!.Name,
                        x.Granted,
                        x.ConsentTimestamp,
                        x.Method,
                        x.Version
                    })
                    .ToListAsync();
                
                export["ConsentHistory"] = consents;
            }
            catch (Exception)
            {
                // ConsentEvent table may not exist
                export["ConsentHistory"] = new List<object>();
            }
            
            // Get DSR history for this user
            var dsrRequests = await context.Set<DsrRequest>()
                .Where(x => x.TenantId == tenantId && x.UserId == userId)
                .OrderByDescending(x => x.SubmittedUtc)
                .Select(x => new {
                    x.Id,
                    x.RequestType,
                    x.Status,
                    x.SubmittedUtc,
                    x.ProcessedUtc,
                    x.Notes
                })
                .ToListAsync();
            
            export["DataSubjectRequests"] = dsrRequests;
            
            // Export should include schema information for transparency
            export["DataSchema"] = new {
                Version = "1.0",
                Format = "JSON",
                GeneratedAt = DateTime.UtcNow,
                Description = "Complete export of personal data as per GDPR Article 20"
            };
            
            var json = JsonConvert.SerializeObject(export, Formatting.Indented);
            
            // In a real implementation, you would save this to a secure file system
            // and return the file path
            var fileName = $"user_data_export_{userId}_{DateTime.UtcNow:yyyyMMdd_HHmmss}.json";
            var exportDir = Path.Combine(Path.GetTempPath(), "exports");
            Directory.CreateDirectory(exportDir);
            var filePath = Path.Combine(exportDir, fileName);
            
            await File.WriteAllTextAsync(filePath, json, Encoding.UTF8);
            
            return filePath;
        }

        public async Task<bool> EraseUserData(int tenantId, Guid userId)
        {
            using var context = _databaseService.GetDbContext();
            
            try
            {
                // This is a basic implementation - in reality, you'd need to:
                // 1. Identify all entities containing user data based on data classification
                // 2. Either delete records or anonymize them based on retention policies
                // 3. Maintain referential integrity
                // 4. Keep audit trail of the erasure process
                
                // Example: Anonymize audit logs instead of deleting them (if table exists)
                try
                {
                    await context.Database.ExecuteSqlRawAsync(@"
                        UPDATE mix_audit_log 
                        SET CreatedBy = 'ANONYMIZED_USER',
                            RequestIp = 'ANONYMIZED',
                            UserAgent = 'ANONYMIZED'
                        WHERE TenantId = {0} AND CreatedBy = {1}", tenantId, userId.ToString());
                }
                catch (Exception)
                {
                    // Audit log table may not exist
                }
                
                // Delete consent events (they are no longer relevant)
                try
                {
                    var consents = await context.Set<ConsentEvent>()
                        .Where(x => x.TenantId == tenantId && x.UserId == userId)
                        .ToListAsync();
                    
                    context.RemoveRange(consents);
                }
                catch (Exception)
                {
                    // ConsentEvent table may not exist
                }
                
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<DsrMetrics> GetSlaMetrics(int tenantId, DateTime? fromDate = null, DateTime? toDate = null)
        {
            using var context = _databaseService.GetDbContext();
            
            var query = context.Set<DsrRequest>().Where(x => x.TenantId == tenantId);
            
            if (fromDate.HasValue)
                query = query.Where(x => x.SubmittedUtc >= fromDate.Value);
            
            if (toDate.HasValue)
                query = query.Where(x => x.SubmittedUtc <= toDate.Value);

            var requests = await query.ToListAsync();
            var now = DateTime.UtcNow;
            
            var completed = requests.Where(x => x.Status == DsrRequestStatus.Completed).ToList();
            var avgProcessingDays = completed.Any() 
                ? completed.Average(x => (x.ProcessedUtc!.Value - x.SubmittedUtc).TotalDays) 
                : 0;

            return new DsrMetrics
            {
                TotalRequests = requests.Count,
                CompletedRequests = completed.Count,
                OverdueRequests = requests.Count(x => x.Status != DsrRequestStatus.Completed 
                    && x.Status != DsrRequestStatus.Rejected && x.DueUtc < now),
                SlaBreaches = requests.Count(x => !x.SlaMetricMet && x.ProcessedUtc.HasValue),
                AverageProcessingDays = avgProcessingDays,
                RequestsByType = requests.GroupBy(x => x.RequestType).ToDictionary(g => g.Key, g => g.Count()),
                RequestsByStatus = requests.GroupBy(x => x.Status).ToDictionary(g => g.Key, g => g.Count()),
                GeneratedAt = DateTime.UtcNow
            };
        }
    }
}