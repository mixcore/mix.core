using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Mix.Heart.Constants;
using Mix.Heart.Helpers;
using Mix.Heart.Models;
using Mix.Identity.Constants;
using Mix.Identity.Models;
using Mix.Lib;
using Mix.Lib.Constants;
using Mix.Lib.Entities.Cms;
using Mix.Lib.Enums;
using Mix.Lib.Services;
using Mix.Lib.ViewModels.Account;
using Mix.Services;
using Mix.Theme.Domain.Dtos;
using Mix.Theme.Domain.Helpers;
using Mix.Theme.Domain.ViewModels.Init;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mix.Theme.Controllers.v2
{
    [Route("api/v2/mix-theme/init")]
    [ApiController]
    public class InitController: Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly MixIdentityService _idService;

        public InitController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, MixIdentityService idService)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _idService = idService;
        }
        #region Post

        /// <summary>
        /// Step 1 when status = 0
        ///     - Init Cms Database
        ///     - Init Account Database
        ///     - Init Selected Culture as default
        ///     - Init Menu Positions
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("init-cms/step-1")]
        public async Task<RepositoryResponse<bool>> Step1([FromBody] InitCmsDto model)
        {
            if (model != null)
            {
                var result = new RepositoryResponse<bool>() { IsSucceed = true };
                if (MixAppSettingService.GetConfig<int>("InitStatus") == 0)
                {
                    result = await InitStep1Async(model).ConfigureAwait(false);
                }
                return result;
            }
            return new RepositoryResponse<bool>();
        }

        /// <summary>
        /// Step 2 when status = 1
        ///     - Init SuperAdmin
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("init-cms/step-2")]
        public async Task<RepositoryResponse<AccessTokenViewModel>> InitSuperAdmin([FromBody] MixRegisterViewModel model)
        {
            RepositoryResponse<AccessTokenViewModel> result = new RepositoryResponse<AccessTokenViewModel>();
            if (_userManager.Users.Count() == 0)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Username,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Avatar = model.Avatar ?? MixAppSettingService.GetConfig<string>("DefaultAvatar"),
                    JoinDate = DateTime.UtcNow
                };
                var createResult = await _userManager.CreateAsync(user, password: model.Password).ConfigureAwait(false);
                if (createResult.Succeeded)
                {
                    user = await _userManager.FindByEmailAsync(model.Email).ConfigureAwait(false);
                    await _userManager.AddToRoleAsync(user, MixRoles.SuperAdmin);
                    //await MixAccountHelper.LoadUserInfoAsync(user.UserName);
                    var rsaKeys = RSAEncryptionHelper.GenerateKeys();
                    var aesKey = MixAppSettingService.GetConfig<string>(MixAppSettingKeywords.ApiEncryptKey);

                    var token = await _idService.GenerateAccessTokenAsync(user, true, aesKey, rsaKeys[MixConstants.CONST_RSA_PUBLIC_KEY]);
                    if (token != null)
                    {
                        result.IsSucceed = true;
                        MixAppSettingService.LoadFromDatabase();
                        MixAppSettingService.SetConfig(MixAppSettingsSection.GlobalSettings, MixAppSettingKeywords.ApiEncryptKey, aesKey);
                        MixAppSettingService.SetConfig(MixAppSettingsSection.GlobalSettings, "InitStatus", 2);
                        MixAppSettingService.SaveSettings();
                        MixAppSettingService.Reload();
                        result.Data = token;
                        return result;
                    }
                    else
                    {
                        return result;
                    }
                }
                else
                {
                    foreach (var error in createResult.Errors)
                    {
                        result.Errors.Add(error.Description);
                    }
                    return result;
                }
            }
            return result;
        }
        
        /// <summary>
        /// Step 3 when status = 3 (Finished)
        ///     - Init default theme
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("init-cms/step-3")]
        [DisableRequestSizeLimit]
        public async Task<RepositoryResponse<InitThemeViewModel>> Save(InitThemePackageDto request)
        {
            string _lang = MixAppSettingService.GetConfig<string>("Language");
            string user = _idService._helper.GetClaim(User, MixClaims.Username);
            return await ThemeHelper.InitTheme(request, user, _lang);
        }

        
        /// <summary>
        /// Step 4 when status = 3
        ///     - Init Languages for translate from languages.json
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("init-cms/step-4")]
        public async Task<RepositoryResponse<bool>> InitLanguages([FromBody] List<MixLanguage> model)
        {
            if (model != null)
            {
                var result = new RepositoryResponse<bool>();
                if (MixAppSettingService.GetConfig<int>("InitStatus") == 3)
                {
                    string culture = MixAppSettingService.GetConfig<string>("DefaultCulture");
                    InitCmsService sv = new InitCmsService();
                    result = await sv.InitLanguagesAsync(culture, model);
                    if (result.IsSucceed)
                    {
                        MixAppSettingService.LoadFromDatabase();
                        MixAppSettingService.SetConfig(MixAppSettingsSection.GlobalSettings, "InitStatus", 4);
                        MixAppSettingService.SetConfig(MixAppSettingsSection.GlobalSettings, MixAppSettingKeywords.IsInit, true);
                        MixAppSettingService.SaveSettings();
                        _ = Services.MixCacheService.RemoveCacheAsync();
                        MixAppSettingService.Reload();
                    }
                }
                return result;
            }
            return new RepositoryResponse<bool>();
        }

        /// <summary>
        /// Step 3 when status = 3 (Finished)
        ///     - Init default theme
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("init-cms/step-3/active")]
        [DisableRequestSizeLimit]
        public async Task<ActionResult<bool>> Active([FromBody] InitThemeViewModel model)
        {
            model.IsActived = true;
            var result = await ThemeHelper.ActivedThemeAsync(model.Id, model.Name, model.Specificulture);
            if (result.IsSucceed)
            {
                // MixService.SetConfig<string>(MixAppSettingKeywords.SiteName, _lang, data.Title);
                MixAppSettingService.LoadFromDatabase();
                MixAppSettingService.SetConfig(MixAppSettingsSection.GlobalSettings, "InitStatus", 3);
                MixAppSettingService.SetConfig(MixAppSettingsSection.GlobalSettings, MixAppSettingKeywords.IsInit, false);
                MixAppSettingService.SaveSettings();
                _ = MixCacheService.RemoveCacheAsync();
                MixAppSettingService.Reload();
            }
            return Ok(result);
        }
        #endregion Post

        #region Helpers

        private async Task<RepositoryResponse<bool>> InitStep1Async(InitCmsDto model)
        {
            MixAppSettingService.SetConfig(MixAppSettingsSection.ConnectionStrings, MixConstants.CONST_CMS_CONNECTION, model.ConnectionString);
            MixAppSettingService.SetConfig(MixAppSettingsSection.ConnectionStrings, MixConstants.CONST_MESSENGER_CONNECTION, model.ConnectionString);
            MixAppSettingService.SetConfig(MixAppSettingsSection.ConnectionStrings, MixConstants.CONST_ACCOUNT_CONNECTION, model.ConnectionString);
            MixAppSettingService.SetConfig(MixAppSettingsSection.GlobalSettings, MixConstants.CONST_SETTING_DATABASE_PROVIDER, model.DatabaseProvider.ToString());
            MixAppSettingService.SetConfig(MixAppSettingsSection.GlobalSettings, MixConstants.CONST_SETTING_LANGUAGE, model.Culture.Specificulture);
            MixAppSettingService.SetConfig(MixAppSettingsSection.MixConfigurations, WebConfiguration.MixCacheConnectionString, model.ConnectionString);
            MixAppSettingService.SetConfig(MixAppSettingsSection.MixConfigurations, WebConfiguration.MixCacheDbProvider, model.DatabaseProvider.ToString());
            MixAppSettingService.SaveSettings();
            MixAppSettingService.Reload();
            var result = await InitCmsService.InitCms(model.SiteName, model.Culture);

            if (result.IsSucceed)
            {
                await InitRolesAsync();
                result.IsSucceed = true;
                MixAppSettingService.LoadFromDatabase();
                MixAppSettingService.SetConfig<string>(MixAppSettingsSection.GlobalSettings, "DefaultCulture", model.Culture.Specificulture);
                MixAppSettingService.SetConfig(MixAppSettingsSection.GlobalSettings, "InitStatus", 1);
                MixAppSettingService.SaveSettings();
                //MixService.Reload();
            }
            else
            {
                // if cannot init cms
                //  => reload from default settings
                //  => save to appSettings
                MixAppSettingService.Reload();
                MixAppSettingService.SaveSettings();
            }
            return result;
        }

        private async Task<bool> InitRolesAsync()
        {
            bool isSucceed = true;
            var getRoles = await RoleViewModel.Repository.GetModelListAsync();
            if (getRoles.IsSucceed && getRoles.Data.Count == 0)
            {
                var saveResult = await _roleManager.CreateAsync(new IdentityRole()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = MixRoles.SuperAdmin
                });
                isSucceed = saveResult.Succeeded;
            }
            return isSucceed;
        }

        #endregion Helpers
    }
}
