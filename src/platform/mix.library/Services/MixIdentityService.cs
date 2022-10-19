using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.Amqp.Framing;
using Microsoft.IdentityModel.Tokens;
using Mix.Communicator.Services;
using Mix.Database.Entities.Account;
using Mix.Identity.Constants;
using Mix.Identity.Domain.Models;
using Mix.Identity.Dtos;
using Mix.Identity.Enums;
using Mix.Identity.Models.AccountViewModels;
using Mix.Identity.ViewModels;
using Mix.Lib.Extensions;
using Mix.Lib.Interfaces;
using Mix.Lib.Models;
using Mix.RepoDb.Repositories;
using Mix.Shared.Models.Configurations;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace Mix.Lib.Services
{
    public class MixIdentityService : IMixIdentityService
    {
        private const string tenantIdFieldName = "MixTenantId";
        protected readonly UnitOfWorkInfo _accountUow;
        protected readonly UnitOfWorkInfo _cmsUow;
        protected readonly MixCacheService _cacheService;
        protected readonly TenantUserManager _userManager;
        protected readonly SignInManager<MixUser> _signInManager;
        protected readonly RoleManager<MixRole> _roleManager;
        protected readonly AuthConfigService _authConfigService;
        protected readonly FirebaseService _firebaseService;
        protected readonly MixService _mixService;
        protected readonly MixRepoDbRepository _repoDbRepository;
        protected readonly MixCmsContext _cmsContext;
        protected readonly Repository<MixCmsAccountContext, MixRole, Guid, RoleViewModel> _roleRepo;
        protected readonly Repository<MixCmsAccountContext, RefreshTokens, Guid, RefreshTokenViewModel> _refreshTokenRepo;
        public List<RoleViewModel> Roles { get; set; }
        protected ISession _session;
        private MixTenantSystemViewModel _currentTenant;
        public MixTenantSystemViewModel CurrentTenant
        {
            get
            {
                if (_currentTenant == null)
                {
                    _currentTenant = _session.Get<MixTenantSystemViewModel>(MixRequestQueryKeywords.Tenant);
                }
                return _currentTenant;
            }
        }
        public MixIdentityService(
            IHttpContextAccessor httpContextAccessor,
            TenantUserManager userManager,
            SignInManager<MixUser> signInManager,
            RoleManager<MixRole> roleManager,
            AuthConfigService authConfigService,
            UnitOfWorkInfo<MixCmsContext> cmsUOW,
            UnitOfWorkInfo<MixCmsAccountContext> accountUOW,
            MixCacheService cacheService,
            FirebaseService firebaseService, MixRepoDbRepository repoDbRepository,
            MixService mixService)
        {
            _session = httpContextAccessor.HttpContext.Session;
            _cmsUow = cmsUOW;
            _cmsContext = cmsUOW.DbContext;
            _cacheService = cacheService;
            _accountUow = accountUOW;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _authConfigService = authConfigService;
            _roleRepo = RoleViewModel.GetRepository(_accountUow);
            _refreshTokenRepo = RefreshTokenViewModel.GetRepository(_accountUow);
            _firebaseService = firebaseService;
            _repoDbRepository = repoDbRepository;
            _mixService = mixService;
        }

        public virtual async Task<MixUserViewModel> GetUserAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user != null)
            {
                var userInfo = new MixUserViewModel(user, _cmsUow);
                await userInfo.LoadUserDataAsync(CurrentTenant.Id, _repoDbRepository);
                return userInfo;
            }
            return null;
        }
        public virtual async Task<JObject> Login(LoginViewModel model)
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
                return await GetAuthData(user, model.RememberMe, CurrentTenant.Id);
            }
            else
            {
                throw new MixException(MixErrorStatus.Badrequest, "Login failed");
            }

        }

        public virtual async Task<JObject> GetAuthData(MixUser user, bool rememberMe, int tenantId)
        {
            var rsaKeys = RSAEncryptionHelper.GenerateKeys();
            var aesKey = GlobalConfigService.Instance.AesKey;  //AesEncryptionHelper.GenerateCombinedKeys();

            var userInfo = new MixUserViewModel(user, _cmsUow);
            await userInfo.LoadUserDataAsync(tenantId, _repoDbRepository);
            var token = await GenerateAccessTokenAsync(user, userInfo, rememberMe, aesKey, rsaKeys[MixConstants.CONST_RSA_PUBLIC_KEY]);
            if (token != null)
            {
                var data = ReflectionHelper.ParseObject(token);
                if (CurrentTenant.Configurations.IsEncryptApi)
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

        public virtual async Task<JObject> GetToken(GetTokenModel model)
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
                return await GetAuthData(user, true, CurrentTenant.Id);
            }
            return default;
        }

        public virtual async Task<MixUser> Register(RegisterViewModel model, int tenantId, UnitOfWorkInfo _cmsUOW)
        {
            var user = new MixUser
            {
                Id = Guid.NewGuid(),
                UserName = model.UserName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                CreatedDateTime = DateTime.UtcNow
            };

            var createResult = !string.IsNullOrEmpty(model.Password)
                ? await _userManager.CreateAsync(user, password: model.Password).ConfigureAwait(false)
                : await _userManager.CreateAsync(user).ConfigureAwait(false);
            if (createResult.Succeeded)
            {
                if (model.Provider.HasValue && !string.IsNullOrEmpty(model.ProviderKey))
                {
                    var createLoginResult = await _userManager.AddLoginAsync(
                        user,
                        new UserLoginInfo(model.Provider.ToString(), model.ProviderKey, model.Provider.ToString()));
                    if (!createLoginResult.Succeeded)
                    {
                        throw new MixException(MixErrorStatus.Badrequest, createLoginResult.Errors.First().Description);
                    }
                }
                await _userManager.AddToRoleAsync(user, MixRoleEnums.Guest.ToString(), tenantId);
                await _userManager.AddToTenant(user, tenantId);

                user = await _userManager.FindByNameAsync(model.UserName).ConfigureAwait(false);
                await CreateUserData(user, model.Data);
                return user;
                //return await GetAuthData(_cmsContext, user, true, tenantId);
            }
            throw new MixException(MixErrorStatus.Badrequest, createResult.Errors.First().Description);
        }

        public virtual async Task CreateUserData(MixUser user, JObject obj)
        {
            try
            {
                if (obj != null)
                {
                    MixDatabaseViewModel database = await MixDatabaseViewModel.GetRepository(_cmsUow)
                        .GetSingleAsync(m => m.SystemName == MixDatabaseNames.SYSTEM_USER_DATA);
                    var data = new JObject(obj.Properties().Where(m => database.Columns.Any(c => c.SystemName == m.Name)));
                    int userDataId = await CreateUserInfomation(user, data);
                    foreach (var relation in database.Relationships)
                    {
                        if (obj.ContainsKey(relation.DisplayName))
                        {
                            var nestedData = obj.Value<JArray>(relation.DisplayName);
                            await CreateNestedData(relation.DestinateDatabaseName, nestedData, userDataId);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MixService.LogException(ex);
            }
        }

        private async Task CreateNestedData(string databaseName, JArray nestedData, int userDataId)
        {
            _repoDbRepository.Init(databaseName);
            foreach (JObject data in nestedData)
            {
                if (!data.ContainsKey(tenantIdFieldName))
                {
                    data.Add(new JProperty(tenantIdFieldName, CurrentTenant.Id));
                }
                if (!data.ContainsKey("createdDateTime"))
                {
                    data.Add(new JProperty("createdDateTime", DateTime.UtcNow));
                }

                var id = await _repoDbRepository.InsertAsync(data);
                MixDatabaseAssociationViewModel association = new(_cmsUow)
                {
                    MixTenantId = CurrentTenant.Id,
                    ParentDatabaseName = MixDatabaseNames.SYSTEM_USER_DATA,
                    ChildDatabaseName = databaseName,
                    ParentId = userDataId,
                    ChildId = id,
                };
                await association.SaveAsync();
            }
        }

        private async Task<int> CreateUserInfomation(MixUser user, JObject data)
        {
            _repoDbRepository.Init(MixDatabaseNames.SYSTEM_USER_DATA);
            if (!data.ContainsKey(tenantIdFieldName))
            {
                data.Add(new JProperty(tenantIdFieldName, CurrentTenant.Id));
            }
            if (!data.ContainsKey("createdDateTime"))
            {
                data.Add(new JProperty("createdDateTime", DateTime.UtcNow));
            }
            if (!data.ContainsKey("parentId"))
            {
                data.Add(new JProperty("parentId", user.Id));
            }
            if (!data.ContainsKey("parentType"))
            {
                data.Add(new JProperty("parentType", MixContentType.User.ToString()));
            }
            return await _repoDbRepository.InsertAsync(data);
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
                    LastUpdateConfiguration = CurrentTenant.Configurations.LastUpdateConfiguration
                };
                return token;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public virtual async Task<JObject> ExternalLogin(RegisterExternalBindingModel model)
        {
            var verifiedAccessToken = await VerifyExternalAccessToken(model.Provider, model.ExternalAccessToken, _authConfigService.AppSettings);

            if (verifiedAccessToken != null)
            {

                MixUser user = await _userManager.FindByLoginAsync(
                    model.Provider.ToString(),
                    verifiedAccessToken.user_id);
                // return local token if already register
                if (user != null)
                {
                    return await GetAuthData(user, true, CurrentTenant.Id);
                }
                // register new account
                else
                {
                    string userName = model.UserName ?? model.Email ?? model.PhoneNumber;

                    if (!string.IsNullOrEmpty(userName))
                    {
                        user = await Register(new RegisterViewModel()
                        {
                            Email = model.Email,
                            PhoneNumber = model.PhoneNumber,
                            UserName = userName,
                            Provider = model.Provider,
                            ProviderKey = verifiedAccessToken.user_id,
                            Data = model.Data
                        }, CurrentTenant.Id, _cmsUow);

                        return await GetAuthData(user, true, CurrentTenant.Id);
                    }
                    else
                    {
                        throw new MixException(MixErrorStatus.Badrequest, "Login Failed");
                    }
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

                        var token = await GetAuthData(user, true, CurrentTenant.Id);
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
            string verifyTokenEndPoint = string.Empty;

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
                    CreateClaim(MixClaims.Avatar, info.UserData?.Value<string>("avatar") ?? MixConstants.CONST_DEFAULT_AVATAR),
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
                var roleName = userRole.Name == MixRoles.SuperAdmin ? userRole.Name : $"{userRole.Name}-{CurrentTenant.Id}";
                claims.Add(new Claim(ClaimTypes.Role, roleName));
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
