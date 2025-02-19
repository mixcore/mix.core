using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Mix.Auth.Constants;
using Mix.Auth.Dtos;
using Mix.Auth.Enums;
using Mix.Communicator.Services;
using Mix.Database.Entities.Account;
using Mix.Database.Entities.MixDb;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Identity.Domain.Models;
using Mix.Identity.Enums;
using Mix.Identity.ViewModels;
using Mix.Lib.Extensions;
using Mix.Lib.Interfaces;
using Mix.Mixdb.Interfaces;
using Mix.Mixdb.Services;
using Mix.Mixdb.ViewModels;
using Mix.Mq.Lib.Models;
using Mix.RepoDb.Repositories;
using Mix.Service.Commands;
using Mix.Shared.Models.Configurations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace Mix.Lib.Services
{
    public class MixIdentityService : IMixIdentityService
    {
        protected const string TenantIdFieldName = "TenantId";
        protected const string datetimeFormat = "yyyy-MM-ddTHH:mm:ss.FFFZ";
        protected readonly IMemoryQueueService<MessageQueueModel> QueueService;
        protected IConfiguration Configuration;
        protected readonly UnitOfWorkInfo<MixCmsAccountContext> AccountUow;
        protected readonly UnitOfWorkInfo<MixCmsContext> CmsUow;
        protected readonly UnitOfWorkInfo<MixDbDbContext> MixDbUow;
        protected readonly MixCacheService CacheService;
        protected readonly TenantUserManager UserManager;
        protected readonly SignInManager<MixUser> SignInManager;
        protected readonly RoleManager<MixRole> RoleManager;
        protected readonly AuthConfigService AuthConfigService;
        protected readonly GlobalSettingsModel GlobalConfig;
        protected readonly FirebaseService FirebaseService;
        protected readonly IMixDbDataService MixDbDataService;
        protected readonly RepoDbRepository RepoDbRepository;
        protected readonly MixCmsContext CmsContext;
        private readonly MixCmsAccountContext _accContext;
        protected readonly Repository<MixCmsAccountContext, MixRole, Guid, RoleViewModel> RoleRepo;
        protected readonly Repository<MixCmsAccountContext, RefreshTokens, Guid, RefreshTokenViewModel> RefreshTokenRepo;
        protected readonly DatabaseService DatabaseService;
        public List<RoleViewModel> Roles { get; set; }
        protected ISession Session;
        private MixTenantSystemModel _currentTenant;
        public MixTenantSystemModel CurrentTenant
        {
            get
            {
                if (_currentTenant == null)
                {
                    _currentTenant = Session?.Get<MixTenantSystemModel>(MixRequestQueryKeywords.Tenant);
                    _currentTenant ??= new MixTenantSystemModel()
                    {
                        Id = 1
                    };
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
            UnitOfWorkInfo<MixCmsContext> cmsUow,
            UnitOfWorkInfo<MixCmsAccountContext> accountUow,
            MixCacheService cacheService,
            FirebaseService firebaseService,
            DatabaseService databaseService,
            MixCmsAccountContext accContext,
            UnitOfWorkInfo<MixDbDbContext> mixDbUow,
            IConfiguration configuration,
            IMemoryQueueService<MessageQueueModel> queueService,
            MixDbDataServiceFactory mixDbDataFactory)
        {
            Session = httpContextAccessor.HttpContext?.Session;
            Configuration = configuration;
            CmsUow = cmsUow;
            CmsContext = cmsUow.DbContext;
            CacheService = cacheService;
            AccountUow = accountUow;
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;
            AuthConfigService = authConfigService;
            RoleRepo = RoleViewModel.GetRepository(AccountUow, CacheService);
            RefreshTokenRepo = RefreshTokenViewModel.GetRepository(AccountUow, CacheService);
            FirebaseService = firebaseService;
            DatabaseService = databaseService;
            _accContext = accContext;
            MixDbUow = mixDbUow;
            MixDbDataService = mixDbDataFactory.GetDataService(databaseService.DatabaseProvider, databaseService.GetConnectionString(MixConstants.CONST_ACCOUNT_CONNECTION));
            GlobalConfig = Configuration.GetSection(MixAppSettingsSection.GlobalSettings).Get<GlobalSettingsModel>();
            QueueService = queueService;
        }

        public virtual async Task<bool> Any(Guid userId)
        {
            var user = await UserManager.FindByIdAsync(userId.ToString());
            return user != null;
        }

        public virtual async Task<MixUserViewModel> GetUserAsync(Guid userId, CancellationToken cancellationToken)
        {
            var user = await UserManager.FindByIdAsync(userId.ToString());
            if (user != null)
            {
                var userInfo = new MixUserViewModel(user, CmsUow);
                var roles = await UserManager.GetRolesAsync(user);
                if (!Configuration.IsInit())
                {
                    await userInfo.LoadUserDataAsync(CurrentTenant.Id, MixDbDataService, _accContext, CacheService, cancellationToken);
                }
                await userInfo.LoadUserPortalMenus(roles.ToArray(), CurrentTenant.Id, RepoDbRepository);

                return userInfo;
            }
            return null;
        }

        public virtual async Task<TokenResponseModel> LoginAsync(LoginRequestModel model, CancellationToken cancellationToken = default)
        {
            try
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                MixUser user = null;
                if (!string.IsNullOrEmpty(model.Email))
                {
                    user = await UserManager.FindByEmailAsync(model.Email);
                }
                if (user == null && !string.IsNullOrEmpty(model.UserName))
                {
                    user = await UserManager.FindByNameAsync(model.UserName);
                    user ??= await UserManager.FindByEmailAsync(model.UserName);
                    user ??= await UserManager.FindByPhoneNumberAsync(model.UserName);
                }
                if (user == null && !string.IsNullOrEmpty(model.PhoneNumber))
                {
                    user = await UserManager.FindByPhoneNumberAsync(model.PhoneNumber);
                }

                if (user == null)
                {
                    throw new MixException(MixErrorStatus.Badrequest, "Login failed");
                }

                var result = await SignInManager.PasswordSignInAsync(user, model.Password, isPersistent: model.RememberMe, lockoutOnFailure: true).ConfigureAwait(false);

                if (result.IsLockedOut)
                {
                    throw new MixException(MixErrorStatus.Badrequest, "This account has been locked out, please try again later.");
                }

                if (result.Succeeded)
                {
                    return await GetAuthData(user, model.RememberMe, CurrentTenant.Id, default, cancellationToken);
                }
                else
                {
                    throw new MixException(MixErrorStatus.Badrequest, "Login failed");
                }
            }
            catch (MixException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.ServerError, ex);
            }
        }

        public virtual async Task<TokenResponseModel> GetAuthData(MixUser user, bool rememberMe, int tenantId, JObject additionalData = default, CancellationToken cancellationToken = default)
        {

            return await GenerateAccessTokenAsync(user, rememberMe, additionalData, cancellationToken);
            ////if (token != null)
            ////{
            ////    var data = ReflectionHelper.ParseObject(token);
            ////    if (CurrentTenant.Configurations.IsEncryptApi)
            ////    {
            ////        var encryptedInfo = AesEncryptionHelper.EncryptString(data.ToString(Formatting.None), aesKey);

            ////        var resp = new JObject()
            ////                {
            ////                    //new JProperty(MixEncryptKeywords.AESKey, aesKey),
            ////                    //new JProperty(MixEncryptKeywords.RSAKey, rsaKeys[MixConstants.CONST_RSA_PRIVATE_KEY]),
            ////                    new JProperty(MixEncryptKeywords.Message, encryptedInfo)
            ////                };
            ////        return resp;
            ////    }
            ////    else
            ////    {
            ////        return data;
            ////    }
            ////}
            //return default;
        }

        public virtual async Task<TokenResponseModel> GetTokenAsync(GetTokenModel model, CancellationToken cancellationToken = default)
        {
            MixUser user = null;
            if (!string.IsNullOrEmpty(model.Email))
            {
                user = await UserManager.FindByEmailAsync(model.Email);
            }
            else if (!string.IsNullOrEmpty(model.PhoneNumber))
            {
                user = await UserManager.FindByPhoneNumberAsync(model.PhoneNumber);
            }

            if (user != null)
            {
                return await GetAuthData(user, true, CurrentTenant.Id, default, cancellationToken);
            }
            return default;
        }

        public virtual async Task<MixUser> RegisterAsync(RegisterRequestModel model, int tenantId, UnitOfWorkInfo cmsUow, JObject additionalData = default, CancellationToken cancellationToken = default)
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
                ? await UserManager.CreateAsync(user, password: model.Password).ConfigureAwait(false)
                : await UserManager.CreateAsync(user).ConfigureAwait(false);
            if (createResult.Succeeded)
            {
                if (model.Provider.HasValue && !string.IsNullOrEmpty(model.ProviderKey))
                {
                    var createLoginResult = await UserManager.AddLoginAsync(
                        user,
                        new UserLoginInfo(model.Provider.ToString(), model.ProviderKey, model.Provider.ToString()));
                    if (!createLoginResult.Succeeded)
                    {
                        throw new MixException(MixErrorStatus.Badrequest, createLoginResult.Errors.First().Description);
                    }
                }

                await UserManager.AddToRoleAsync(user, MixRoleEnums.Guest.ToString(), tenantId);
                await UserManager.AddToTenant(user, tenantId);

                user = await UserManager.FindByNameAsync(model.UserName).ConfigureAwait(false);
                await GetOrCreateUserData(user, additionalData, cancellationToken);
                return user;
            }
            throw new MixException(MixErrorStatus.Badrequest, createResult.Errors.First().Description);
        }

        public virtual async Task<JObject> GetOrCreateUserData(MixUser user, JObject additionalData = default, CancellationToken cancellationToken = default)
        {
            try
            {
                if (Configuration.IsInit())
                {
                    return new JObject();
                }

                var u = await MixDbDataService.GetSingleByParentAsync(MixDatabaseNames.SYSTEM_USER_DATA, MixContentType.User, user.Id, string.Empty, cancellationToken);
                if (u == null && !Configuration.IsInit())
                {
                    u = new JObject()
                    {
                        new JProperty("Id", null),
                        new JProperty("ParentId", user.Id),
                        new JProperty("ParentType", MixContentType.User.ToString()),
                        new JProperty("UserName", user.UserName),
                        new JProperty("Email", user.Email),
                        new JProperty("PhoneNumber", user.PhoneNumber),

                    };
                    if (additionalData != null)
                    {
                        u.Add(additionalData.Properties());
                    }

                    var id = await MixDbDataService.CreateAsync(MixDatabaseNames.SYSTEM_USER_DATA, u, user.UserName, cancellationToken);

                    QueueService.PushMemoryQueue(
                    CurrentTenant.Id,
                    MixQueueTopics.MixBackgroundTasks,
                    MixQueueActions.MixDbEvent,
                    new MixDbEventCommand(user.UserName, MixDbCommandQueueAction.POST.ToString(), MixDatabaseNames.SYSTEM_USER_DATA, new Shared.Models.MixDbAuditLogModel()
                    {
                        Id = id,
                        MixDbName = MixDatabaseNames.SYSTEM_USER_DATA,
                        After = u
                    }));
                }
                return u;
            }
            catch (Exception ex)
            {
                await MixLogService.LogExceptionAsync(ex);
                return ReflectionHelper.ParseObject(new
                {
                    ParentId = user?.Id,
                    ParentType = MixDatabaseParentType.User,
                    Username = user?.UserName,
                    Email = user?.Email,
                    PhoneNumber = user?.PhoneNumber
                });
            }
        }

        public virtual async Task<TokenResponseModel> GenerateAccessTokenAsync(
            MixUser user,
            bool isRemember,
            JObject additionalData = default,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var dtIssued = DateTime.UtcNow;
                var dtExpired = dtIssued.AddMinutes(AuthConfigService.AppSettings.AccessTokenExpiration);
                var dtRefreshTokenExpired = dtIssued.AddMinutes(AuthConfigService.AppSettings.RefreshTokenExpiration);
                var refreshTokenId = Guid.Empty;
                var userInfo = await GetOrCreateUserData(user, additionalData, cancellationToken);
                if (isRemember)
                {
                    refreshTokenId = Guid.NewGuid();

                    await CacheService.SetAsync($"tokens:{refreshTokenId}", new RefreshTokens()
                    {
                        Id = refreshTokenId,
                        Email = user.Email,
                        IssuedUtc = dtIssued,
                        ClientId = AuthConfigService.AppSettings.ClientId,
                        UserName = user.UserName,
                        ExpiresUtc = dtRefreshTokenExpired
                    }, TimeSpan.FromMinutes(AuthConfigService.AppSettings.RefreshTokenExpiration));
                }

                var token = new TokenResponseModel()
                {
                    Info = userInfo,
                    EmailConfirmed = user.EmailConfirmed,
                    IsActive = user.IsActive,
                    AccessToken = await GenerateTokenAsync(
                        user, new(), dtExpired, refreshTokenId.ToString(), AuthConfigService.AppSettings),
                    RefreshToken = refreshTokenId,
                    TokenType = AuthConfigService.AppSettings.TokenType,
                    ExpiresIn = AuthConfigService.AppSettings.AccessTokenExpiration,
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

        public virtual async Task<TokenResponseModel> ExternalLogin(RegisterExternalBindingModel model, CancellationToken cancellationToken = default)
        {
            try
            {
                var verifiedAccessToken = await VerifyExternalAccessTokenAsync(model.Provider, model.ExternalAccessToken, AuthConfigService.AppSettings);
                if (verifiedAccessToken != null)
                {

                    var user = await UserManager.FindByLoginAsync(model.Provider.ToString(), verifiedAccessToken.user_id);
                    if (user == null && !string.IsNullOrEmpty(model.Email))
                    {
                        user = await UserManager.FindByEmailAsync(model.Email);
                    }

                    if (user == null && !string.IsNullOrEmpty(model.PhoneNumber))
                    {
                        user = await UserManager.FindByPhoneNumberAsync(model.PhoneNumber);
                    }
                    if (user == null && !string.IsNullOrEmpty(model.UserName))
                    {
                        user = await UserManager.FindByNameAsync(model.UserName);
                    }

                    // return local token if already register
                    if (user != null)
                    {
                        return await GetAuthData(user, true, CurrentTenant.Id, model.Data, cancellationToken);
                    }

                    throw new MixException(MixErrorStatus.Badrequest, "Invalid Account");
                }
                throw new MixException(MixErrorStatus.Badrequest);
            }
            catch (MixException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.ServerError, ex);
            }
        }

        public async Task<TokenResponseModel> RenewTokenAsync(RenewTokenDto refreshTokenDto, CancellationToken cancellationToken = default)
        {
            var result = new TokenResponseModel();
            if (refreshTokenDto.RefreshToken == default(Guid))
            {
                throw new MixException(MixErrorStatus.Badrequest, "Invalid Token");
            }
            string key = $"tokens:{refreshTokenDto.RefreshToken}";
            var oldToken = await CacheService.GetAsync<RefreshTokens>(key, cancellationToken);
            if (oldToken != null)
            {
                await CacheService.RemoveCacheAsync(key);
                if (oldToken.ExpiresUtc > DateTime.UtcNow)
                {

                    var principle = GetPrincipalFromExpiredToken(refreshTokenDto.AccessToken, AuthConfigService.AppSettings);
                    if (principle != null && oldToken.UserName == GetClaim(principle, MixClaims.UserName))
                    {
                        var user = await UserManager.FindByNameAsync(oldToken.UserName);
                        await SignInManager.SignInAsync(user, true).ConfigureAwait(false);
                        return await GetAuthData(user, true, CurrentTenant.Id, default, cancellationToken);
                    }
                    else
                    {                        
                        throw new MixException(MixErrorStatus.Badrequest, "Invalid Token");
                    }
                }
                else
                {
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

        public async Task<ParsedExternalAccessToken> VerifyExternalAccessTokenAsync(
            MixExternalLoginProviders provider,
            string accessToken,
            MixAuthenticationConfigurations appConfigs)
        {
            ParsedExternalAccessToken parsedToken = null;
            string verifyTokenEndPoint;
            switch (provider)
            {
                // You can get it from here: https://developers.facebook.com/tools/accesstoken/
                // More about debug_tokn here: http://stackoverflow.com/questions/16641083/how-does-one-get-the-app-access-token-for-debug-token-inspection-on-facebook
                case MixExternalLoginProviders.Facebook:
                    var appToken = $"{appConfigs.Facebook.AppId}|{appConfigs.Facebook.AppSecret}";
                    verifyTokenEndPoint = string.Format("https://graph.facebook.com/debug_token?input_token={0}&access_token={1}", accessToken, appToken);
                    break;
                case MixExternalLoginProviders.Google:
                    verifyTokenEndPoint = string.Format("https://www.googleapis.com/oauth2/v1/tokeninfo?access_token={0}", accessToken);
                    break;
                case MixExternalLoginProviders.Firebase:
                    var token = await FirebaseService.VerifyTokenAsync(accessToken);
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

        public virtual async Task<string> GenerateTokenAsync(
            MixUser user,
            JObject info,
            DateTime expires,
            string refreshToken,
            MixAuthenticationConfigurations appConfigs)
        {
            var userRoles = await UserManager.GetUserRolesAsync(user);
            List<Claim> claims = await GetClaimsAsync(user, userRoles);
            if (info != null && info.ContainsKey("endpoints"))
            {
                var endpoints = info.Values<JArray>("endpoints");
                foreach (var endpoint in endpoints)
                {
                    claims.Add(CreateClaim(MixClaims.Endpoints, endpoint.Value<string>()));
                }
            }
            claims.AddRange(new[]
                {
                    CreateClaim(MixClaims.Id, user.Id.ToString()),
                    CreateClaim(MixClaims.UserName, user.UserName),
                    CreateClaim(MixClaims.TenantId, CurrentTenant.Id.ToString()),
                    CreateClaim(MixClaims.RefreshToken, refreshToken),
                    CreateClaim(MixClaims.Avatar, info?.Value<string>("avatar") ?? MixConstants.CONST_DEFAULT_AVATAR),
                    CreateClaim(MixClaims.ExpireAt, expires.ToString(datetimeFormat))
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

        public async Task<List<MixRole>> LoadUserRoles(string username)
        {
            try
            {
                var user = await UserManager.FindByNameAsync(username);
                return user != null ? (await UserManager.GetUserRolesAsync(user)).ToList() : default;
            }
            catch (Exception ex)
            {
                await MixLogService.LogExceptionAsync(ex);
                return default;
            }
        }
        protected async Task<List<Claim>> GetClaimsAsync(MixUser user, IList<MixRole> userRoles)
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
                var role = await RoleManager.FindByNameAsync(userRole.Name);
                if (role != null)
                {
                    var roleClaims = await RoleManager.GetClaimsAsync(role);
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
            if (User == null)
            {
                return null;
            }
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
