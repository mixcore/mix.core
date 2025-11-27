using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mix.Lib.Services.Compliance;
using Mix.Database.Entities.Compliance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Mix.Lib.Controllers.Compliance
{
    [ApiController]
    [Route("api/privacy/consent")]
    [Authorize]
    public class ConsentController : ControllerBase
    {
        private readonly IConsentService _consentService;
        private readonly ILogger<ConsentController> _logger;

        public ConsentController(IConsentService consentService, ILogger<ConsentController> logger)
        {
            _consentService = consentService;
            _logger = logger;
        }

        [HttpPost("grant")]
        public async Task<ActionResult<ConsentEvent>> GrantConsent(ConsentRequestDto request)
        {
            try
            {
                var tenantId = GetCurrentTenantId();
                var userId = GetCurrentUserId();
                var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
                var userAgent = HttpContext.Request.Headers["User-Agent"].FirstOrDefault();
                
                var consentEvent = await _consentService.RecordConsent(
                    tenantId, userId, request.PurposeId, true, "web_form", ipAddress, userAgent, request.Version);
                
                _logger.LogInformation("Consent granted by user {UserId} for purpose {PurposeId} in tenant {TenantId}", 
                    userId, request.PurposeId, tenantId);
                
                return Ok(consentEvent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error recording consent");
                return StatusCode(500, "An error occurred while recording consent");
            }
        }

        [HttpPost("withdraw")]
        public async Task<ActionResult<ConsentEvent>> WithdrawConsent(ConsentWithdrawDto request)
        {
            try
            {
                var tenantId = GetCurrentTenantId();
                var userId = GetCurrentUserId();
                var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
                
                var consentEvent = await _consentService.WithdrawConsent(
                    tenantId, userId, request.PurposeId, "web_form", ipAddress);
                
                _logger.LogInformation("Consent withdrawn by user {UserId} for purpose {PurposeId} in tenant {TenantId}", 
                    userId, request.PurposeId, tenantId);
                
                return Ok(consentEvent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error withdrawing consent");
                return StatusCode(500, "An error occurred while withdrawing consent");
            }
        }

        [HttpGet("summary")]
        public async Task<ActionResult<ConsentSummary>> GetConsentSummary()
        {
            try
            {
                var tenantId = GetCurrentTenantId();
                var userId = GetCurrentUserId();
                
                var summary = await _consentService.GetConsentSummary(tenantId, userId);
                return Ok(summary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving consent summary");
                return StatusCode(500, "An error occurred while retrieving consent summary");
            }
        }

        [HttpGet("history")]
        public async Task<ActionResult<IEnumerable<ConsentEvent>>> GetConsentHistory()
        {
            try
            {
                var tenantId = GetCurrentTenantId();
                var userId = GetCurrentUserId();
                
                var history = await _consentService.GetUserConsentHistory(tenantId, userId);
                return Ok(history);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving consent history");
                return StatusCode(500, "An error occurred while retrieving consent history");
            }
        }

        [HttpGet("check/{purposeId}")]
        public async Task<ActionResult<ConsentCheckResult>> CheckConsent(int purposeId)
        {
            try
            {
                var tenantId = GetCurrentTenantId();
                var userId = GetCurrentUserId();
                
                var hasConsent = await _consentService.HasActiveConsent(tenantId, userId, purposeId);
                
                return Ok(new ConsentCheckResult 
                { 
                    PurposeId = purposeId, 
                    HasActiveConsent = hasConsent,
                    CheckedAt = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking consent for purpose {PurposeId}", purposeId);
                return StatusCode(500, "An error occurred while checking consent");
            }
        }

        [HttpGet("metrics")]
        [Authorize(Roles = "Admin,PrivacyOfficer")]
        public async Task<ActionResult<ConsentMetrics>> GetMetrics([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
        {
            try
            {
                var tenantId = GetCurrentTenantId();
                var metrics = await _consentService.GetConsentMetrics(tenantId, fromDate, toDate);
                
                return Ok(metrics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving consent metrics");
                return StatusCode(500, "An error occurred while retrieving metrics");
            }
        }

        private int GetCurrentTenantId()
        {
            return HttpContext.Session.GetInt32("TenantId") ?? 1;
        }

        private Guid GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst("user_id")?.Value;
            return Guid.TryParse(userIdClaim, out var userId) ? userId : Guid.Empty;
        }
    }

    public class ConsentRequestDto
    {
        public int PurposeId { get; set; }
        public string Version { get; set; } = "1.0";
    }

    public class ConsentWithdrawDto
    {
        public int PurposeId { get; set; }
    }

    public class ConsentCheckResult
    {
        public int PurposeId { get; set; }
        public bool HasActiveConsent { get; set; }
        public DateTime CheckedAt { get; set; }
    }
}