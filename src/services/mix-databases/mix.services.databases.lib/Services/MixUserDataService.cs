using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Mix.Constant.Enums;
using Mix.Database.Entities.Account;
using Mix.Database.Entities.Cms;
using Mix.Heart.Enums;
using Mix.Heart.Exceptions;
using Mix.Heart.Helpers;
using Mix.Heart.UnitOfWork;
using Mix.Identity.Constants;
using Mix.Lib.Base;
using Mix.Lib.Services;
using Mix.Portal.Domain.ViewModels;
using Mix.Services.Databases.Lib.Dtos;
using Mix.Services.Databases.Lib.Entities;
using Mix.Services.Databases.Lib.Enums;
using Mix.Services.Databases.Lib.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Threading;

namespace Mix.Services.Databases.Lib.Services
{
    public sealed class MixUserDataService : TenantServiceBase
    {
        private readonly TenantUserManager _userManager;
        private readonly MixIdentityService _identityService;
        private MixServiceDatabaseDbContext _permissionDbContext;
        private UnitOfWorkInfo<MixCmsContext> _cmsUOW;
        private UnitOfWorkInfo<MixServiceDatabaseDbContext> _uow;

        public MixUserDataService(
            IHttpContextAccessor httpContextAccessor,
            UnitOfWorkInfo<MixServiceDatabaseDbContext> uow,
            MixIdentityService identityService,
            UnitOfWorkInfo<MixCmsContext> cmsUOW,
            TenantUserManager userManager)
            : base(httpContextAccessor)
        {
            _uow = uow;
            _permissionDbContext = _uow.DbContext;
            _identityService = identityService;
            _cmsUOW = cmsUOW;
            _userManager = userManager;
        }

        public async Task<MixUserDataViewModel> GetUserData(Guid userId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var data = await MixUserDataViewModel.GetRepository(_uow).GetSingleAsync(m => m.ParentId == userId);
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

        public async Task<MixUserDataViewModel> CreateUserAddress(CreateUserAddressDto dto, MixUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var userData = await GetUserData(user.Id);
            if (userData == null)
            {
                throw new MixException(MixErrorStatus.Badrequest, "User Data not existed");
            }

            MixContactAddressViewModel address = new(_uow)
            {
                MixTenantId = CurrentTenant.Id,
                SysUserDataId = userData.Id,
                CreatedBy = user.UserName
            };
            ReflectionHelper.MapObject(dto, address);
            await address.SaveAsync(cancellationToken);
            userData.Addresses.Add(address);
            return userData;
        }

        public async Task DeleteUserAddress(int addressId, MixUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var userData = await GetUserData(user.Id);
            if (userData == null)
            {
                throw new MixException(MixErrorStatus.Badrequest, "User Data not existed");
            }
            var address = userData.Addresses.SingleOrDefault(a => a.Id == addressId);
            if (address == null)
            {
                throw new MixException(MixErrorStatus.Badrequest, "Address not existed");
            }
            await address.DeleteAsync(cancellationToken);
        }

        public async Task UpdateUserAddress(MixContactAddressViewModel address, MixUser? user, CancellationToken cancellationToken)
        {
            var userData = await GetUserData(user.Id);
            if (address.SysUserDataId != userData.Id)
            {
                throw new MixException(MixErrorStatus.Badrequest);
            }

            await address.SaveAsync(cancellationToken);
        }
    }
}
