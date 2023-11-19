using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Mix.OAuth.Common;
using Mix.OAuth.OauthResponse;
using Mix.Database.Entities.Account;

namespace Mix.OAuth.Services
{
    public class TokenRevocationService : ITokenRevocationService
    {
        private readonly MixCmsAccountContext _dbContext;
        public TokenRevocationService(MixCmsAccountContext context)
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
