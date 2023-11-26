using Microsoft.AspNetCore.Http;
using Mix.Auth.Models.OAuthResponses;
using System.Threading.Tasks;

namespace Mix.Identity.Interfaces
{
    public interface IOAuthTokenRevocationService
    {
        Task<TokenRecovationResponse> RevokeTokenAsync(HttpContext httpContext, string clientId);
    }
}