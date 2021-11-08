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
using Mix.Shared.Enums;

namespace Mix.Tenancy.Domain.Services
{
    public partial class InitCmsService
    {
        public async Task<AccessTokenViewModel> InitAccountAsync(RegisterViewModel model)
        {
            var accountContext = _databaseService.GetAccountDbContext();
            await accountContext.Database.MigrateAsync();
            AuthConfigService authConfigService = new(_configuration);
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
                    var aesKey = _globalConfigService.AppSettings.ApiEncryptKey;

                    var token = await _identityService.GenerateAccessTokenAsync(user, true, aesKey, rsaKeys[MixConstants.CONST_RSA_PUBLIC_KEY]);
                    if (token != null)
                    {
                        _globalConfigService.AppSettings.ApiEncryptKey = aesKey;
                        _globalConfigService.AppSettings.InitStatus = InitStep.InitAccount;
                        _globalConfigService.AppSettings.IsInit = false;
                        _globalConfigService.SaveSettings();

                        authConfigService.AppSettings.SecretKey = Guid.NewGuid().ToString("N");
                        authConfigService.SaveSettings();
                    }
                    return token;
                }
            }

            return null;
        }
    }
}
