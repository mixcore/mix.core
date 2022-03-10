using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Mix.Database.Entities.Account;
using Mix.Identity.Enums;
using Mix.Identity.Models.AccountViewModels;
using Mix.Lib.Models;
using Mix.Shared.Enums;

namespace Mix.Tenancy.Domain.Services
{
    public partial class InitCmsService
    {
        public async Task<AccessTokenViewModel> InitAccountAsync(RegisterViewModel model)
        {
            var accountContext = _databaseService.GetAccountDbContext();
            await accountContext.Database.MigrateAsync();
            if (!_roleManager.Roles.Any())
            {
                var roles = MixHelper.LoadEnumValues(typeof(MixRoles));
                foreach (var role in roles)
                {
                    await _roleManager.CreateAsync(new IdentityRole()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = role.ToString()
                    });
                }
            }

            if (_userManager.Users.Count() == 0)
            {
                var user = new MixUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    JoinDate = DateTime.UtcNow,
                };
                var createResult = await _userManager.CreateAsync(user, password: model.Password).ConfigureAwait(false);
                if (createResult.Succeeded)
                {
                    user = await _userManager.FindByEmailAsync(model.Email).ConfigureAwait(false);
                    await _userManager.AddToRoleAsync(user, MixRoles.Owner.ToString());
                    // TODO: await MixAccountHelper.LoadUserInfoAsync(user.UserName);
                    var rsaKeys = RSAEncryptionHelper.GenerateKeys();
                    var aesKey = GlobalConfigService.Instance.AppSettings.ApiEncryptKey;

                    var token = await _identityService.GenerateAccessTokenAsync(user, true, aesKey, rsaKeys[MixConstants.CONST_RSA_PUBLIC_KEY]);
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
