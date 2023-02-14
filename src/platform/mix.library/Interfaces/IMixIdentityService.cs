using Microsoft.AspNetCore.Http;
using Mix.Database.Entities.Account;
using Mix.Identity.Constants;
using Mix.Identity.Domain.Models;
using Mix.Identity.Dtos;
using Mix.Identity.Models.AccountViewModels;
using Mix.Identity.ViewModels;
using Mix.Lib.Models;
using Mix.Shared.Models.Configurations;
using System.Security.Claims;

namespace Mix.Lib.Interfaces
{
    public interface IMixIdentityService
    {
        public List<RoleViewModel> Roles { get; set; }
        public bool CheckEndpointPermission(ClaimsPrincipal user, PathString path, string method);
        public Claim CreateClaim(string type, string value);
        public Task<JObject> ExternalLogin(RegisterExternalBindingModel model, CancellationToken cancellationToken = default);
        public Task<AccessTokenViewModel> GenerateAccessTokenAsync(MixUser user, bool isRemember, string aesKey, string rsaPublicKey, CancellationToken cancellationToken = default);
        public Task<string> GenerateTokenAsync(MixUser user, JObject info, DateTime expires, string refreshToken, string aesKey, string rsaPublicKey, MixAuthenticationConfigurations appConfigs);
        public Task<JObject> GetAuthData(MixUser user, bool rememberMe, int tenantId, CancellationToken cancellationToken = default);
        public string GetClaim(ClaimsPrincipal User, string claimType);
        public Task<JObject> GetTokenAsync(GetTokenModel model, CancellationToken cancellationToken = default);
        public Task<JObject> LoginAsync(LoginViewModel model, CancellationToken cancellationToken = default);
        public Task<MixUser> RegisterAsync(RegisterViewModel model, int tenantId, UnitOfWorkInfo _cmsUOW, CancellationToken cancellationToken = default);
        public Task<JObject> RenewTokenAsync(RenewTokenDto refreshTokenDto, CancellationToken cancellationToken = default);
        public Task<ParsedExternalAccessToken> VerifyExternalAccessTokenAsync(MixExternalLoginProviders provider, string accessToken, MixAuthenticationConfigurations appConfigs);
    }
}