using Microsoft.EntityFrameworkCore;
using Mix.Database.Services.MixGlobalSettings;

namespace Mix.Lib.Services.Compliance
{
    public class RetentionService : IRetentionService
    {
        private readonly DatabaseService _databaseService;
        private readonly IDataClassificationService _dataClassificationService;

        public RetentionService(DatabaseService databaseService, IDataClassificationService dataClassificationService)
        {
            _databaseService = databaseService;
            _dataClassificationService = dataClassificationService;
        }

        public async Task<RetentionPolicy> CreateRetentionPolicy(int tenantId, string name, string category, int maxAgeDays, RetentionAction action)
        {
            using var context = _databaseService.GetDbContext();
            
            var policy = new RetentionPolicy
            {
                TenantId = tenantId,
                Name = name,
                Category = category,
                MaxAgeDays = maxAgeDays,
                ActionOnExpiry = action,
                IsActive = true,
                DisplayName = name,
                Description = $"Retention policy for {category} - {action} after {maxAgeDays} days",
                Status = Mix.Heart.Enums.MixContentStatus.Published
            };

            context.Add(policy);
            await context.SaveChangesAsync();
            return policy;
        }

        public async Task<IEnumerable<RetentionPolicy>> GetActiveRetentionPolicies(int tenantId)
        {
            using var context = _databaseService.GetDbContext();
            return await context.Set<RetentionPolicy>()
                .Where(x => x.TenantId == tenantId && x.IsActive)
                .ToListAsync();
        }

        public async Task<RetentionExecution> ExecuteRetentionPolicy(int tenantId, int policyId, string executedBy)
        {
            using var context = _databaseService.GetDbContext();
            
            var policy = await context.Set<RetentionPolicy>()
                .FirstOrDefaultAsync(x => x.TenantId == tenantId && x.Id == policyId && x.IsActive);
                
            if (policy == null)
                throw new InvalidOperationException($"Retention policy {policyId} not found or inactive for tenant {tenantId}");

            var execution = new RetentionExecution
            {
                TenantId = tenantId,
                RetentionPolicyId = policyId,
                ExecutedUtc = DateTime.UtcNow,
                ExecutedBy = executedBy,
                DisplayName = $"Retention execution for {policy.Name}",
                Status = Mix.Heart.Enums.MixContentStatus.Published
            };

            try
            {
                var expiredEntities = await GetExpiredEntities(tenantId, policy);
                var processedCount = 0;
                var errorCount = 0;
                var errors = new List<string>();

                foreach (var entity in expiredEntities)
                {
                    try
                    {
                        bool success = policy.ActionOnExpiry switch
                        {
                            RetentionAction.Delete => await DeleteEntity(GetEntityType(entity), GetEntityId(entity), tenantId),
                            RetentionAction.Pseudonymize => await AnonymizeEntity(GetEntityType(entity), GetEntityId(entity), tenantId),
                            RetentionAction.Archive => await ArchiveEntity(GetEntityType(entity), GetEntityId(entity), tenantId),
                            _ => false
                        };

                        if (success)
                            processedCount++;
                        else
                            errorCount++;
                    }
                    catch (Exception ex)
                    {
                        errorCount++;
                        errors.Add($"Entity {GetEntityId(entity)}: {ex.Message}");
                    }
                }

                execution.ProcessedCount = processedCount;
                execution.ErrorCount = errorCount;
                execution.ErrorDetails = errors.Any() ? string.Join("; ", errors) : null;
                execution.Success = errorCount == 0;
            }
            catch (Exception ex)
            {
                execution.ProcessedCount = 0;
                execution.ErrorCount = 1;
                execution.ErrorDetails = ex.Message;
                execution.Success = false;
            }

            context.Add(execution);
            await context.SaveChangesAsync();
            return execution;
        }

        public async Task<IEnumerable<object>> GetExpiredEntities(int tenantId, RetentionPolicy policy)
        {
            using var context = _databaseService.GetDbContext();
            var cutoffDate = DateTime.UtcNow.AddDays(-policy.MaxAgeDays);
            var entities = new List<object>();

            // This is a simplified implementation - in practice, you'd need to:
            // 1. Query each entity type based on data classification metadata
            // 2. Apply the retention policy rules
            // 3. Consider legal holds and other exceptions
            
            // Example: Get expired audit logs based on category
            if (policy.Category.Equals("AuditLog", StringComparison.OrdinalIgnoreCase))
            {
                var expiredLogs = await context.Set<Mix.Database.Entities.AuditLog.AuditLog>()
                    .Where(x => x.TenantId == tenantId && x.CreatedDateTime < cutoffDate)
                    .ToListAsync();
                entities.AddRange(expiredLogs);
            }

            // Example: Get expired consent events
            if (policy.Category.Equals("ConsentEvent", StringComparison.OrdinalIgnoreCase))
            {
                var expiredConsents = await context.Set<ConsentEvent>()
                    .Where(x => x.TenantId == tenantId && x.ConsentTimestamp < cutoffDate)
                    .ToListAsync();
                entities.AddRange(expiredConsents);
            }

            return entities;
        }

