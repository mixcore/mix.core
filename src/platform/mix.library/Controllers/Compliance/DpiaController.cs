using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mix.Lib.Services.Compliance;
using Mix.Database.Entities.Compliance;

namespace Mix.Lib.Controllers.Compliance
{
    [ApiController]
    [Route("api/compliance/dpia")]
    [Authorize]
    public class DpiaController : ControllerBase
    {
        private readonly IDpiaService _dpiaService;

        public DpiaController(IDpiaService dpiaService)
        {
            _dpiaService = dpiaService;
        }

        [HttpPost]
        [Authorize(Roles = "Administrator,DataProtectionOfficer")]
        public async Task<ActionResult<DataProtectionImpactAssessment>> CreateAssessment([FromBody] DataProtectionImpactAssessment assessment)
        {
            try
            {
                var tenantId = GetTenantId();
                var result = await _dpiaService.CreateAssessment(tenantId, assessment);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{assessmentId}")]
        [Authorize(Roles = "Administrator,DataProtectionOfficer")]
        public async Task<ActionResult<DataProtectionImpactAssessment>> UpdateAssessment(int assessmentId, [FromBody] DataProtectionImpactAssessment assessment)
        {
            try
            {
                var tenantId = GetTenantId();
                var result = await _dpiaService.UpdateAssessment(tenantId, assessmentId, assessment);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{assessmentId}")]
        public async Task<ActionResult<DataProtectionImpactAssessment>> GetAssessment(int assessmentId)
        {
            try
            {
                var tenantId = GetTenantId();
                var assessment = await _dpiaService.GetAssessment(tenantId, assessmentId);
                
                if (assessment == null)
                    return NotFound();
                
                return Ok(assessment);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DataProtectionImpactAssessment>>> GetAssessments([FromQuery] string status = null)
        {
            var tenantId = GetTenantId();
            var assessments = await _dpiaService.GetAssessments(tenantId, status);
            return Ok(assessments);
        }

        [HttpPost("{assessmentId}/approve")]
        [Authorize(Roles = "Administrator,DataProtectionOfficer")]
        public async Task<ActionResult<DataProtectionImpactAssessment>> ApproveAssessment(int assessmentId)
        {
            try
            {
                var tenantId = GetTenantId();
                var approvedBy = GetUserName();
                
                var assessment = await _dpiaService.ApproveAssessment(tenantId, assessmentId, approvedBy);
                return Ok(assessment);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{assessmentId}/reject")]
        [Authorize(Roles = "Administrator,DataProtectionOfficer")]
        public async Task<ActionResult<DataProtectionImpactAssessment>> RejectAssessment(int assessmentId, [FromBody] RejectAssessmentRequest request)
        {
            try
            {
                var tenantId = GetTenantId();
                var rejectedBy = GetUserName();
                
                var assessment = await _dpiaService.RejectAssessment(tenantId, assessmentId, rejectedBy, request.RejectionReason);
                return Ok(assessment);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{assessmentId}/review")]
        [Authorize(Roles = "Administrator,DataProtectionOfficer")]
        public async Task<ActionResult<DataProtectionImpactAssessment>> ReviewAssessment(int assessmentId, [FromBody] ReviewAssessmentRequest request)
        {
            try
            {
                var tenantId = GetTenantId();
                var assessment = await _dpiaService.ReviewAssessment(tenantId, assessmentId, request.ReviewComments);
                return Ok(assessment);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{assessmentId}/risks")]
        [Authorize(Roles = "Administrator,DataProtectionOfficer")]
        public async Task<ActionResult> AddRisk(int assessmentId, [FromBody] DpiaRisk risk)
        {
            try
            {
                var tenantId = GetTenantId();
                await _dpiaService.AddRisk(tenantId, assessmentId, risk);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("risks/{riskId}/mitigations")]
        [Authorize(Roles = "Administrator,DataProtectionOfficer")]
        public async Task<ActionResult> AddMitigation(int riskId, [FromBody] DpiaMitigation mitigation)
        {
            try
            {
                var tenantId = GetTenantId();
                await _dpiaService.AddMitigation(tenantId, riskId, mitigation);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("check-required")]
        public async Task<ActionResult<bool>> CheckIfDpiaRequired([FromBody] DpiaRequirementRequest request)
        {
            var required = await _dpiaService.RequiresDpia(request.DataTypes, request.ProcessingType, request.RiskScore);
            return Ok(new { dpiaRequired = required });
        }

        [HttpGet("report")]
        [Authorize(Roles = "Administrator,DataProtectionOfficer")]
        public async Task<ActionResult<DpiaReport>> GenerateReport([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
        {
            var tenantId = GetTenantId();
            var report = await _dpiaService.GenerateComplianceReport(tenantId, fromDate, toDate);
            return Ok(report);
        }

        [HttpGet("overdue")]
        [Authorize(Roles = "Administrator,DataProtectionOfficer")]
        public async Task<ActionResult<IEnumerable<DataProtectionImpactAssessment>>> GetOverdueAssessments()
        {
            var tenantId = GetTenantId();
            var overdue = await _dpiaService.GetOverdueAssessments(tenantId);
            return Ok(overdue);
        }

        private int GetTenantId()
        {
            // This should be extracted from the user's context or JWT token
            // For now, returning a default value - this needs to be implemented based on your authentication system
            return 1; // Replace with actual tenant resolution logic
        }

        private string GetUserName()
        {
            // This should be extracted from the user's context or JWT token
            return User?.Identity?.Name ?? "System"; // Replace with actual user name resolution logic
        }
    }

    public class RejectAssessmentRequest
    {
        public string RejectionReason { get; set; }
    }

    public class ReviewAssessmentRequest
    {
        public string ReviewComments { get; set; }
    }

    public class DpiaRequirementRequest
    {
        public string DataTypes { get; set; }
        public string ProcessingType { get; set; }
        public int RiskScore { get; set; }
    }
}