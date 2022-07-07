using Microsoft.EntityFrameworkCore;
using Mix.Database.Entities.Account;
using Mix.Identity.Enums;
using Mix.Identity.Models.AccountViewModels;
using Mix.Lib.Models;
using Mix.Lib.ViewModels;

namespace Mix.Tenancy.Domain.Services
{
    public partial class InitCmsService
    {
        public async Task<AccessTokenViewModel>? InitAccountAsync(RegisterViewModel model)
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

            if (_userManager.Users.Count() == 0)
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
                    await _userManager.AddToTenant(user, tenantId);
                    var rsaKeys = RSAEncryptionHelper.GenerateKeys();
                    var aesKey = GlobalConfigService.Instance.AppSettings.ApiEncryptKey;
                    var userInfo = new MixUserViewModel(user, _cmsUow);
                    //await userInfo.CreateDefaultUserData(tenantId);
                    await _cmsUow.CompleteAsync();
                    var token = await _identityService.GenerateAccessTokenAsync(
                        user, userInfo, true, aesKey, rsaKeys[MixConstants.CONST_RSA_PUBLIC_KEY]);
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
