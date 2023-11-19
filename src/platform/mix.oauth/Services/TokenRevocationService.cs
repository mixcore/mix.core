/*
                        GNU GENERAL PUBLIC LICENSE
                          Version 3, 29 June 2007
 Copyright (C) 2022 Mohammed Ahmed Hussien babiker Free Software Foundation, Inc. <https://fsf.org/>
 Everyone is permitted to copy and distribute verbatim copies
 of this license document, but changing it is not allowed.
 */

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
            return new TokenRecovationResponse() { Succeeded = true };

            //if (httpContext.Request.ContentType != OAuthConstants.ContentTypeSupported.XwwwFormUrlEncoded)
            //{
            //    response.Succeeded = false;
            //    response.Error = "not supported content type";
            //}
            //string? token = httpContext.Request.Form["token"];
            //string? tokenTypeHint = httpContext.Request.Form["token_type_hint"];

            //var oauthToken = await _dbContext.OAuthTokens
            //    .Where(x => x.Token == token && x.ClientId == clientId &&
            //    (string.IsNullOrWhiteSpace(tokenTypeHint) || tokenTypeHint == x.TokenTypeHint))
            //    .AsNoTracking()
            //    .FirstOrDefaultAsync();
            //if (oauthToken != null)
            //{
            //    oauthToken.Revoked = true;
            //    var res = _dbContext.OAuthTokens.Update(oauthToken);
            //    await _dbContext.SaveChangesAsync();
            //}
            //return response;
        }
    }
}
