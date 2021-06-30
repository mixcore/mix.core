using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Mix.Database.Entities.Account;
using Mix.Database.Entities.Cms.v2;
using Mix.Heart.Helpers;
using Mix.Heart.Repository;
using Mix.Identity.Constants;
using Mix.Identity.Helpers;
using Mix.Identity.Models;
using Mix.Identity.Models.AccountViewModels;
using Mix.Identity.ViewModels;
using Mix.Lib.Dtos;
using Mix.Lib.Services;
using Mix.Shared.Constants;
using Mix.Shared.Enums;
using Mix.Shared.Models;
using Mix.Shared.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Identity.Services
{
    public class MixIdentityService
    {
        private readonly UserManager<MixUser> _userManager;
        private readonly SignInManager<MixUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly MixAppSettingService _appSettingService;
        private readonly MixCmsAccountContext _context;
        public readonly MixIdentityHelper _idHelper;
        protected CommandRepository<MixCmsAccountContext, RefreshTokens, Guid> _refreshTokenRepo;
        protected CommandRepository<MixCmsAccountContext, AspNetRoles, Guid> _roleRepo;
        public List<RoleViewModel> Roles { get; set; }

        public MixIdentityService(
            UserManager<MixUser> userManager,
            SignInManager<MixUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            MixIdentityHelper helper,
            MixAppSettingService appSettingService,
            CommandRepository<MixCmsAccountContext, RefreshTokens, Guid> refreshTokenRepo,
            CommandRepository<MixCmsAccountContext, AspNetRoles, Guid> roleRepo, 
            MixCmsAccountContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _idHelper = helper;
            _appSettingService = appSettingService;

            LoadRoles();
            _refreshTokenRepo = refreshTokenRepo;
            _roleRepo = roleRepo;
            _context = context;
        }

        public async Task<JObject> Login(LoginViewModel model)
        {
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            var result = await _signInManager.PasswordSignInAsync(
                model.UserName, model.Password, isPersistent: model.RememberMe, lockoutOnFailure: true).ConfigureAwait(false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(model.UserName).ConfigureAwait(false);
                var token = await GetAuthData(user, model.RememberMe);
            }

            if (result.IsLockedOut)
            {
                throw new Exception("This account has been locked out, please try again later.");
            }
            else
            {
                throw new Exception("Logi failed");
            }
        }

        public async Task<JObject> GetAuthData(MixUser user, bool rememberMe)
        {
            var rsaKeys = RSAEncryptionHelper.GenerateKeys();
            var aesKey = AesEncryptionHelper.GenerateCombinedKeys(256);
            var token = await GenerateAccessTokenAsync(user, rememberMe, aesKey, rsaKeys[MixConstants.CONST_RSA_PUBLIC_KEY]);
            if (token != null)
            {
                //TODO: token.Info = new MixUserViewModel(user);

                var plainText = JObject.FromObject(token).ToString(Formatting.None).Replace("\r\n", string.Empty);
                var encryptedInfo = AesEncryptionHelper.EncryptString(plainText, aesKey);

                var resp = new JObject()
                        {
                            new JProperty("k", aesKey),
                            new JProperty("rpk", rsaKeys[MixConstants.CONST_RSA_PRIVATE_KEY]),
                            new JProperty("data", encryptedInfo)
                        };
                return resp;
            }
            return default;
        }

        public async Task<AccessTokenViewModel> GenerateAccessTokenAsync(MixUser user, bool isRemember, string aesKey, string rsaPublicKey)
        {
            try
            {
                var dtIssued = DateTime.UtcNow;
                var dtExpired = dtIssued.AddMinutes(_appSettingService.GetConfig(
                        MixAppSettingsSection.Authentication, MixAuthConfigurations.AccessTokenExpiration, 20));
                var dtRefreshTokenExpired = dtIssued.AddMinutes(_appSettingService.GetConfig(
                        MixAppSettingsSection.Authentication, MixAuthConfigurations.RefreshTokenExpiration, 20));
                var refreshToken = string.Empty;
                var refreshTokenId = Guid.Empty;
                if (isRemember)
                {
                    refreshTokenId = Guid.NewGuid();
                    RefreshTokenViewModel vmRefreshToken = new RefreshTokenViewModel(_context)
                    {
                        Id = refreshTokenId,
                        Email = user.Email,
                        IssuedUtc = dtIssued,
                        ClientId = _appSettingService.GetConfig(
                        MixAppSettingsSection.Authentication, MixAuthConfigurations.Audience, string.Empty),
                        Username = user.UserName,
                        //Subject = SWCmsConstants.AuthConfiguration.Audience,
                        ExpiresUtc = dtRefreshTokenExpired
                    };

                    var saveRefreshTokenResult = await vmRefreshToken.SaveAsync();
                    refreshToken = saveRefreshTokenResult.ToString();
                }
                var authSettings = _appSettingService.LoadSection<MixAuthenticationConfigurations>(MixAppSettingsSection.Authentication);
                AccessTokenViewModel token = new AccessTokenViewModel()
                {
                    AccessToken = await _idHelper.GenerateTokenAsync(
                        user, dtExpired, refreshToken, aesKey, rsaPublicKey, authSettings),
                    RefreshToken = refreshToken,
                    TokenType = _appSettingService.GetConfig(
                        MixAppSettingsSection.Authentication, MixAuthConfigurations.TokenType, "Bearer"),
                    ExpiresIn = _appSettingService.GetConfig(
                        MixAppSettingsSection.Authentication, MixAuthConfigurations.AccessTokenExpiration, 20),
                    Issued = dtIssued,
                    Expires = dtExpired,
                    LastUpdateConfiguration = _appSettingService.GetConfig(
                        MixAppSettingsSection.GlobalSettings, MixAppSettingKeywords.LastUpdateConfiguration, DateTime.UtcNow)
                };
                return token;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public async Task<JObject> RenewTokenAsync(RenewTokenDto refreshTokenDto)
        {
            JObject result = new JObject();
            var oldToken = await _refreshTokenRepo.GetSingleAsync(t => t.Id == refreshTokenDto.RefreshToken);
            if (oldToken != null)
            {
                if (oldToken.ExpiresUtc > DateTime.UtcNow)
                {
                    var authSettings = _appSettingService.LoadSection<MixAuthenticationConfigurations>(MixAppSettingsSection.Authentication);
                    var principle = _idHelper.GetPrincipalFromExpiredToken(refreshTokenDto.AccessToken, authSettings);
                    if (principle != null && oldToken.Username == _idHelper.GetClaim(principle, MixClaims.Username))
                    {
                        var user = await _userManager.FindByEmailAsync(oldToken.Email);
                        await _signInManager.SignInAsync(user, true).ConfigureAwait(false);

                        result = await GetAuthData(user, true);
                        if (result != null)
                        {
                            await _refreshTokenRepo.DeleteAsync(oldToken);
                        }
                    }
                    else
                    {
                        throw new Exception("Invalid Token");
                    }

                    return result;
                }
                else
                {
                    await _refreshTokenRepo.DeleteAsync(oldToken);
                    throw new Exception("Token expired");
                }
            }
            else
            {
                throw new Exception("Token expired");
            }
        }

        public bool CheckEndpointPermission(ClaimsPrincipal user, PathString path, string method)
        {
            return true;

            var roles = _idHelper.GetClaims(user, MixClaims.Role);
            if (roles.Any(r => r == MixDefaultRoles.SuperAdmin || r == MixDefaultRoles.Admin))
            {
                return true;
            }
            var endpoint = $"{method}-{path}";
            var role = Roles.Find(r => r.Name == roles.First());
            
            //return role.MixPermissions.Any(
            //        p => p.Property<JArray>("endpoints")
            //                .Any(e => new Regex(e["endpoint"].Value<string>()).Match(path).Success
            //                        && e["method"].Value<string>() == method.ToUpper())
            //        );
        }

        private void LoadRoles()
        {
            if (!_appSettingService.GetConfig(MixAppSettingsSection.GlobalSettings, MixAppSettingKeywords.IsInit, true))
            {
                _roleRepo
                    .GetAllQuery()
                    .ToList()
                    .ForEach(r=> Roles.Add(new RoleViewModel(r)));
            }
        }
    }
}
