using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Mix.Auth.Common;
using Mix.Database.Entities.Account;
using Mix.Identity.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace Mix.Identity.Services
{
    public class OAuthTokenRevocationService : IOAuthTokenRevocationService
    {
        private readonly MixCmsAccountContext _dbContext;
        public OAuthTokenRevocationService(MixCmsAccountContext context)
        {
            _dbContext = context;
        }

        public async Task<TokenRecovationResponse> RevokeTokenAsync(HttpContext httpContext, string clientId)
        {
            var response = new TokenRecovationResponse() { Succeeded = true };
            if (httpContext.Request.ContentType != OAuthConstants.ContentTypeSupported.XwwwFormUrlEncoded)
            {
                response.Succeeded = false;
                response.Error = "not supported content type";
            }
            string? token = httpContext.Request.Form["token"];
            string? tokenTypeHint = httpContext.Request.Form["token_type_hint"];

            var oauthToken = await _dbContext.OAuthToken
                .Where(x => x.Token == token && x.ClientId == clientId &&
                (string.IsNullOrWhiteSpace(tokenTypeHint) || tokenTypeHint == x.TokenTypeHint))
                .AsNoTracking()
                .FirstOrDefaultAsync();
            if (oauthToken != null)
            {
                oauthToken.Revoked = true;
                var res = _dbContext.OAuthToken.Update(oauthToken);
                await _dbContext.SaveChangesAsync();
            }
            return response;
        }
    }
}
