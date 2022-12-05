using Microsoft.AspNetCore.Mvc;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Base;
using Mix.Lib.Models.Common;
using Mix.Lib.Services;
using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Mix.Service.Services;
using Mix.Services.Databases.Lib.Dtos;
using Mix.Services.Databases.Lib.Entities;
using Mix.Services.Databases.Lib.Services;
using Mix.Services.Databases.Lib.ViewModels;
using Mix.Shared.Dtos;
using Mix.Services.Databases.Lib.Enums;

namespace Mix.Services.Permission.Controllers
{
    [Route("api/v2/rest/mix-services/metadata")]
    public sealed class MixMetadataController :
        MixRestfulApiControllerBase<MixMetadataViewModel, MixServiceDatabaseDbContext, MixMetadata, int>
    {
        private readonly MixMetadataService _metadataService;
        public MixMetadataController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixService mixService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<MixServiceDatabaseDbContext> uow, IQueueService<MessageQueueModel> queueService, MixMetadataService metadataService)
            : base(httpContextAccessor, configuration, mixService, translator, mixIdentityService, uow, queueService)
        {
            _metadataService = metadataService;
        }

        #region Routes

        [HttpGet("get-metadata-content/{contentType}/{metadataSeoContent}")]
        public async Task<ActionResult<List<object>>> GetContentByMetadata(
            MetadataParentType contentType, 
            string metadataSeoContent, 
            [FromQuery] SearchRequestDto req)
        {
            var searchRequest = BuildSearchRequest(req);
            var metadatas = await _metadataService.GetQueryableContentByMetadataSeoContentAsync(metadataSeoContent, contentType, searchRequest);
            return Ok(metadatas);
        }

        [HttpPost("create-metadata-association")]
        public async Task<ActionResult> CreateMetadataContentAssociation(CreateMetadataContentAssociationDto dto)
        {
            await _metadataService.CreateMetadataContentAssociation(dto);
            return Ok();
        }

        #endregion

        #region Overrides

        protected override SearchQueryModel<MixMetadata, int> BuildSearchRequest(SearchRequestDto req)
        {
            var searchRequest = base.BuildSearchRequest(req);
            return searchRequest;
        }

        #endregion
    }
}
