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
    [Route("api/v2/rest/mix-services/permission")]
    public sealed class MixPermissionController :
        MixRestfulApiControllerBase<MixPermissionViewModel, MixServiceDatabaseDbContext, MixPermission, int>
    {
        private readonly MixPermissionService _permissionService;
        public MixPermissionController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixService mixService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<MixServiceDatabaseDbContext> uow, IQueueService<MessageQueueModel> queueService, MixPermissionService permissionService)
            : base(httpContextAccessor, configuration, mixService, translator, mixIdentityService, uow, queueService)
        {
            _permissionService = permissionService;
        }

        #region Routes

        [MixAuthorize(roles: MixRoles.Owner)]
        [HttpGet("get-by-user/{userId}")]
        public async Task<ActionResult<List<MixPermissionViewModel>>> GetPermissionByUser(Guid userId)
        {
            var permissions = await _permissionService.GetPermissionAsyncs(userId);
            return Ok(permissions);
        }

        [MixAuthorize(roles: $"{MixRoles.Owner},{MixRoles.Administrators}")]
        [HttpPost("add-user-permission")]
        public async Task<ActionResult> AddUserPermission(CreateUserPermissionDto dto)
        {
            await _permissionService.AddUserPermission(dto);
            return Ok();
        }

        [HttpGet("get-my-permissions")]
        public async Task<ActionResult<List<MixPermissionViewModel>>> GetMyPermissions()
        {
            var userId = MixIdentityService.GetClaim(User, MixClaims.Id);
            if (!string.IsNullOrEmpty(userId))
            {
                var permissions = await _permissionService.GetPermissionAsyncs(Guid.Parse(userId));
                return Ok(permissions);
            }
            return BadRequest();
        }


        #endregion

        #region Overrides

        protected override SearchQueryModel<MixPermission, int> BuildSearchRequest(SearchRequestDto req)
        {
            var searchRequest = base.BuildSearchRequest(req);
            searchRequest.Predicate = searchRequest.Predicate
                .AndAlsoIf(!string.IsNullOrEmpty(req.Keyword),
                m => m.Metadata != null && EF.Functions.Like(m.Metadata.Description, $"%{req.Keyword}%"));
            return searchRequest;
        }

        #endregion
    }
}
