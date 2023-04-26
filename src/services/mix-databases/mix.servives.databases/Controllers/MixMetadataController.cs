using Microsoft.AspNetCore.Mvc;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Base;
using Mix.Lib.Models.Common;
using Mix.Lib.Services;
using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Mix.Service.Services;
using Mix.Services.Databases.Lib.Dtos;
using Mix.Shared.Dtos;
using Mix.Heart.Extensions;
using Mix.Mixdb.ViewModels;
using Mix.Services.Databases.Lib.Interfaces;
using Mix.Heart.Services;
using Mix.Database.Entities.Cms;
using Mix.Database.Entities.MixDb;

namespace Mix.Services.Databases.Controllers
{
    [Route("api/v2/rest/mix-services/metadata")]
    public sealed class MixMetadataController :
        MixRestfulApiControllerBase<MixMetadataViewModel, MixDbDbContext, MixMetadata, int>
    {
        private readonly IMixMetadataService _metadataService;
        public MixMetadataController(
            IMixMetadataService metadataService, 
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixCacheService cacheService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<MixDbDbContext> uow,
            IQueueService<MessageQueueModel> queueService)
            : base(httpContextAccessor, configuration, cacheService, translator, mixIdentityService, uow, queueService)
        {
            _metadataService = metadataService;
            Repository.IsCache = false;
        }

        #region Routes


        [HttpGet("get-metadata/{contentType}/{contentId}")]
        public async Task<ActionResult<List<MixMetadataContentAsscociationViewModel>>> GetMetadataByContentId([FromQuery] SearchMetadataDto req)
        {
            var searchRequest = BuildSearchRequest(req);
            var metadatas = await _metadataService.GetMetadataByContentId(req.ContentId, req.ContentType, req.MetadataType, searchRequest.PagingData);
            return Ok(metadatas);
        }

        [HttpPost("get-or-create-metadata")]
        public async Task<ActionResult> CreateMetadata([FromBody] CreateMetadataDto dto, CancellationToken cancellationToken = default)
        {
            return Ok(await _metadataService.GetOrCreateMetadata(dto, cancellationToken));
        }

        [HttpPost("create-metadata-association")]
        public async Task<ActionResult> CreateMetadataContentAssociation([FromBody] CreateMetadataContentAssociationDto dto)
        {
            await _metadataService.CreateMetadataContentAssociation(dto);
            return Ok();
        }

        [HttpDelete("delete-metadata-association/{id}")]
        public async Task<ActionResult> DeleteMetadataContentAssociation(int id, CancellationToken cancellationToken = default)
        {
            await _metadataService.DeleteMetadataContentAssociation(id, cancellationToken);
            return Ok();
        }

        #endregion

        #region Overrides

        protected override SearchQueryModel<MixMetadata, int> BuildSearchRequest(SearchRequestDto req)
        {
            var searchRequest = base.BuildSearchRequest(req);
            string type = Request.Query["metadataType"].ToString();
            searchRequest.Predicate = searchRequest.Predicate.AndAlsoIf(!string.IsNullOrEmpty(type),
                m => m.Type == type);
            return searchRequest;
        }

        #endregion
    }
}
