using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mix.Lib.Services.Compliance;
using Mix.Database.Entities.Compliance;

namespace Mix.Lib.Controllers.Compliance
{
    [ApiController]
    [Route("api/compliance/break-glass")]
    [Authorize]
    public class BreakGlassController : ControllerBase
    {
        private readonly IBreakGlassService _breakGlassService;

        public BreakGlassController(IBreakGlassService breakGlassService)
        {
            _breakGlassService = breakGlassService;
        }

        [HttpPost("request")]
        public async Task<ActionResult<BreakGlassAccess>> RequestEmergencyAccess([FromBody] BreakGlassRequest request)
        {
            try
            {
                var tenantId = GetTenantId();
                var userId = GetUserId();
                
                var access = await _breakGlassService.RequestEmergencyAccess(
                    tenantId, userId, request.Reason, request.Justification, request.DurationMinutes);
                
                return Ok(access);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{accessId}/approve")]
        [Authorize(Roles = "Administrator,SuperAdmin")]
        public async Task<ActionResult<BreakGlassAccess>> ApproveAccess(int accessId)
        {
            try
            {
                var tenantId = GetTenantId();
                var approvedBy = GetUserName();
                
                var access = await _breakGlassService.ApproveAccess(tenantId, accessId, approvedBy);
                return Ok(access);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{accessId}/revoke")]
        [Authorize(Roles = "Administrator,SuperAdmin")]
        public async Task<ActionResult<BreakGlassAccess>> RevokeAccess(int accessId)
        {
            try
            {
                var tenantId = GetTenantId();
                var revokedBy = GetUserName();
                
                var access = await _breakGlassService.RevokeAccess(tenantId, accessId, revokedBy);
                return Ok(access);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("active")]
        [Authorize(Roles = "Administrator,SuperAdmin")]
        public async Task<ActionResult<IEnumerable<BreakGlassAccess>>> GetActiveAccesses()
        {
            var tenantId = GetTenantId();
            var accesses = await _breakGlassService.GetActiveAccesses(tenantId);
            return Ok(accesses);
        }

        [HttpGet("status")]
        public async Task<ActionResult<bool>> HasActiveAccess()
        {
            var tenantId = GetTenantId();
            var userId = GetUserId();
            var hasAccess = await _breakGlassService.HasActiveBreakGlassAccess(tenantId, userId);
            return Ok(new { hasActiveAccess = hasAccess });
        }

        [HttpPost("{accessId}/audit")]
        public async Task<ActionResult> LogAction(int accessId, [FromBody] BreakGlassAuditRequest request)
        {
            try
            {
                await _breakGlassService.LogBreakGlassAction(
                    accessId, request.Action, request.EntityType, request.EntityId, request.PhiAccessed);
                
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("metrics")]
        [Authorize(Roles = "Administrator,SuperAdmin")]
        public async Task<ActionResult<BreakGlassMetrics>> GetMetrics([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
        {
            var tenantId = GetTenantId();
            var metrics = await _breakGlassService.GetBreakGlassMetrics(tenantId, fromDate, toDate);
            return Ok(metrics);
        }

        private int GetTenantId()
        {
            // This should be extracted from the user's context or JWT token
            // For now, returning a default value - this needs to be implemented based on your authentication system
            return 1; // Replace with actual tenant resolution logic
        }

        private Guid GetUserId()
        {
            // This should be extracted from the user's context or JWT token
            // For now, returning a default value - this needs to be implemented based on your authentication system
            return Guid.NewGuid(); // Replace with actual user ID resolution logic
        }

        private string GetUserName()
        {
            // This should be extracted from the user's context or JWT token
            return User?.Identity?.Name ?? "System"; // Replace with actual user name resolution logic
        }
    }

    public class BreakGlassRequest
    {
        public string Reason { get; set; }
        public string Justification { get; set; }
        public int DurationMinutes { get; set; } = 30;
    }

    public class BreakGlassAuditRequest
    {
        public string Action { get; set; }
        public string EntityType { get; set; }
        public string EntityId { get; set; }
        public bool PhiAccessed { get; set; }
    }
}