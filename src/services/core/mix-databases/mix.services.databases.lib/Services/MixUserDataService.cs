﻿using Microsoft.AspNetCore.Http;
using Mix.Constant.Enums;
using Mix.Database.Entities.Account;
using Mix.Heart.Enums;
using Mix.Heart.Exceptions;
using Mix.Heart.Helpers;
using Mix.Heart.Services;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Interfaces;
using Mix.Lib.Services;
using Mix.Mixdb.ViewModels;
using Mix.Service.Services;
using Mix.Services.Databases.Lib.Dtos;
using Mix.Services.Databases.Lib.Interfaces;

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
                MixTenantId = CurrentTenant.Id,
                CreatedBy = _userManager.GetUserName(HttpContextAccessor.HttpContext!.User),
                ParentId = userId,
                ParentType = MixDatabaseParentType.User
            };
            await data.SaveAsync(cancellationToken);
            return data;
        }

        public async Task DeleteUserAddress(int addressId, MixUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var userData = await GetUserDataAsync(user.Id, cancellationToken);
            if (userData is null || userData.Addresses is null)
            {
                throw new MixException(MixErrorStatus.Badrequest, "User Data not existed");
            }

            var address = userData.Addresses.SingleOrDefault(a => a.Id == addressId);
            if (address is null)
            {
                throw new MixException(MixErrorStatus.Badrequest, "Address not existed");
            }

            await address.DeleteAsync(cancellationToken);
        }


        public async Task<MixUserDataViewModel> CreateUserAddress(CreateUserAddressDto dto, MixUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var userData = await GetUserDataAsync(user.Id, cancellationToken);
            if (userData is null)
            {
                throw new MixException(MixErrorStatus.Badrequest, "User Data not existed");
            }

            MixContactAddressViewModel address = new(_uow)
            {
                MixTenantId = CurrentTenant.Id,
                SysUserDataId = userData.Id,
                CreatedBy = user.UserName
            };
            ReflectionHelper.Map(dto, address);
            await address.SaveAsync(cancellationToken);
            return userData;
        }

        public async Task UpdateUserAddress(MixContactAddressViewModel address, MixUser user, CancellationToken cancellationToken)
        {
            var userData = await GetUserDataAsync(user.Id, cancellationToken);
            if (address.SysUserDataId != userData.Id)
            {
                throw new MixException(MixErrorStatus.Badrequest);
            }
            address.SetUowInfo(_uow, CacheService);
            await address.SaveAsync(cancellationToken);
        }

        public async Task UpdateProfile(MixUserDataViewModel profile, CancellationToken cancellationToken = default)
        {
            await profile.SaveAsync(cancellationToken);
        }
    }
}
