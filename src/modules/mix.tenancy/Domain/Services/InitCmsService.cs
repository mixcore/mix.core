using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Mix.Database.Entities.Account;
using Mix.Database.Services;
using Mix.Identity.Enums;
using Mix.Identity.Models.AccountViewModels;
using Mix.Lib.Interfaces;
using Mix.Lib.Models;
using Mix.Lib.Services;
using Mix.Shared.Helpers;
using Mix.Tenancy.Domain.Dtos;
using Mix.Tenancy.Domain.Interfaces;
using Mix.Tenancy.Domain.ViewModels.Init;

namespace Mix.Tenancy.Domain.Services
{
    public class InitCmsService : IInitCmsService
    {
        private readonly IMixTenantService _mixTenantService;
        private readonly MixIdentityService _identityService;
        private readonly TenantUserManager _userManager;
        private readonly RoleManager<MixRole> _roleManager;
        private readonly DatabaseService _databaseService;
        private readonly MixCmsContext _context;
        private readonly UnitOfWorkInfo<MixCmsContext> _cmsUow;

        public InitCmsService(
            TenantUserManager userManager,
            MixIdentityService identityService,
            DatabaseService databaseService,
            RoleManager<MixRole> roleManager,
            UnitOfWorkInfo<MixCmsContext> cmsUow, 
            IMixTenantService mixTenantService)
        {
            _userManager = userManager;
            _identityService = identityService;
            _roleManager = roleManager;
            _cmsUow = cmsUow;
            _context = cmsUow.DbContext;
            _databaseService = databaseService;
            _mixTenantService = mixTenantService;
        }


        public async Task InitDbContext(InitCmsDto model)
        {
            _databaseService.InitConnectionStrings(model.ConnectionString, model.DatabaseProvider);

            await _databaseService.UpdateMixCmsContextAsync();
        }

        public async Task InitTenantAsync(InitCmsDto model)
        {
            InitTenantViewModel vm = new(_context, model);
            await vm.SaveAsync();
            await _mixTenantService.Reload();
            GlobalConfigService.Instance.AppSettings.InitStatus = InitStep.InitTenant;
            GlobalConfigService.Instance.SaveSettings();
        }

        public async Task<AccessTokenViewModel> InitAccountAsync(RegisterViewModel model)
        {
            var accountContext = _databaseService.GetAccountDbContext();
            await accountContext.Database.MigrateAsync();
            if (!_roleManager.Roles.Any())
            {
                var roles = MixHelper.LoadEnumValues(typeof(MixRoleEnums));
                foreach (var role in roles)
                {
                    await _roleManager.CreateAsync(new MixRole()
                    {
                        Id = Guid.NewGuid(),
                        Name = role.ToString()
                    }
                    );
                }
            }

            if (!_userManager.Users.Any())
            {
                var user = new MixUser
                {
                    Id = Guid.NewGuid(),
                    UserName = model.UserName,
                    Email = model.Email,
                    CreatedDateTime = DateTime.UtcNow,
                    LastModified = DateTime.UtcNow

                };
                var createResult = await _userManager.CreateAsync(user, password: model.Password).ConfigureAwait(false);
                if (createResult.Succeeded)
                {
                    user = await _userManager.FindByEmailAsync(model.Email).ConfigureAwait(false);
                    await _userManager.AddToRoleAsync(user, MixRoleEnums.SuperAdmin.ToString());
                    await _userManager.AddToRoleAsync(user, MixRoleEnums.Owner.ToString());
                    await _userManager.AddToTenant(user, 1);
                    var rsaKeys = RSAEncryptionHelper.GenerateKeys();
                    var aesKey = GlobalConfigService.Instance.AppSettings.ApiEncryptKey;
                    await _cmsUow.CompleteAsync();
                    var token = await _identityService.GenerateAccessTokenAsync(
                        user, true, aesKey, rsaKeys[MixConstants.CONST_RSA_PUBLIC_KEY]);
                    if (token != null)
                    {
                        GlobalConfigService.Instance.AppSettings.ApiEncryptKey = aesKey;
                        GlobalConfigService.Instance.AppSettings.InitStatus = InitStep.InitAccount;
                        GlobalConfigService.Instance.SaveSettings();
                    }
                    return token;
                }
            }

            return null;
        }
    }
}