using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mix.Database.Entities.Compliance;
using Mix.Lib.Services.Compliance;

namespace Mix.Lib.Controllers.Compliance
{
    [ApiController]
    [Route("api/compliance/classification")]
    [Authorize(Roles = "Admin,DataOfficer")]
    public class DataClassificationController : ControllerBase
    {
        private readonly IDataClassificationService _classificationService;
        private readonly ILogger<DataClassificationController> _logger;

        public DataClassificationController(IDataClassificationService classificationService, ILogger<DataClassificationController> logger)
        {
            _classificationService = classificationService;
            _logger = logger;
        }

        [HttpGet("entity/{entityName}")]
        public async Task<ActionResult<Dictionary<string, DataClassification>>> GetEntityClassifications(string entityName)
        {
            try
            {
                var tenantId = GetCurrentTenantId();
                var classifications = await _classificationService.GetEntityClassifications(tenantId, entityName);
                
                return Ok(classifications);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving classifications for entity {EntityName}", entityName);
                return StatusCode(500, "An error occurred while retrieving classifications");
            }
        }

        [HttpGet("field/{entityName}/{fieldName}")]
        public async Task<ActionResult<DataFieldMetadata>> GetFieldMetadata(string entityName, string fieldName)
        {
            try
            {
                var tenantId = GetCurrentTenantId();
                var metadata = await _classificationService.GetFieldMetadata(tenantId, entityName, fieldName);
                
                if (metadata == null)
                    return NotFound();
                
                return Ok(metadata);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving metadata for field {EntityName}.{FieldName}", entityName, fieldName);
                return StatusCode(500, "An error occurred while retrieving field metadata");
            }
        }

        [HttpPost("classify")]
        public async Task<ActionResult<DataFieldMetadata>> SetFieldClassification(ClassificationRequestDto request)
        {
            try
            {
                var tenantId = GetCurrentTenantId();
                var metadata = await _classificationService.SetFieldClassification(
                    tenantId, request.EntityName, request.FieldName, request.Classification, request.EncryptionRequired);
                
                _logger.LogInformation("Field {EntityName}.{FieldName} classified as {Classification} in tenant {TenantId}", 
                    request.EntityName, request.FieldName, request.Classification, tenantId);
                
                return Ok(metadata);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting classification for field {EntityName}.{FieldName}", request.EntityName, request.FieldName);
                return StatusCode(500, "An error occurred while setting field classification");
            }
        }

        [HttpGet("classification/{classification}")]
        public async Task<ActionResult<IEnumerable<DataFieldMetadata>>> GetEntitiesForClassification(DataClassification classification)
        {
            try
            {
                var tenantId = GetCurrentTenantId();
                var entities = await _classificationService.GetEntitiesForClassification(tenantId, classification);
                
                return Ok(entities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving entities for classification {Classification}", classification);
                return StatusCode(500, "An error occurred while retrieving entities");
            }
        }

        [HttpPost("seed")]
        public async Task<IActionResult> SeedDefaultClassifications()
        {
            try
            {
                var tenantId = GetCurrentTenantId();
                await _classificationService.SeedDefaultClassifications(tenantId);
                
                _logger.LogInformation("Default classifications seeded for tenant {TenantId}", tenantId);
                
                return Ok(new { Message = "Default classifications seeded successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error seeding default classifications");
                return StatusCode(500, "An error occurred while seeding classifications");
            }
        }

        [HttpGet("report")]
        public async Task<ActionResult<ComplianceReport>> GenerateComplianceReport()
        {
            try
            {
                var tenantId = GetCurrentTenantId();
                var report = await _classificationService.GenerateClassificationReport(tenantId);
                
                return Ok(report);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating compliance report");
                return StatusCode(500, "An error occurred while generating the compliance report");
            }
        }

        [HttpGet("check/{entityName}/{fieldName}")]
        public async Task<ActionResult<FieldClassificationStatus>> CheckFieldClassification(string entityName, string fieldName)
        {
            try
            {
                var tenantId = GetCurrentTenantId();
                var isClassified = await _classificationService.IsFieldClassified(tenantId, entityName, fieldName);
                
                return Ok(new FieldClassificationStatus 
                { 
                    EntityName = entityName,
                    FieldName = fieldName,
                    IsClassified = isClassified,
                    CheckedAt = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking classification for field {EntityName}.{FieldName}", entityName, fieldName);
                return StatusCode(500, "An error occurred while checking field classification");
            }
        }

        private int GetCurrentTenantId()
        {
            return HttpContext.Session.GetInt32("TenantId") ?? 1;
        }
    }

    public class ClassificationRequestDto
    {
        public string EntityName { get; set; }
        public string FieldName { get; set; }
        public DataClassification Classification { get; set; }
        public bool EncryptionRequired { get; set; }
    }

    public class FieldClassificationStatus
    {
        public string EntityName { get; set; }
        public string FieldName { get; set; }
        public bool IsClassified { get; set; }
        public DateTime CheckedAt { get; set; }
    }
}