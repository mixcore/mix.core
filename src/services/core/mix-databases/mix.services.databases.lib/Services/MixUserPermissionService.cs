using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Mix.Auth.Constants;
using Mix.Constant.Constants;
using Mix.Database.Entities.Account;
using Mix.Heart.Enums;
using Mix.Heart.Exceptions;
using Mix.Heart.Helpers;
using Mix.Heart.Services;
using Mix.Heart.UnitOfWork;
using Mix.Identity.Dtos;
using Mix.Lib.Interfaces;
using Mix.Lib.Services;
using Mix.Mixdb.ViewModels;
using Mix.Service.Services;
using Mix.Services.Databases.Lib.Dtos;
using Mix.Services.Databases.Lib.Interfaces;
using System.Linq.Expressions;

namespace Mix.Services.Databases.Lib.Services
{
    public sealed class MixUserPermissionService : TenantServiceBase, IMixPermissionService
    {
        private readonly MixIdentityService _identityService;
        private readonly MixDbDbContext _permissionDbContext;
        private readonly UnitOfWorkInfo<MixCmsAccountContext> _accUow;
        private readonly UnitOfWorkInfo<MixDbDbContext> _mixUow;
        private readonly IQueryable<SysMixDatabaseAssociation> _permissionQuery;
        public MixUserPermissionService(
            IHttpContextAccessor httpContextAccessor,
            UnitOfWorkInfo<MixCmsAccountContext> accUow,
            MixIdentityService identityService,
            MixCacheService cacheService,
            IMixTenantService mixTenantService,
            UnitOfWorkInfo<MixDbDbContext> mixUow)
            : base(httpContextAccessor, cacheService, mixTenantService)
        {
            _accUow = accUow;
            _mixUow = mixUow;
            _permissionDbContext = mixUow.DbContext;
            _identityService = identityService;
            _permissionQuery = _accUow.DbContext.SysMixDatabaseAssociation
                                .Where(m => m.ParentDatabaseName == MixDatabaseNames.ROLE
                                            && m.ChildDatabaseName == MixDatabaseNames.SYSTEM_PERMISSION);
        }

        public async Task<List<Permission>> GetPermissionByRoleId(Guid roleId)
        {
            var permissionIds = _permissionQuery.Where(m => m.GuidParentId == roleId).Select(m => m.ChildId);
            return await _accUow.DbContext.Permission.Where(m => permissionIds.Contains(m.Id))
                                    .ToListAsync();
        }

        public async Task GrantPermissions(GrantPermissionsDto dto)
        {
            foreach (var item in dto.Permissions)
            {
                if (dto.IsActive && _permissionQuery.Any(m => m.GuidParentId == item.RoleId && m.ChildId == item.PermissionId))
                {
                    continue;
                }

                try
                {
                    if (dto.IsActive)
                    {
                        _accUow.DbContext.SysMixDatabaseAssociation.Add(new SysMixDatabaseAssociation()
                        {
                            ParentDatabaseName = MixDatabaseNames.ROLE,
                            ChildDatabaseName = MixDatabaseNames.SYSTEM_PERMISSION,
                            GuidParentId = item.RoleId,
                            ChildId = item.PermissionId,
                            CreatedBy = dto.RequestedBy,
                            CreatedDateTime = DateTime.UtcNow
                        });
                    }
                    else
                    {
                        var rel = _permissionQuery.FirstOrDefault(m => m.GuidParentId == item.RoleId && m.ChildId == item.PermissionId);
                        if (rel != null)
                        {
                            _accUow.DbContext.SysMixDatabaseAssociation.Remove(rel);
                        }
                    }
                    await _accUow.DbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw new MixException(MixErrorStatus.ServerError, ex);
                }
            }
        }
        public async Task<List<MixPermissionViewModel>> GetPermissionAsync(Guid userId)
        {
            var permissions = _permissionDbContext.UserPermission.Where(m => m.MixTenantId == CurrentTenant.Id && m.UserId == userId);
            Expression<Func<MixPermission, bool>> predicate = m => m.MixTenantId == CurrentTenant.Id && permissions.Any(p => p.PermissionId == m.Id);
            var result = await MixPermissionViewModel.GetRepository(_accUow, CacheService).GetAllAsync(predicate);
            return result;
        }

        public async Task AddUserPermission(CreateUserPermissionDto dto)
        {
            if (!await _identityService.Any(dto.UserId))
            {
                throw new MixException(Heart.Enums.MixErrorStatus.Badrequest, "User not exist");
            }

            MixUserPermissionViewModel userPermission = new(_accUow)
            {
                MixTenantId = CurrentTenant.Id,
                CreatedBy = _identityService.GetClaim(HttpContextAccessor.HttpContext!.User, MixClaims.Username)
            };
            ReflectionHelper.Map(dto, userPermission);
            await userPermission.SaveAsync();
        }
    }
}