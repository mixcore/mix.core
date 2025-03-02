using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Mix.Auth.Models;
using Mix.Database.Entities.Account;
using Mix.Database.Services;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Identity.Enums;
using Mix.Lib.Interfaces;
using Mix.Lib.Services;
using Mix.Shared.Helpers;
using Mix.Shared.Models;
using Mix.Shared.Models.Configurations;
using Mix.Tenancy.Domain.Dtos;
using Mix.Tenancy.Domain.Interfaces;
using Mix.Tenancy.Domain.ViewModels.Init;

namespace Mix.Tenancy.Domain.Services
{
    public class InitCmsService : IInitCmsService
    {
        private readonly AppSettingsService _appSettingsService;
        private readonly MixIdentityService _identityService;
        private readonly TenantUserManager _userManager;
        private readonly RoleManager<MixRole> _roleManager;
        private readonly DatabaseService _databaseService;
        private readonly MixCmsContext _context;
        private readonly UnitOfWorkInfo<MixCmsContext> _cmsUow;
        protected readonly IConfiguration _configuration;
        public InitCmsService(
            TenantUserManager userManager,
            MixIdentityService identityService,
            DatabaseService databaseService,
            RoleManager<MixRole> roleManager,
            UnitOfWorkInfo<MixCmsContext> cmsUow,
            IMixTenantService mixTenantService,
            AppSettingsService appSettingsService,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _identityService = identityService;
            _roleManager = roleManager;
            _cmsUow = cmsUow;
            _context = cmsUow.DbContext;
            _databaseService = databaseService;
            _appSettingsService = appSettingsService;
            _configuration = configuration;
        }


        public Task InitDbContext(InitCmsDto model)
        {
            _appSettingsService.SetConfig(MixConstants.CONST_SETTINGS_CONNECTION, model.ConnectionString);
            _appSettingsService.SetConfig(nameof(AppSettingsModel.DefaultCulture), model.Culture.Specificulture);
            _appSettingsService.SetConfig(nameof(AppSettingsModel.DatabaseProvider), model.DatabaseProvider.ToString(), true);
            _configuration[nameof(AppSettingsModel.DatabaseProvider)] = model.DatabaseProvider.ToString();
            _configuration[MixConstants.CONST_SETTINGS_CONNECTION] = model.ConnectionString;
            _databaseService.InitConnectionStrings(model.ConnectionString, model.DatabaseProvider);
            _databaseService.UpdateMixCmsContext();
            return Task.CompletedTask;
        }

        public async Task InitTenantAsync(InitCmsDto model)
        {
            InitTenantViewModel vm = new(_cmsUow, model);
            await vm.SaveAsync();
        }

        public async Task<TokenResponseModel> InitAccountAsync(RegisterRequestModel model)
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
                    await _cmsUow.CompleteAsync();
                    var token = await _identityService.GenerateAccessTokenAsync(
                        user, true);
                    return token;
                }
            }

            return null;
        }
    }
}