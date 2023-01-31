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
        List<RoleViewModel> Roles { get; set; }
        bool CheckEndpointPermission(ClaimsPrincipal user, PathString path, string method);
        Claim CreateClaim(string type, string value);
        Task<JObject> ExternalLogin(RegisterExternalBindingModel model, CancellationToken cancellationToken = default);
        Task<AccessTokenViewModel> GenerateAccessTokenAsync(MixUser user, bool isRemember, string aesKey, string rsaPublicKey, CancellationToken cancellationToken = default);
        Task<string> GenerateTokenAsync(MixUser user, MixUserViewModel info, DateTime expires, string refreshToken, string aesKey, string rsaPublicKey, MixAuthenticationConfigurations appConfigs);
        Task<JObject> GetAuthData(MixUser user, bool rememberMe, int tenantId, CancellationToken cancellationToken = default);
        string GetClaim(ClaimsPrincipal User, string claimType);
        Task<JObject> GetTokenAsync(GetTokenModel model, CancellationToken cancellationToken = default);
        Task<JObject> LoginAsync(LoginViewModel model, CancellationToken cancellationToken = default);
        Task<MixUser> RegisterAsync(RegisterViewModel model, int tenantId, UnitOfWorkInfo _cmsUOW, CancellationToken cancellationToken = default);
        Task<JObject> RenewTokenAsync(RenewTokenDto refreshTokenDto, CancellationToken cancellationToken = default);
        Task<ParsedExternalAccessToken> VerifyExternalAccessTokenAsync(MixExternalLoginProviders provider, string accessToken, MixAuthenticationConfigurations appConfigs);
    }
}