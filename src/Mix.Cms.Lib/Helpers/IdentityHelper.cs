using Microsoft.AspNetCore.Identity;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Models.Account;
using Mix.Cms.Lib.Services;
using Mix.Cms.Lib.ViewModels.Account;
using Mix.Heart.Helpers;
using Mix.Identity.Helpers;
using Mix.Identity.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.Helpers
{
    public class IdentityHelper
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly MixIdentityHelper _helper;

        public IdentityHelper(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager, 
            MixIdentityHelper helper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _helper = helper;
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
            var dtIssued = DateTime.UtcNow;
            var dtExpired = dtIssued.AddMinutes(MixService.GetAuthConfig<int>(MixAuthConfigurations.CookieExpiration));
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
                Access_token = await _helper.GenerateTokenAsync(user, dtExpired, refreshToken, aesKey, rsaPublicKey, MixService.Instance.MixAuthentications),
                Refresh_token = refreshTokenId,
                Token_type = MixService.GetAuthConfig<string>(MixAuthConfigurations.TokenType),
                Expires_in = MixService.GetAuthConfig<int>(MixAuthConfigurations.CookieExpiration),
                Issued = dtIssued,
                Expires = dtExpired,
                LastUpdateConfiguration = MixService.GetConfig<DateTime?>(MixAppSettingKeywords.LastUpdateConfiguration)
            };
            return token;
        }
    }
}