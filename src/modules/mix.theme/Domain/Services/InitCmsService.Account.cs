using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Mix.Identity.Models.AccountViewModels;
using System.Linq;
using Mix.Shared.Constants;
using Mix.Heart.Helpers;
using Mix.Shared.Enums;
using Mix.Database.Entities.Account;
using Mix.Identity.Models;

namespace Mix.Theme.Domain.Services
{
    public partial class InitCmsService
    {
        public async Task<AccessTokenViewModel> InitAccountAsync(RegisterViewModel model)
        {
            var accountContext = _databaseService.GetAccountDbContext();
            await accountContext.Database.MigrateAsync();
            
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
                    var aesKey = _appSettingService.GetConfig<string>(
                            MixAppSettingsSection.GlobalSettings, MixAppSettingKeywords.ApiEncryptKey);

                    var token = await _idHelper.GenerateAccessTokenAsync(user, true, aesKey, rsaKeys[MixConstants.CONST_RSA_PUBLIC_KEY]);
                    if (token != null)
                    {

                        _appSettingService.SetConfig(
                            MixAppSettingsSection.GlobalSettings, MixAppSettingKeywords.ApiEncryptKey, aesKey);
                        _appSettingService.SetConfig(
                            MixAppSettingsSection.GlobalSettings, MixAppSettingKeywords.InitStatus, 2, true);
                    }
                    return token;
                }
            }

            return null;
        }
    }
}
