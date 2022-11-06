using Mix.Heart.Exceptions;
using Mix.Heart.Helpers;
using Mix.Heart.UnitOfWork;
using Mix.Identity.Constants;
using Mix.Lib.Base;
using Mix.Lib.Services;
using Mix.Services.Permission.Domain.Dtos;
using Mix.Services.Permission.Domain.Entities;
using Mix.Services.Permission.Domain.ViewModels;
using System.Linq.Expressions;

namespace Mix.Services.Permission.Domain.Services
{
    public sealed class MixPermissionService : TenantServiceBase
    {
        private readonly MixIdentityService _identityService;
        private PermissionDbContext _permissionDbContext;
        private UnitOfWorkInfo<PermissionDbContext> _uow;

        public MixPermissionService(
            IHttpContextAccessor httpContextAccessor,
            UnitOfWorkInfo<PermissionDbContext> uow,
            MixIdentityService identityService)
            : base(httpContextAccessor)
        {
            _uow = uow;
            _permissionDbContext = _uow.DbContext;
            _identityService = identityService;
        }

        public async Task<List<MixPermissionViewModel>> GetPermissionAsyncs(Guid userId)
        {
            var permissions = _permissionDbContext.UserPermission.Where(m => m.MixTenantId == CurrentTenant.Id && m.UserId == userId);
            Expression<Func<MixPermission, bool>> predicate =
                m => m.MixTenantId == CurrentTenant.Id
                && permissions.Any(p => p.PermissionId == m.Id);
            var result = await MixPermissionViewModel.GetRepository(_uow).GetAllAsync(predicate);
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
                CreatedBy = _identityService.GetClaim(_httpContextAccessor.HttpContext!.User, MixClaims.Username)
            };
            ReflectionHelper.MapObject(dto, userPermission);
            await userPermission.SaveAsync();
        }
    }
}
