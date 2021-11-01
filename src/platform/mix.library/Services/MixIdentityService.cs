using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Mix.Database.Entities.Account;
using Mix.Database.Entities.Cms;
using Mix.Heart.Enums;
using Mix.Heart.Exceptions;
using Mix.Heart.Helpers;
using Mix.Heart.Repository;
using Mix.Heart.UnitOfWork;
using Mix.Identity.Constants;
using Mix.Identity.Dtos;
using Mix.Identity.Models;
using Mix.Identity.Models.AccountViewModels;
using Mix.Identity.ViewModels;
using Mix.Lib.Helpers;
using Mix.Lib.Models;
using Mix.Shared.Constants;
using Mix.Shared.Enums;
using Mix.Shared.Models;
using Mix.Shared.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Lib.Services
{
    public class MixIdentityService
    {
        private readonly UnitOfWorkInfo _uow;
        private readonly UserManager<MixUser> _userManager;
        private readonly SignInManager<MixUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AuthConfigService _authConfigService;
        private readonly GlobalConfigService _globalConfigService;
        private readonly MixCmsContext _context;
        private readonly Repository<MixCmsAccountContext, AspNetRoles, Guid, RoleViewModel> _roleRepo;
        private readonly Repository<MixCmsAccountContext, RefreshTokens, Guid, RefreshTokenViewModel> _refreshTokenRepo;
        public List<RoleViewModel> Roles { get; set; }
        public MixIdentityService(
            UserManager<MixUser> userManager,
            SignInManager<MixUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            AuthConfigService authConfigService,
            GlobalConfigService globalConfigService,
            MixCmsContext context,
            MixCmsAccountContext accountContext)
        {
            _context = context;
            _uow = new(accountContext);
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _authConfigService = authConfigService;
            _roleRepo = RoleViewModel.GetRootRepository(accountContext);
            _globalConfigService = globalConfigService;
            _refreshTokenRepo = RefreshTokenViewModel.GetRootRepository(accountContext);
            LoadRoles();
        }

        public async Task<JObject> Login(LoginViewModel model)
        {
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            var result = await _signInManager.PasswordSignInAsync(
                model.UserName, model.Password, isPersistent: model.RememberMe, lockoutOnFailure: true).ConfigureAwait(false);

            if (result.IsLockedOut)
            {
                throw new MixException(MixErrorStatus.Badrequest, "This account has been locked out, please try again later.");
            }

            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(model.UserName).ConfigureAwait(false);
                return await GetAuthData(_context, user, model.RememberMe);
            }
            else
            {
                throw new MixException(MixErrorStatus.Badrequest, "Login failed");
            }
        }

        public async Task<JObject> GetAuthData(MixCmsContext context, MixUser user, bool rememberMe)
        {
            var rsaKeys = RSAEncryptionHelper.GenerateKeys();
            var aesKey = AesEncryptionHelper.GenerateCombinedKeys(256);
            var token = await GenerateAccessTokenAsync(user, rememberMe, aesKey, rsaKeys[MixConstants.CONST_RSA_PUBLIC_KEY]);
            if (token != null)
            {
                token.Info = await MixDataHelper.GetAdditionalDataAsync(
                    context,
                    MixDatabaseParentType.User,
                    MixDatabaseNames.SYSTEM_USER_DATA,
                    Guid.Parse(user.Id));
                var plainText = JsonConvert.SerializeObject(
                    token, 
                    new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
                var encryptedInfo = AesEncryptionHelper.EncryptString(plainText, aesKey);

                var resp = new JObject()
                        {
                            new JProperty(MixEncryptKeywords.AESKey, aesKey),
                            new JProperty(MixEncryptKeywords.RSAKey, rsaKeys[MixConstants.CONST_RSA_PRIVATE_KEY]),
                            new JProperty(MixEncryptKeywords.Message, encryptedInfo)
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
                var dtExpired = dtIssued.AddMinutes(_authConfigService.AppSettings.AccessTokenExpiration);
                var dtRefreshTokenExpired = dtIssued.AddMinutes(_authConfigService.AppSettings.RefreshTokenExpiration);
                var refreshTokenId = Guid.Empty;
                var refreshToken = Guid.Empty;
                if (isRemember)
                {
                    refreshToken = Guid.NewGuid();
                    RefreshTokenViewModel vmRefreshToken = new(
                                new RefreshTokens()
                                {
                                    Id = refreshToken,
                                    Email = user.Email,
                                    IssuedUtc = dtIssued,
                                    ClientId = _authConfigService.AppSettings.ClientId,
                                    Username = user.UserName,
                                    ExpiresUtc = dtRefreshTokenExpired
                                }, _uow);

                    var saveRefreshTokenResult = await vmRefreshToken.SaveAsync();
                    refreshTokenId = saveRefreshTokenResult;
                }

                AccessTokenViewModel token = new()
                {
                    AccessToken = await GenerateTokenAsync(
                        user, dtExpired, refreshToken.ToString(), aesKey, rsaPublicKey, _authConfigService.AppSettings),
                    RefreshToken = refreshTokenId,
                    TokenType = _authConfigService.AppSettings.TokenType,
                    ExpiresIn = _authConfigService.AppSettings.AccessTokenExpiration,
                    Issued = dtIssued,
                    Expires = dtExpired,
                    LastUpdateConfiguration = _globalConfigService.AppSettings.LastUpdateConfiguration
                };
                return token;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public async Task<JObject> ExternalLogin(RegisterExternalBindingModel model)
        {
            var verifiedAccessToken = await VerifyExternalAccessToken(model.Provider, model.ExternalAccessToken, _authConfigService.AppSettings);
            if (verifiedAccessToken != null)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                // return local token if already register
                if (user != null)
                {
                    return await GetAuthData(_context, user, true);
                }
                else if (!string.IsNullOrEmpty(model.Email))// register new account
                {
                    user = new MixUser()
                    {
                        Email = model.Email,
                        UserName = model.Email
                    };
                    await _userManager.CreateAsync(user);
                    return await GetAuthData(_context, user, true);
                }
                else
                {
                    throw new MixException(MixErrorStatus.Badrequest, "Login Failed");
                }
            }
            return default;
        }

        public async Task<JObject> RenewTokenAsync(RenewTokenDto refreshTokenDto)
        {
            JObject result = new();
            var oldToken = await _refreshTokenRepo.GetSingleAsync(t => t.Id == refreshTokenDto.RefreshToken);
            if (oldToken != null)
            {
                if (oldToken.ExpiresUtc > DateTime.UtcNow)
                {

                    var principle = GetPrincipalFromExpiredToken(refreshTokenDto.AccessToken, _authConfigService.AppSettings);
                    if (principle != null && oldToken.Username == GetClaim(principle, MixClaims.Username))
                    {
                        var user = await _userManager.FindByEmailAsync(oldToken.Email);
                        await _signInManager.SignInAsync(user, true).ConfigureAwait(false);

                        var token = await GetAuthData(_context, user, true);
                        if (token != null)
                        {
                            await oldToken.DeleteAsync();
                            result = token;
                        }
                    }
                    else
                    {
                        throw new MixException(MixErrorStatus.Badrequest, "Invalid Token");
                    }

                    return result;
                }
                else
                {
                    await oldToken.DeleteAsync();
                    throw new MixException(MixErrorStatus.Badrequest, "Token expired");
                }
            }
            else
            {
                throw new MixException(MixErrorStatus.Badrequest, "Token expired");
            }
        }

        public bool CheckEndpointPermission(ClaimsPrincipal user, PathString path, string method)
        {
            return true;

            // TODO: 
            //var roles = _idHelper.GetClaims(user, MixClaims.Role);
            //if (roles.Any(r => r == MixDefaultRoles.SuperAdmin || r == MixDefaultRoles.Admin))
            //{
            //    return true;
            //}
            //var endpoint = $"{method}-{path}";
            //var role = Roles.Find(r => r.Name == roles.First());
            //return role.MixPermissions.Any(
            //        p => p.Property<JArray>("endpoints")
            //                .Any(e => new Regex(e["endpoint"].Value<string>()).Match(path).Success
            //                        && e["method"].Value<string>() == method.ToUpper())
            //        );
        }

        public async Task<ParsedExternalAccessToken> VerifyExternalAccessToken(
            MixExternalLoginProviders provider,
            string accessToken,
            MixAuthenticationConfigurations appConfigs)
        {
            ParsedExternalAccessToken parsedToken = null;

            string verifyTokenEndPoint;
            switch (provider)
            {
                case MixExternalLoginProviders.Facebook:
                    //You can get it from here: https://developers.facebook.com/tools/accesstoken/
                    //More about debug_tokn here: http://stackoverflow.com/questions/16641083/how-does-one-get-the-app-access-token-for-debug-token-inspection-on-facebook

                    var appToken = $"{appConfigs.Facebook.AppId}|{appConfigs.Facebook.AppSecret}";
                    verifyTokenEndPoint = string.Format("https://graph.facebook.com/debug_token?input_token={0}&access_token={1}", accessToken, appToken);
                    break;
                case MixExternalLoginProviders.Google:
                    verifyTokenEndPoint = string.Format("https://www.googleapis.com/oauth2/v1/tokeninfo?access_token={0}", accessToken);
                    break;
                case MixExternalLoginProviders.Twitter:
                case MixExternalLoginProviders.Microsoft:
                default:
                    return null;
            }

            var client = new HttpClient();
            var uri = new Uri(verifyTokenEndPoint);
            var response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                dynamic jObj = JObject.Parse(content);

                parsedToken = new ParsedExternalAccessToken();

                if (provider == MixExternalLoginProviders.Facebook)
                {
                    parsedToken.user_id = jObj["data"]["user_id"];
                    parsedToken.app_id = jObj["data"]["app_id"];

                    if (!string.Equals(appConfigs.Facebook.AppId, parsedToken.app_id, StringComparison.OrdinalIgnoreCase))
                    {
                        return null;
                    }
                }
                else if (provider == MixExternalLoginProviders.Google)
                {
                    parsedToken.user_id = jObj["user_id"];
                    parsedToken.app_id = jObj["audience"];

                    if (!string.Equals(appConfigs.Google.AppId, parsedToken.app_id, StringComparison.OrdinalIgnoreCase))
                    {
                        return null;
                    }

                }

            }

            return parsedToken;
        }

        public async Task<string> GenerateTokenAsync(
            MixUser user,
            DateTime expires,
            string refreshToken,
            string aesKey,
            string rsaPublicKey,
            MixAuthenticationConfigurations appConfigs)
        {
            List<Claim> claims = await GetClaimsAsync(user);
            claims.AddRange(new[]
                {
                    new Claim(MixClaims.Id, user.Id.ToString()),
                    new Claim(MixClaims.Username, user.UserName),
                    new Claim(MixClaims.RefreshToken, refreshToken),
                    new Claim(MixClaims.AESKey, aesKey),
                    new Claim(MixClaims.RSAPublicKey, rsaPublicKey)
                });

            JwtSecurityToken jwtSecurityToken = new(
                issuer: appConfigs.Issuer,
                audience: appConfigs.Audience,
                claims: claims,
                notBefore: expires.AddMinutes(-appConfigs.AccessTokenExpiration),
                expires: expires,
                signingCredentials: new SigningCredentials(JwtSecurityKey.Create(appConfigs.SecretKey), SecurityAlgorithms.HmacSha256));
            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }

        public async Task<List<Claim>> GetClaimsAsync(MixUser user)
        {
            List<Claim> claims = new();
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var claim in user.Claims)
            {
                claims.Add(CreateClaim(claim.ClaimType, claim.ClaimValue));
            }

            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
                var role = await _roleManager.FindByNameAsync(userRole);
                if (role != null)
                {
                    var roleClaims = await _roleManager.GetClaimsAsync(role);
                    foreach (Claim roleClaim in roleClaims)
                    {
                        claims.Add(roleClaim);
                    }
                }
            }
            return claims;
        }

        public static Claim CreateClaim(string type, string value)
        {
            return new Claim(type, value, ClaimValueTypes.String);
        }

        public static string GetClaim(ClaimsPrincipal User, string claimType)
        {
            return User.Claims.FirstOrDefault(c => c.Type == claimType)?.Value;
        }

        public static IEnumerable<string> GetClaims(ClaimsPrincipal User, string claimType)
        {
            return User.Claims.Where(c => c.Type == claimType).Select(c => c.Value);
        }

        public static ClaimsPrincipal GetPrincipalFromExpiredToken(string token, MixAuthenticationConfigurations appConfigs)
        {
            var tokenValidationParameters = GetValidationParameters(appConfigs, false);
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                return null;

            return principal;
        }

        public static TokenValidationParameters GetValidationParameters(MixAuthenticationConfigurations appConfigs, bool validateLifetime)
        {
            return new TokenValidationParameters
            {
                ClockSkew = TimeSpan.Zero,
                ValidateIssuer = appConfigs.ValidateIssuer,
                ValidateAudience = appConfigs.ValidateAudience,
                ValidateLifetime = validateLifetime,
                ValidateIssuerSigningKey = appConfigs.ValidateIssuerSigningKey,
                IssuerSigningKey = JwtSecurityKey.Create(appConfigs.SecretKey)
            };
        }

        public static class JwtSecurityKey
        {
            public static SymmetricSecurityKey Create(string secret)
            {
                return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
            }
        }

        private void LoadRoles()
        {
            if (!_globalConfigService.IsInit)
            {
                Roles = _roleRepo.GetListAsync(m => true).GetAwaiter().GetResult();
                //using var ctx = new MixCmsContext();
                //var transaction = ctx.Database.BeginTransaction();
                // TODO:
                //Roles.ForEach(m => m.LoadMixPermissions(ctx, transaction).GetAwaiter().GetResult());
            }
        }
    }
}