        public async Task<bool> AnonymizeEntity(string entityType, object entityId, int tenantId)
        {
            using var context = _databaseService.GetDbContext();

            try
            {
                // This is a basic implementation - in practice, you'd need sophisticated
                // anonymization based on data classification metadata
                
                if (entityType == "AuditLog" && Guid.TryParse(entityId.ToString(), out var auditId))
                {
                    var auditLog = await context.Set<Mix.Database.Entities.AuditLog.AuditLog>()
                        .FirstOrDefaultAsync(x => x.Id == auditId && x.TenantId == tenantId);
                        
                    if (auditLog != null)
                    {
                        // Anonymize personal data fields
                        auditLog.RequestIp = AnonymizeIpAddress(auditLog.RequestIp);
                        auditLog.UserAgent = "ANONYMIZED";
                        auditLog.CreatedBy = "ANONYMIZED_USER";
                        
                        await context.SaveChangesAsync();
                        return true;
                    }
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteEntity(string entityType, object entityId, int tenantId)
        {
            using var context = _databaseService.GetDbContext();

            try
            {
                if (entityType == "ConsentEvent" && int.TryParse(entityId.ToString(), out var consentId))
                {
                    var consent = await context.Set<ConsentEvent>()
                        .FirstOrDefaultAsync(x => x.Id == consentId && x.TenantId == tenantId);
                        
                    if (consent != null)
                    {
                        context.Remove(consent);
                        await context.SaveChangesAsync();
                        return true;
                    }
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        private async Task<bool> ArchiveEntity(string entityType, object entityId, int tenantId)
        {
            // Archive implementation would move entities to archive storage
            // This is a placeholder for the archive functionality
            await Task.Delay(1); // Placeholder
            return true;
        }

        public async Task<RetentionMetrics> GetRetentionMetrics(int tenantId, DateTime? fromDate = null, DateTime? toDate = null)
        {
            using var context = _databaseService.GetDbContext();
            
            var policies = await context.Set<RetentionPolicy>()
                .Where(x => x.TenantId == tenantId)
                .ToListAsync();

            var executionsQuery = context.Set<RetentionExecution>()
                .Where(x => x.TenantId == tenantId);
                
            if (fromDate.HasValue)
                executionsQuery = executionsQuery.Where(x => x.ExecutedUtc >= fromDate.Value);
                
            if (toDate.HasValue)
                executionsQuery = executionsQuery.Where(x => x.ExecutedUtc <= toDate.Value);

            var executions = await executionsQuery.ToListAsync();

            return new RetentionMetrics
            {
                ActivePolicies = policies.Count(x => x.IsActive),
                TotalExecutions = executions.Count,
                SuccessfulExecutions = executions.Count(x => x.Success),
                FailedExecutions = executions.Count(x => !x.Success),
                ProcessedEntities = executions.Sum(x => x.ProcessedCount),
                DeletedEntities = executions.Where(x => x.RetentionPolicy?.ActionOnExpiry == RetentionAction.Delete).Sum(x => x.ProcessedCount),
                AnonymizedEntities = executions.Where(x => x.RetentionPolicy?.ActionOnExpiry == RetentionAction.Pseudonymize).Sum(x => x.ProcessedCount),
                LastExecution = executions.Any() ? executions.Max(x => x.ExecutedUtc) : DateTime.MinValue,
                GeneratedAt = DateTime.UtcNow
            };
        }

        public async Task ScheduleRetentionJobs()
        {
            // This would integrate with a job scheduler like Quartz.NET or Hangfire
            // to automatically execute retention policies on a schedule
            await Task.CompletedTask;
        }

        private string GetEntityType(object entity)
        {
            return entity.GetType().Name;
        }

        private object GetEntityId(object entity)
        {
            var idProperty = entity.GetType().GetProperty("Id");
            return idProperty?.GetValue(entity);
        }

        private string AnonymizeIpAddress(string ipAddress)
        {
            if (string.IsNullOrEmpty(ipAddress)) return ipAddress;
            
            // Basic IP anonymization - zero out last octet for IPv4
            var parts = ipAddress.Split('.');
            if (parts.Length == 4)
            {
                return $"{parts[0]}.{parts[1]}.{parts[2]}.0";
            }
            
            // For IPv6 or other formats, just return a generic anonymized value
            return "ANONYMIZED_IP";
        }
    }
}