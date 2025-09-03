using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mix.Lib.Services.Compliance;
using Mix.Database.Entities.Compliance;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Mix.Lib.Controllers.Compliance
{
    [ApiController]
    [Route("api/privacy/dsr")]
    [Authorize]
    public class DsrController : ControllerBase
    {
        private readonly IDsrService _dsrService;
        private readonly ILogger<DsrController> _logger;

        public DsrController(IDsrService dsrService, ILogger<DsrController> logger)
        {
            _dsrService = dsrService;
            _logger = logger;
        }

        [HttpPost("request")]
        public async Task<ActionResult<DsrRequest>> SubmitRequest(DsrRequestDto request)
        {
            try
            {
                var tenantId = GetCurrentTenantId();
                var userId = GetCurrentUserId();
                
                var dsrRequest = await _dsrService.SubmitRequest(
                    tenantId, userId, request.RequestType, request.Notes);
                
                _logger.LogInformation("DSR request {RequestId} submitted by user {UserId} for tenant {TenantId}", 
                    dsrRequest.Id, userId, tenantId);
                
                return Ok(dsrRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting DSR request");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        [HttpGet("requests")]
        public async Task<ActionResult<IEnumerable<DsrRequest>>> GetUserRequests()
        {
            try
            {
                var tenantId = GetCurrentTenantId();
                var userId = GetCurrentUserId();
                
                var requests = await _dsrService.GetUserRequests(tenantId, userId);
                return Ok(requests);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving DSR requests");
                return StatusCode(500, "An error occurred while retrieving your requests");
            }
        }

        [HttpGet("requests/{requestId}")]
        public async Task<ActionResult<DsrRequest>> GetRequest(int requestId)
        {
            try
            {
                var tenantId = GetCurrentTenantId();
                var request = await _dsrService.GetRequest(tenantId, requestId);
                
                if (request == null)
                    return NotFound();
                
                // Only allow users to see their own requests (or admins)
                var userId = GetCurrentUserId();
                if (request.UserId != userId && !User.IsInRole("Admin"))
                    return Forbid();
                
                return Ok(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving DSR request {RequestId}", requestId);
                return StatusCode(500, "An error occurred while retrieving the request");
            }
        }

        [HttpPost("requests/{requestId}/process")]
        [Authorize(Roles = "Admin,PrivacyOfficer")]
        public async Task<ActionResult<DsrRequest>> ProcessRequest(int requestId)
        {
            try
            {
                var tenantId = GetCurrentTenantId();
                var processedBy = User.Identity?.Name ?? "Unknown";
                
                var request = await _dsrService.ProcessRequest(tenantId, requestId, processedBy);
                
                _logger.LogInformation("DSR request {RequestId} processed by {ProcessedBy}", requestId, processedBy);
                
                return Ok(request);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing DSR request {RequestId}", requestId);
                return StatusCode(500, "An error occurred while processing the request");
            }
        }

        [HttpPost("requests/{requestId}/complete")]
        [Authorize(Roles = "Admin,PrivacyOfficer")]
        public async Task<ActionResult<DsrRequest>> CompleteRequest(int requestId, [FromBody] CompleteRequestDto dto)
        {
            try
            {
                var tenantId = GetCurrentTenantId();
                var processedBy = User.Identity?.Name ?? "Unknown";
                
                var request = await _dsrService.CompleteRequest(tenantId, requestId, processedBy, dto.ExportFilePath);
                
                _logger.LogInformation("DSR request {RequestId} completed by {ProcessedBy}", requestId, processedBy);
                
                return Ok(request);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error completing DSR request {RequestId}", requestId);
                return StatusCode(500, "An error occurred while completing the request");
            }
        }

        [HttpPost("export")]
        public async Task<IActionResult> ExportUserData()
        {
            try
            {
                var tenantId = GetCurrentTenantId();
                var userId = GetCurrentUserId();
                
                var filePath = await _dsrService.ExportUserData(tenantId, userId);
                
                _logger.LogInformation("User data exported for user {UserId} in tenant {TenantId}", userId, tenantId);
                
                // In production, you'd want to return a download URL or stream the file
                return Ok(new { FilePath = filePath, Message = "Export completed successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting user data");
                return StatusCode(500, "An error occurred while exporting your data");
            }
        }

        [HttpGet("metrics")]
        [Authorize(Roles = "Admin,PrivacyOfficer")]
        public async Task<ActionResult<DsrMetrics>> GetMetrics([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
        {
            try
            {
                var tenantId = GetCurrentTenantId();
                var metrics = await _dsrService.GetSlaMetrics(tenantId, fromDate, toDate);
                
                return Ok(metrics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving DSR metrics");
                return StatusCode(500, "An error occurred while retrieving metrics");
            }
        }

        private int GetCurrentTenantId()
        {
            // This should be implemented based on your tenant resolution strategy
            // For example, from session, claims, or header
            return HttpContext.Session.GetInt32("TenantId") ?? 1;
        }

        private Guid GetCurrentUserId()
        {
            // This should be implemented based on your authentication strategy
            var userIdClaim = User.FindFirst("user_id")?.Value;
            return Guid.TryParse(userIdClaim, out var userId) ? userId : Guid.Empty;
        }
    }

    public class DsrRequestDto
    {
        public DsrRequestType RequestType { get; set; }
        public string? Notes { get; set; }
    }

    public class CompleteRequestDto
    {
        public string? ExportFilePath { get; set; }
    }
}