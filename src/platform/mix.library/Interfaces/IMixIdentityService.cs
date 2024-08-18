﻿using Microsoft.AspNetCore.Http;
using Mix.Auth.Dtos;
using Mix.Auth.Enums;
using Mix.Database.Entities.Account;
using Mix.Identity.Domain.Models;
using Mix.Identity.ViewModels;
using Mix.Shared.Models.Configurations;
using System.Security.Claims;

namespace Mix.Lib.Interfaces
{
    public interface IMixIdentityService
    {
        public List<RoleViewModel> Roles { get; set; }
        public bool CheckEndpointPermission(ClaimsPrincipal user, PathString path, string method);
        public Claim CreateClaim(string type, string value);
        public Task<TokenResponseModel> ExternalLogin(RegisterExternalBindingModel model, CancellationToken cancellationToken = default);
        public Task<TokenResponseModel> GenerateAccessTokenAsync(MixUser user, bool isRemember, JObject additionalData = default, CancellationToken cancellationToken = default);
        public Task<string> GenerateTokenAsync(MixUser user, JObject info, DateTime expires, string refreshToken, MixAuthenticationConfigurations appConfigs);
        public Task<TokenResponseModel> GetAuthData(MixUser user, bool rememberMe, int tenantId, JObject additionalData = default, CancellationToken cancellationToken = default);
        public string GetClaim(ClaimsPrincipal User, string claimType);
        public Task<TokenResponseModel> GetTokenAsync(GetTokenModel model, CancellationToken cancellationToken = default);
        public Task<TokenResponseModel> LoginAsync(LoginRequestModel model, CancellationToken cancellationToken = default);
        public Task<MixUser> RegisterAsync(RegisterRequestModel model, int tenantId, UnitOfWorkInfo _cmsUOW, JObject additionalData = default, CancellationToken cancellationToken = default);
        public Task<TokenResponseModel> RenewTokenAsync(RenewTokenDto refreshTokenDto, CancellationToken cancellationToken = default);
        public Task<ParsedExternalAccessToken> VerifyExternalAccessTokenAsync(MixExternalLoginProviders provider, string accessToken, MixAuthenticationConfigurations appConfigs);
    }
}