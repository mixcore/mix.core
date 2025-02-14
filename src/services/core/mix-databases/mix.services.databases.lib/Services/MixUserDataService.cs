using Microsoft.AspNetCore.Http;
using Mix.Mixdb.ViewModels;
using Mix.Constant.Enums;
using Mix.Database.Entities.Account;
using Mix.Heart.Enums;
using Mix.Heart.Exceptions;
using Mix.Heart.Helpers;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Services;
using Mix.Service.Services;
using Mix.Services.Databases.Lib.Dtos;
using Mix.Services.Databases.Lib.Interfaces;
using Mix.Heart.Services;
using Mix.Lib.Interfaces;

namespace Mix.Services.Databases.Lib.Services
{
    public sealed class MixUserDataService : TenantServiceBase, IMixUserDataService
    {
        private readonly TenantUserManager _userManager;
        private readonly UnitOfWorkInfo<MixDbDbContext> _uow;

        public MixUserDataService(
            IHttpContextAccessor httpContextAccessor,
            UnitOfWorkInfo<MixDbDbContext> uow,
            TenantUserManager userManager,
            MixCacheService cacheService,
            IMixTenantService mixTenantService)
            : base(httpContextAccessor, cacheService, mixTenantService)
        {
            _uow = uow;
            _userManager = userManager;
        }

        public async Task<MixUserDataViewModel> GetUserDataAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var data = await MixUserDataViewModel.GetRepository(_uow, CacheService).GetSingleAsync(m => m.ParentId == userId, cancellationToken);
            if (data == null)
            {
                data = await CreateDefaultUserData(userId, cancellationToken);
            }
            return data;
        }

        public async Task<MixUserDataViewModel> CreateDefaultUserData(Guid userId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            MixUserDataViewModel data = new(_uow)
            {
                TenantId = CurrentTenant.Id,
                CreatedBy = _userManager.GetUserName(HttpContextAccessor.HttpContext!.User),
                ParentId = userId,
                ParentType = MixDatabaseParentType.User
            };
            await data.SaveAsync(cancellationToken);
            return data;
        }
        public async Task UpdateProfile(MixUserDataViewModel profile, CancellationToken cancellationToken = default)
        {
            await profile.SaveAsync(cancellationToken);
        }
    }
}
