using Microsoft.AspNetCore.Http;
using Mix.Heart.Exceptions;
using Mix.Heart.Helpers;
using Mix.Heart.UnitOfWork;
using Mix.Identity.Constants;
using Mix.Lib.Base;
using Mix.Lib.Services;
using Mix.Services.Databases.Lib.Dtos;
using Mix.Services.Databases.Lib.Entities;
using Mix.Services.Databases.Lib.ViewModels;
using System.Linq.Expressions;

namespace Mix.Services.Databases.Lib.Services
{
    public sealed class MixMetadataService : TenantServiceBase
    {
        private readonly MixIdentityService _identityService;
        private MixServiceDatabaseDbContext _permissionDbContext;
        private UnitOfWorkInfo<MixServiceDatabaseDbContext> _uow;

        public MixMetadataService(
            IHttpContextAccessor httpContextAccessor,
            UnitOfWorkInfo<MixServiceDatabaseDbContext> uow,
            MixIdentityService identityService)
            : base(httpContextAccessor)
        {
            _uow = uow;
            _permissionDbContext = _uow.DbContext;
            _identityService = identityService;
        }

        public async Task<List<MixMetadataViewModel>> GetPermissionAsyncs(Guid userId)
        {
            var permissions = _permissionDbContext.UserPermission.Where(m => m.MixTenantId == CurrentTenant.Id && m.UserId == userId);
            Expression<Func<MixMetadata, bool>> predicate =
                m => m.MixTenantId == CurrentTenant.Id
                && permissions.Any(p => p.PermissionId == m.Id);
            var result = await MixMetadataViewModel.GetRepository(_uow).GetAllAsync(predicate);
            return result;
        }

        public async Task AddMetadataLink(CreateMetadataLinkDto dto)
        {
            MixUserPermissionViewModel userPermission = new(_uow)
            {
                MixTenantId = CurrentTenant.Id,
                CreatedBy = _identityService.GetClaim(HttpContextAccessor.HttpContext!.User, MixClaims.Username)
            };
            ReflectionHelper.MapObject(dto, userPermission);
            await userPermission.SaveAsync();
        }
    }
}
