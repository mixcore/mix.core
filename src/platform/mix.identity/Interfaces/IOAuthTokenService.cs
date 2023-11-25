using Microsoft.AspNetCore.Http;
using Mix.Auth.Models;
using Mix.Auth.Models.Requests;
using Mix.Database.Entities.Account;
using Mix.OAuth.OauthResponse;
using System.Collections.Generic;

namespace Mix.Identity.Interfaces
{
    public interface IOAuthTokenService
    {
        AuthorizeResponse AuthorizeRequest(IHttpContextAccessor httpContextAccessor, OAuthRequest authorizationRequest);
        TokenResult GenerateJWTToken(IEnumerable<string> scopes, string tokenType, OAuthClient client);
        TokenResponse GenerateToken(OAuthTokenRequest tokenRequest);
    }
}