using Mix.Auth.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace Mix.Identity.Interfaces
{
    public interface IOAuthCodeStoreService
    {
        string GenerateAuthorizationCode(OAuthCode authorizationCode);
        OAuthCode GetClientDataByCode(string key);
        OAuthCode UpdateClientDataByCode(string key, ClaimsPrincipal claimsPrincipal, IList<string> requestdScopes);
        OAuthCode RemoveClientDataByCode(string key);
    }
}
