using Microsoft.AspNetCore.Http;
using Mix.Auth.Models;
using Mix.Database.Entities.Account;
using System.Collections.Generic;

namespace Mix.Identity.Interfaces
{
    public interface IOAuthTokenService
    {
        AuthorizeResponse AuthorizeRequest(IHttpContextAccessor httpContextAccessor, OAuthRequest authorizationRequest);
        TokenResult GenerateJWTToken(IEnumerable<string> scopes, string tokenType, OAuthClient client);
        OAuthTokenResponse GenerateToken(OAuthTokenRequest tokenRequest);
    }
}