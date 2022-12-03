using Microsoft.AspNetCore.Mvc;
using Mix.Heart.UnitOfWork;
using Mix.Identity.Constants;
using Mix.Lib.Attributes;
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
using Mix.Heart.Extensions;
using Microsoft.EntityFrameworkCore;

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

        [MixAuthorize(roles: MixRoles.Owner)]
        [HttpGet("get-by-user/{userId}")]
        public async Task<ActionResult<List<MixMetadataViewModel>>> GetPermissionByUser(Guid userId)
        {
            var metadatas = await _metadataService.GetPermissionAsyncs(userId);
            return Ok(metadatas);
        }

        [MixAuthorize(roles: $"{MixRoles.Owner},{MixRoles.Administrators}")]
        [HttpPost("add-user-metadata")]
        public async Task<ActionResult> AddUserPermission(CreateMetadataLinkDto dto)
        {
            await _metadataService.AddMetadataLink(dto);
            return Ok();
        }

        [HttpGet("get-my-metadatas")]
        public async Task<ActionResult<List<MixMetadataViewModel>>> GetMyPermissions()
        {
            var userId = MixIdentityService.GetClaim(User, MixClaims.Id);
            if (!string.IsNullOrEmpty(userId))
            {
                var metadatas = await _metadataService.GetPermissionAsyncs(Guid.Parse(userId));
                return Ok(metadatas);
            }
            return BadRequest();
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
