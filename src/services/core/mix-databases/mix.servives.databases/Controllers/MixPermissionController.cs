﻿using Microsoft.AspNetCore.Mvc;
using Mix.Auth.Constants;
using Mix.Heart.Services;
using Mix.Heart.UnitOfWork;
using Mix.Identity.Dtos;
using Mix.Lib.Attributes;
using Mix.Lib.Base;
using Mix.Lib.Interfaces;
using Mix.Lib.Services;
using Mix.Mixdb.ViewModels;
using Mix.Mq.Lib.Models;
using Mix.Queue.Interfaces;
using Mix.Services.Databases.Lib.Dtos;
using Mix.Services.Databases.Lib.Interfaces;
using Mix.SignalR.Interfaces;

namespace Mix.Services.Databases.Controllers
{
    [Route("api/v2/rest/mix-services/permission")]
    public sealed class MixPermissionController :
        MixRestfulApiControllerBase<MixPermissionViewModel, MixDbDbContext, MixPermission, int>
    {
        private readonly IMixPermissionService _permissionService;
        public MixPermissionController(
            IMixPermissionService permissionService,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixCacheService cacheService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<MixDbDbContext> uow,
            IMemoryQueueService<MessageQueueModel> queueService,
            IPortalHubClientService portalHub,
            IMixTenantService mixTenantService)
            : base(httpContextAccessor, configuration,
                  cacheService, translator, mixIdentityService, uow, queueService, portalHub, mixTenantService)
        {
            _permissionService = permissionService;
            Repository.IsCache = false;
        }

        #region Routes

        [MixAuthorize(roles: MixRoles.Owner)]
        [HttpGet("get-by-user/{userId}")]
        public async Task<ActionResult<List<MixPermissionViewModel>>> GetPermissionByUser(Guid userId)
        {
            var permissions = await _permissionService.GetPermissionAsync(userId);
            return Ok(permissions);
        }

        [MixAuthorize(roles: $"{MixRoles.Owner},{MixRoles.Administrators}")]
        [HttpPost("add-user-permission")]
        public async Task<ActionResult> AddUserPermission(CreateUserPermissionDto dto)
        {
            await _permissionService.AddUserPermission(dto);
            return Ok();
        }

        [MixAuthorize]
        [HttpGet("get-my-permissions")]
        public async Task<ActionResult<List<MixPermissionViewModel>>> GetMyPermissions()
        {
            var userId = MixIdentityService.GetClaim(User, MixClaims.Id);
            if (!string.IsNullOrEmpty(userId))
            {
                var permissions = await _permissionService.GetPermissionAsync(Guid.Parse(userId));
                return Ok(permissions);
            }
            return BadRequest();
        }

        [MixAuthorize(roles: $"{MixRoles.Owner},{MixRoles.Administrators}")]
        [Route("grant-permission")]
        [HttpPost]
        public async Task<ActionResult> GrantPermission([FromBody] GrantPermissionsDto dto)
        {
            dto.RequestedBy = MixIdentityService.GetClaim(User, MixClaims.Username);
            await _permissionService.GrantPermissions(dto);
            return Ok();
        }
        #endregion

        #region Overrides

        #endregion
    }
}
