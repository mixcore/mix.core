using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Mix.Identity.Models.AccountViewModels;
using System.Linq;
using Mix.Shared.Constants;
using Mix.Heart.Helpers;
using Mix.Database.Entities.Account;
using Microsoft.AspNetCore.Identity;
using Mix.Identity.Constants;
using Mix.Shared.Services;
using Mix.Lib.Models;

namespace Mix.Theme.Domain.Services
{
    public partial class InitCmsService
    {
        public async Task<AccessTokenViewModel> InitAccountAsync(RegisterViewModel model)
        {
            var accountContext = _databaseService.GetAccountDbContext();
            await accountContext.Database.MigrateAsync();
            AuthConfigService authConfigService = new();
            if (!_roleManager.Roles.Any())
            {
                await _roleManager.CreateAsync(new IdentityRole()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = MixDefaultRoles.SuperAdmin
                });
            }

            if (_userManager.Users.Count() == 0)
            {
                var user = new MixUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    JoinDate = DateTime.UtcNow
                };
                var createResult = await _userManager.CreateAsync(user, password: model.Password).ConfigureAwait(false);
                if (createResult.Succeeded)
                {
                    user = await _userManager.FindByEmailAsync(model.Email).ConfigureAwait(false);
                    await _userManager.AddToRoleAsync(user, MixRoles.SuperAdmin);
                    // TODO: await MixAccountHelper.LoadUserInfoAsync(user.UserName);
                    var rsaKeys = RSAEncryptionHelper.GenerateKeys();
                    var aesKey = _globalConfigService.GetConfig<string>(MixAppSettingKeywords.ApiEncryptKey);

                    var token = await _identityService.GenerateAccessTokenAsync(user, true, aesKey, rsaKeys[MixConstants.CONST_RSA_PUBLIC_KEY]);
                    if (token != null)
                    {

                        _globalConfigService.SetConfig(MixAppSettingKeywords.IsInit, false);
                        _globalConfigService.SetConfig(MixAppSettingKeywords.ApiEncryptKey, aesKey);
                        _globalConfigService.SetConfig(MixAppSettingKeywords.InitStatus, 2);
                        authConfigService.SetConfig(MixAuthConfigurations.SecretKey, Guid.NewGuid().ToString("N"));
                    }
                    return token;
                }
            }

            return null;
        }
    }
}
