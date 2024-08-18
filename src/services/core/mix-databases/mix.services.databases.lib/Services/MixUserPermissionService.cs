using Microsoft.AspNetCore.Http;
using Mix.Mixdb.ViewModels;
using Mix.Heart.Exceptions;
using Mix.Heart.Helpers;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Services;
using Mix.Services.Databases.Lib.Dtos;
using System.Linq.Expressions;
using Mix.Service.Services;
using Mix.Services.Databases.Lib.Interfaces;
using Mix.Heart.Services;
using Mix.Lib.Interfaces;
using Mix.Auth.Constants;
using Mix.Constant.Constants;
using Mix.Database.Entities.Account;
using Mix.Heart.Enums;
using Mix.Identity.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Mix.Services.Databases.Lib.Services
{
    public sealed class MixUserPermissionService : TenantServiceBase, IMixPermissionService
    {
        private readonly MixIdentityService _identityService;
        private readonly MixDbDbContext _permissionDbContext;
        private readonly UnitOfWorkInfo<MixDbDbContext> _uow;

        public MixUserPermissionService(
            IHttpContextAccessor httpContextAccessor,
            UnitOfWorkInfo<MixDbDbContext> uow,
            MixIdentityService identityService,
            MixCacheService cacheService,
            IMixTenantService mixTenantService)
            : base(httpContextAccessor, cacheService, mixTenantService)
        {
            _uow = uow;
            _permissionDbContext = _uow.DbContext;
            _identityService = identityService;
        }

        public async Task<List<MixPermissionViewModel>> GetPermissionAsync(Guid userId)
        {
            var permissions = _permissionDbContext.UserPermission.Where(m => m.MixTenantId == CurrentTenant.Id && m.UserId == userId);
            Expression<Func<MixPermission, bool>> predicate = m => m.MixTenantId == CurrentTenant.Id && permissions.Any(p => p.PermissionId == m.Id);
            var result = await MixPermissionViewModel.GetRepository(_uow, CacheService).GetAllAsync(predicate);
            return result;
        }

        public async Task AddUserPermission(CreateUserPermissionDto dto)
        {
            if (!await _identityService.Any(dto.UserId))
            {
                throw new MixException(Heart.Enums.MixErrorStatus.Badrequest, "User not exist");
            }

            MixUserPermissionViewModel userPermission = new(_uow)
            {
                MixTenantId = CurrentTenant.Id,
                CreatedBy = _identityService.GetClaim(HttpContextAccessor.HttpContext!.User, MixClaims.Username)
            };
            ReflectionHelper.Map(dto, userPermission);
            await userPermission.SaveAsync();
        }
    }
}