using Microsoft.AspNetCore.Http;
using Mix.Database.Entities.Account;
using Mix.Identity.Constants;
using Mix.Identity.Domain.Models;
using Mix.Identity.Dtos;
using Mix.Identity.Models.AccountViewModels;
using Mix.Identity.ViewModels;
using Mix.Lib.Models;
using Mix.Shared.Models;
using System.Security.Claims;

namespace Mix.Lib.Interfaces
{
    public interface IMixIdentityService
    {
        List<RoleViewModel> Roles { get; set; }

        bool CheckEndpointPermission(ClaimsPrincipal user, PathString path, string method);
        Claim CreateClaim(string type, string value);
        Task<JObject> ExternalLogin(RegisterExternalBindingModel model);
        Task<AccessTokenViewModel> GenerateAccessTokenAsync(MixUser user, MixUserViewModel info, bool isRemember, string aesKey, string rsaPublicKey);
        Task<string> GenerateTokenAsync(MixUser user, MixUserViewModel info, DateTime expires, string refreshToken, string aesKey, string rsaPublicKey, MixAuthenticationConfigurations appConfigs);
        Task<JObject> GetAuthData(MixCmsContext context, MixUser user, bool rememberMe, int tenantId);
        string GetClaim(ClaimsPrincipal User, string claimType);
        Task<JObject> GetToken(GetTokenModel model);
        Task<JObject> Login(LoginViewModel model);
        Task<JObject> Register(RegisterViewModel model, int tenantId, UnitOfWorkInfo _cmsUOW);
        Task<JObject> RenewTokenAsync(RenewTokenDto refreshTokenDto);
        Task<ParsedExternalAccessToken> VerifyExternalAccessToken(MixExternalLoginProviders provider, string accessToken, MixAuthenticationConfigurations appConfigs);
    }
}