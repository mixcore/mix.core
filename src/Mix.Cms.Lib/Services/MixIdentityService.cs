using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Dtos;
using Mix.Cms.Lib.Helpers;
using Mix.Cms.Lib.Models.Account;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.ViewModels.Account;
using Mix.Heart.Helpers;
using Mix.Heart.Models;
using Mix.Identity.Constants;
using Mix.Identity.Helpers;
using Mix.Identity.Models;
using Mix.Identity.Models.AccountViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.Services
{
    public class MixIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public readonly MixIdentityHelper _idHelper;
        public List<ViewModels.Account.MixRoles.ReadViewModel> Roles { get; set; }
        public MixIdentityService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            MixIdentityHelper helper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _idHelper = helper;

            LoadRoles();
        }

        public async Task<RepositoryResponse<JObject>> Login(LoginViewModel model)
        {
            RepositoryResponse<JObject> loginResult = new RepositoryResponse<JObject>();
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            var result = await _signInManager.PasswordSignInAsync(
                model.UserName, model.Password, isPersistent: model.RememberMe, lockoutOnFailure: true).ConfigureAwait(false);

            if (result.IsLockedOut)
            {
                loginResult.Errors.Add("This account has been locked out, please try again later.");
            }
            else
            {
                loginResult.Errors.Add("Login failed");
            }

            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(model.UserName).ConfigureAwait(false);
                var token = await GetAuthData(user, model.RememberMe);
                loginResult.IsSucceed = true;
                loginResult.Data = token;
            }

            return loginResult;
        }

        public async Task<JObject> GetAuthData(ApplicationUser user, bool rememberMe)
        {
            var rsaKeys = RSAEncryptionHelper.GenerateKeys();
            var aesKey = AesEncryptionHelper.GenerateCombinedKeys(256);
            var token = await GenerateAccessTokenAsync(user, rememberMe, aesKey, rsaKeys[MixConstants.CONST_RSA_PUBLIC_KEY]);
            if (token != null)
            {
                token.Info = new MixUserViewModel(user);
                await token.Info.LoadUserDataAsync();

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

        public async Task<AccessTokenViewModel> GenerateAccessTokenAsync(ApplicationUser user, bool isRemember, string aesKey, string rsaPublicKey)
        {
            try
            {
                var dtIssued = DateTime.UtcNow;
                var dtExpired = dtIssued.AddMinutes(MixService.GetAuthConfig<int>(MixAuthConfigurations.AccessTokenExpiration, 20));
                var dtRefreshTokenExpired = dtIssued.AddMinutes(MixService.GetAuthConfig<int>(MixAuthConfigurations.RefreshTokenExpiration));
                string refreshTokenId = string.Empty;
                string refreshToken = string.Empty;
                if (isRemember)
                {
                    refreshToken = Guid.NewGuid().ToString();
                    RefreshTokenViewModel vmRefreshToken = new RefreshTokenViewModel(
                                new RefreshTokens()
                                {
                                    Id = refreshToken,
                                    Email = user.Email,
                                    IssuedUtc = dtIssued,
                                    ClientId = MixService.GetAuthConfig<string>(MixAuthConfigurations.Audience),
                                    Username = user.UserName,
                                    //Subject = SWCmsConstants.AuthConfiguration.Audience,
                                    ExpiresUtc = dtRefreshTokenExpired
                                });

                    var saveRefreshTokenResult = await vmRefreshToken.SaveModelAsync();
                    refreshTokenId = saveRefreshTokenResult.Data?.Id;
                }

                AccessTokenViewModel token = new AccessTokenViewModel()
                {
                    Access_token = await _idHelper.GenerateTokenAsync(
                        user, dtExpired, refreshToken, aesKey, rsaPublicKey, MixService.Instance.MixAuthentications),
                    Refresh_token = refreshTokenId,
                    Token_type = MixService.GetAuthConfig<string>(MixAuthConfigurations.TokenType),
                    Expires_in = MixService.GetAuthConfig(MixAuthConfigurations.AccessTokenExpiration, 20),
                    Issued = dtIssued,
                    Expires = dtExpired,
                    LastUpdateConfiguration = MixService.GetAppSetting<DateTime?>(MixAppSettingKeywords.LastUpdateConfiguration)
                };
                return token;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public async Task<RepositoryResponse<JObject>> ExternalLogin(RegisterExternalBindingModel model)
        {
            RepositoryResponse<JObject> loginResult = new RepositoryResponse<JObject>();
            var verifiedAccessToken = await _idHelper.VerifyExternalAccessToken(model.Provider, model.ExternalAccessToken, MixService.Instance.MixAuthentications);
            if (verifiedAccessToken != null)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                // return local token if already register
                if (user != null)
                {
                    loginResult.Data = await GetAuthData(user, true);
                    loginResult.IsSucceed = true;
                }
                else if (!string.IsNullOrEmpty(model.Email))// register new account
                {
                    user = new ApplicationUser()
                    {
                        Email = model.Email,
                        UserName = model.Email
                    };
                    await _userManager.CreateAsync(user);
                    loginResult.Data = await GetAuthData(user, true);
                    loginResult.IsSucceed = true;
                }
                else
                {
                    loginResult.Errors.Add("Login Failed");
                }
            }
            return loginResult;
        }

        public async Task<RepositoryResponse<JObject>> RenewTokenAsync(RenewTokenDto refreshTokenDto)
        {
            RepositoryResponse<JObject> result = new RepositoryResponse<JObject>();
            var getRefreshToken = await RefreshTokenViewModel.Repository.GetSingleModelAsync(t => t.Id == refreshTokenDto.RefreshToken);
            if (getRefreshToken.IsSucceed)
            {
                var oldToken = getRefreshToken.Data;
                if (oldToken.ExpiresUtc > DateTime.UtcNow)
                {

                    var principle = _idHelper.GetPrincipalFromExpiredToken(refreshTokenDto.AccessToken, MixService.Instance.MixAuthentications);
                    if (principle != null && oldToken.Username == _idHelper.GetClaim(principle, MixClaims.Username))
                    {
                        var user = await _userManager.FindByEmailAsync(oldToken.Email);
                        await _signInManager.SignInAsync(user, true).ConfigureAwait(false);

                        var token = await GetAuthData(user, true);
                        if (token != null)
                        {
                            await oldToken.RemoveModelAsync();
                            result.IsSucceed = true;
                            result.Data = token;
                        }
                    }
                    else
                    {
                        result.Errors.Add("Invalid Token");
                    }

                    return result;
                }
                else
                {
                    await oldToken.RemoveModelAsync();
                    result.Errors.Add("Token expired");
                    return result;
                }
            }
            else
            {
                result.Errors.Add("Token expired");
                return result;
            }
        }

        public bool CheckEndpointPermission(ClaimsPrincipal user, PathString path, string method)
        {
            var roles = _idHelper.GetClaims(user, MixClaims.Role);
            if (roles.Any(r => r == MixDefaultRoles.SuperAdmin || r == MixDefaultRoles.Admin))
            {
                return true;
            }
            var endpoint = $"{method}-{path}";
            var role = Roles.Find(r => r.Name == roles.First());
            return role.MixPermissions.Any(
                    p => p.Property<JArray>("endpoints")
                            .Any(e => new Regex(e["endpoint"].Value<string>()).Match(path).Success
                                    && e["method"].Value<string>() == method.ToUpper())
                    );
        }

        private void LoadRoles()
        {
            if (!MixService.GetAppSetting<bool>(MixAppSettingKeywords.IsInit))
            {
                var getRoles = ViewModels.Account.MixRoles.ReadViewModel.Repository
                                .GetModelList();
                Roles = getRoles.Data;
                using (var ctx = new MixCmsContext())
                {
                    var transaction = ctx.Database.BeginTransaction();
                    Roles.ForEach(m => m.LoadMixPermissions(ctx, transaction).GetAwaiter().GetResult());
                }
            }
        }
    }
}