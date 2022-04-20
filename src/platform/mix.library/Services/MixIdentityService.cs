using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Mix.Communicator.Services;
using Mix.Database.Entities.Account;
using Mix.Identity.Constants;
using Mix.Identity.Domain.Models;
using Mix.Identity.Dtos;
using Mix.Identity.Enums;
using Mix.Identity.Models.AccountViewModels;
using Mix.Identity.ViewModels;
using Mix.Lib.Dtos;
using Mix.Lib.Models;
using Mix.Shared.Models;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace Mix.Lib.Services
{
    public class MixIdentityService
    {
        private readonly UnitOfWorkInfo _accountUow;
        private readonly UnitOfWorkInfo _cmsUow;
        private readonly MixCacheService _cacheService;
        private readonly TenantUserManager _userManager;
        private readonly SignInManager<MixUser> _signInManager;
        private readonly RoleManager<MixRole> _roleManager;
        private readonly AuthConfigService _authConfigService;
        private readonly FirebaseService _firebaseService;
        private readonly MixDataService _mixDataService;
        private readonly MixCmsContext _cmsSontext;
        private readonly Repository<MixCmsAccountContext, AspNetRoles, Guid, RoleViewModel> _roleRepo;
        private readonly Repository<MixCmsAccountContext, RefreshTokens, Guid, RefreshTokenViewModel> _refreshTokenRepo;
        public List<RoleViewModel> Roles { get; set; }
        protected int MixTenantId { get; set; } = 1;
        public MixIdentityService(
            IHttpContextAccessor httpContextAccessor,
            TenantUserManager userManager,
            SignInManager<MixUser> signInManager,
            RoleManager<MixRole> roleManager,
            AuthConfigService authConfigService,
            MixCmsContext cmsContext,
            MixCmsAccountContext accountContext,
            MixCacheService cacheService,
            FirebaseService firebaseService, MixDataService mixDataService)
        {
            _cmsSontext = cmsContext;
            _cacheService = cacheService;
            _accountUow = new(accountContext);
            _cmsUow = new(cmsContext);
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _authConfigService = authConfigService;
            _roleRepo = RoleViewModel.GetRepository(_accountUow);
            _refreshTokenRepo = RefreshTokenViewModel.GetRepository(_accountUow);
            _firebaseService = firebaseService;

            if (httpContextAccessor.HttpContext != null && httpContextAccessor.HttpContext.Session.GetInt32(MixRequestQueryKeywords.MixTenantId).HasValue)
            {
                MixTenantId = httpContextAccessor.HttpContext.Session.GetInt32(MixRequestQueryKeywords.MixTenantId).Value;
            }
            _mixDataService = mixDataService;
            _mixDataService.SetUnitOfWork(_cmsUow);
        }

        public async Task<JObject> Login(LoginViewModel model)
        {
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true

            MixUser user = null;
            if (!string.IsNullOrEmpty(model.Email))
            {
                user = await _userManager.FindByEmailAsync(model.Email);
            }
            if (!string.IsNullOrEmpty(model.UserName))
            {
                user = await _userManager.FindByNameAsync(model.UserName);
            }
            if (!string.IsNullOrEmpty(model.PhoneNumber))
            {
                user = await _userManager.FindByPhoneNumberAsync(model.PhoneNumber);
            }

            if (user == null)
            {
                throw new MixException(MixErrorStatus.Badrequest, "Login failed");
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, isPersistent: model.RememberMe, lockoutOnFailure: true).ConfigureAwait(false);

            if (result.IsLockedOut)
            {
                throw new MixException(MixErrorStatus.Badrequest, "This account has been locked out, please try again later.");
            }

            if (result.Succeeded)
            {
                return await GetAuthData(_cmsSontext, user, model.RememberMe, MixTenantId);
            }
            else
            {
                throw new MixException(MixErrorStatus.Badrequest, "Login failed");
            }

        }

        public async Task<JObject> GetAuthData(MixCmsContext context, MixUser user, bool rememberMe, int tenantId)
        {
            var rsaKeys = RSAEncryptionHelper.GenerateKeys();
            var aesKey = GlobalConfigService.Instance.AesKey;  //AesEncryptionHelper.GenerateCombinedKeys();

            var userInfo = new MixUserViewModel(user, _cmsUow);
            await userInfo.LoadUserDataAsync(tenantId, _mixDataService);
            await _cmsUow.CompleteAsync();
            var token = await GenerateAccessTokenAsync(user, userInfo, rememberMe, aesKey, rsaKeys[MixConstants.CONST_RSA_PUBLIC_KEY]);
            if (token != null)
            {
                await _accountUow.CompleteAsync();
                var data = ReflectionHelper.ParseObject(token);
                if (GlobalConfigService.Instance.IsEncryptApi)
                {
                    var encryptedInfo = AesEncryptionHelper.EncryptString(data.ToString(Formatting.None), aesKey);

                    var resp = new JObject()
                            {
                                //new JProperty(MixEncryptKeywords.AESKey, aesKey),
                                //new JProperty(MixEncryptKeywords.RSAKey, rsaKeys[MixConstants.CONST_RSA_PRIVATE_KEY]),
                                new JProperty(MixEncryptKeywords.Message, encryptedInfo)
                            };
                    return resp;
                }
                else
                {
                    return data;
                }
            }
            return default;
        }

        public async Task<JObject> GetToken(GetTokenModel model)
        {
            MixUser user = null;
            if (!string.IsNullOrEmpty(model.Email))
            {
                user = await _userManager.FindByEmailAsync(model.Email);
            }
            else if (!string.IsNullOrEmpty(model.PhoneNumber))
            {
                user = await _userManager.FindByPhoneNumberAsync(model.PhoneNumber);
            }

            if (user != null)
            {
                return await GetAuthData(_cmsSontext, user, true, MixTenantId);
            }
            return default;
        }

        public async Task<JObject> Register(RegisterViewModel model, int tenantId, UnitOfWorkInfo _cmsUOW)
        {
            var user = new MixUser
            {
                Id = Guid.NewGuid(),
                UserName = model.UserName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                CreatedDateTime = DateTime.UtcNow
            };

            var createResult = await _userManager.CreateAsync(user, password: model.Password).ConfigureAwait(false);
            if (createResult.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, MixRoleEnums.Guest.ToString(), tenantId);
                await _userManager.AddToTenant(user, tenantId);

                user = await _userManager.FindByNameAsync(model.UserName).ConfigureAwait(false);
                var vm = new MixUserViewModel(user, _cmsUOW);
                await vm.LoadUserDataAsync(MixTenantId, _mixDataService);
                vm.UserData.Data = model.Data;
                await vm.UserData.SaveAsync();
                return await GetAuthData(_cmsSontext, user, true, tenantId);
            }
            throw new MixException(MixErrorStatus.Badrequest, createResult.Errors.First().Description);
        }

        public async Task<AccessTokenViewModel> GenerateAccessTokenAsync(
            MixUser user,
            MixUserViewModel info,
            bool isRemember,
            string aesKey,
            string rsaPublicKey)
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
                                }, _accountUow);

                    var saveRefreshTokenResult = await vmRefreshToken.SaveAsync();
                    refreshTokenId = saveRefreshTokenResult;
                }

                AccessTokenViewModel token = new()
                {
                    Info = info,
                    AccessToken = await GenerateTokenAsync(
                        user, info, dtExpired, refreshToken.ToString(), aesKey, rsaPublicKey, _authConfigService.AppSettings),
                    RefreshToken = refreshTokenId,
                    TokenType = _authConfigService.AppSettings.TokenType,
                    ExpiresIn = _authConfigService.AppSettings.AccessTokenExpiration,
                    Issued = dtIssued,
                    Expires = dtExpired,
                    LastUpdateConfiguration = GlobalConfigService.Instance.AppSettings.LastUpdateConfiguration
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
                MixUser user = null;
                if (!string.IsNullOrEmpty(model.Email))
                {
                    user = await _userManager.FindByEmailAsync(model.Email);
                }
                if (!string.IsNullOrEmpty(model.UserName))
                {
                    user = await _userManager.FindByNameAsync(model.UserName);
                }
                if (!string.IsNullOrEmpty(model.PhoneNumber))
                {
                    user = await _userManager.FindByPhoneNumberAsync(model.PhoneNumber);
                }

                // return local token if already register
                if (user != null)
                {
                    return await GetAuthData(_cmsSontext, user, true, MixTenantId);
                }
                // register new account
                else if (!string.IsNullOrEmpty(model.Email))
                {
                    user = new MixUser()
                    {
                        Email = model.Email,
                        PhoneNumber = model.PhoneNumber,
                        UserName = model.Email ?? model.PhoneNumber,
                    };
                    await _userManager.CreateAsync(user);
                    return await GetAuthData(_cmsSontext, user, true, MixTenantId);
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

                        var token = await GetAuthData(_cmsSontext, user, true, MixTenantId);
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
            var endpoints = GetClaims(user, MixClaims.Endpoints);

            // Pattern: "[method] - [regex]"
            // Example: "post - /api/v1/rest/.+/module-data/portal$"
            string currentEndpoint = $"{method.ToLower()} - {path.ToString().ToLower()}";
            return endpoints.Any(
                    e => new Regex(e.ToLower()).Match(currentEndpoint).Success);
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
                case MixExternalLoginProviders.Firebase:
                    var token = await _firebaseService.VeriryTokenAsync(accessToken);
                    return new ParsedExternalAccessToken()
                    {
                        app_id = token.TenantId,
                        user_id = token.Uid
                    };
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
            MixUserViewModel info,
            DateTime expires,
            string refreshToken,
            string aesKey,
            string rsaPublicKey,
            MixAuthenticationConfigurations appConfigs)
        {
            var userRoles = await _userManager.GetUserRolesAsync(user);
            List<Claim> claims = await GetClaimsAsync(user, userRoles);
           
            foreach (var endpoint in info.Endpoints)
            {
                claims.Add(CreateClaim(MixClaims.Endpoints, endpoint));
            }
            claims.AddRange(new[]
                {
                    CreateClaim(MixClaims.Id, user.Id.ToString()),
                    CreateClaim(MixClaims.Username, user.UserName),
                    CreateClaim(MixClaims.RefreshToken, refreshToken),
                    CreateClaim(MixClaims.AESKey, aesKey),
                    CreateClaim(MixClaims.RSAPublicKey, rsaPublicKey),
                    CreateClaim(MixClaims.ExpireAt, expires.ToString())
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

        
        private async Task<List<Claim>> GetClaimsAsync(MixUser user, IList<MixRole> userRoles)
        {
            List<Claim> claims = new();
            foreach (var claim in user.Claims)
            {
                claims.Add(CreateClaim(claim.ClaimType, claim.ClaimValue));
            }

            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole.Name));
                var role = await _roleManager.FindByNameAsync(userRole.Name);
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

        public Claim CreateClaim(string type, string value)
        {
            return new Claim(type, value, ClaimValueTypes.String);
        }

        public string GetClaim(ClaimsPrincipal User, string claimType)
        {
            return string.Join(',', User.Claims.Where(c => c.Type == claimType).Select(m => m.Value));
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
    }
}
