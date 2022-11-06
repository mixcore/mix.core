using Microsoft.AspNetCore.Mvc;
using Mix.Heart.Entities.Cache;
using Mix.Heart.UnitOfWork;
using Mix.Identity.Constants;
using Mix.Lib.Attributes;
using Mix.Lib.Base;
using Mix.Lib.Services;
using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Mix.Service.Services;
using Mix.Services.Permission.Domain.Dtos;
using Mix.Services.Permission.Domain.Entities;
using Mix.Services.Permission.Domain.Services;
using Mix.Services.Permission.Domain.ViewModels;

namespace Mix.Services.Permission.Controllers
{
    [Route("api/v2/rest/mix-services/permission")]
    public sealed class MixPermissionController :
        MixRestfulApiControllerBase<MixPermissionViewModel, PermissionDbContext, MixPermission, int>
    {
        private readonly MixPermissionService _permissionService;
        public MixPermissionController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixService mixService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<MixCacheDbContext> cacheUOW,
            UnitOfWorkInfo<PermissionDbContext> uow, IQueueService<MessageQueueModel> queueService, MixPermissionService permissionService)
            : base(httpContextAccessor, configuration, mixService, translator, mixIdentityService, cacheUOW, uow, queueService)
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
            var userId = _mixIdentityService.GetClaim(User, MixClaims.Id);
            if (!string.IsNullOrEmpty(userId))
            {
                var permissions = await _permissionService.GetPermissionAsyncs(Guid.Parse(userId));
                return Ok(permissions);
            }
            return BadRequest();
        }


        #endregion
    }
}
