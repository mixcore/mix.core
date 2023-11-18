/*
                        GNU GENERAL PUBLIC LICENSE
                          Version 3, 29 June 2007
 Copyright (C) 2022 Mohammed Ahmed Hussien babiker Free Software Foundation, Inc. <https://fsf.org/>
 Everyone is permitted to copy and distribute verbatim copies
 of this license document, but changing it is not allowed.
 */

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Mix.Database.Entities.Account;
using Mix.OAuth.Configuration;
using Mix.OAuth.Models;
using Mix.OAuth.OauthRequest;
using Mix.OAuth.OauthResponse;
using Mix.OAuth.Validations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Mix.OAuth.Services
{
    public class TokenIntrospectionService : ITokenIntrospectionService
    {
        private readonly ITokenIntrospectionValidation _tokenIntrospectionValidation;
        private readonly ILogger<TokenIntrospectionService> _logger;
        private readonly MixCmsAccountContext _dbContext;
        private readonly OAuthServerOptions _optionsMonitor;
        private readonly ClientStore _clientStore = new ClientStore();
        public TokenIntrospectionService(
            ITokenIntrospectionValidation tokenIntrospectionValidation,
            ILogger<TokenIntrospectionService> logger,
            MixCmsAccountContext dbContext,
            IOptionsMonitor<OAuthServerOptions> optionsMonitor
            )
        {
            _tokenIntrospectionValidation = tokenIntrospectionValidation;
            _logger = logger;
            _dbContext = dbContext;
            _optionsMonitor = optionsMonitor.CurrentValue ?? new OAuthServerOptions();
        }

        public async Task<TokenIntrospectionResponse> IntrospectTokenAsync(TokenIntrospectionRequest tokenIntrospectionRequest)
        {
            TokenIntrospectionResponse response = new();
            var validationResult = await _tokenIntrospectionValidation.ValidateAsync(tokenIntrospectionRequest);
            if (validationResult.Succeeded == false)
                response.Active = false;

            else
            {
                RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
                string publicPrivateKey = File.ReadAllText("PublicPrivateKey.xml");
                provider.FromXmlString(publicPrivateKey);

                RsaSecurityKey rsaSecurityKey = new RsaSecurityKey(provider);
                JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtSecurityToken = jwtSecurityTokenHandler.ReadJwtToken(tokenIntrospectionRequest.Token);

                TokenValidationParameters tokenValidationParameters = new TokenValidationParameters();

                tokenValidationParameters.IssuerSigningKey = rsaSecurityKey;
                tokenValidationParameters.ValidAudiences = jwtSecurityToken.Audiences;
                tokenValidationParameters.ValidTypes = new[] { "JWT" };
                tokenValidationParameters.ValidateIssuer = true;
                tokenValidationParameters.ValidIssuer = _optionsMonitor.IDPUri;
                tokenValidationParameters.ValidateAudience = true;

                try
                {
                    var tokenValidationReslt = await jwtSecurityTokenHandler.ValidateTokenAsync(tokenIntrospectionRequest.Token, tokenValidationParameters);

                    if (tokenValidationReslt.IsValid)
                    {
                        //int exp = (int)jwtSecurityToken.ValidTo.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
                        int exp = (int)tokenValidationReslt.Claims.FirstOrDefault(x => x.Key == "exp").Value;
                        string? scope = tokenValidationReslt.Claims.FirstOrDefault(x => x.Key == "scope").Value.ToString();
                        string? aud = tokenValidationReslt.Claims.FirstOrDefault(x => x.Key == "aud").Value.ToString();

                        response.Active = true;
                        response.TokenType = "access_token";
                        response.Exp = exp;
                        response.Iat = (int)jwtSecurityToken.IssuedAt.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
                        response.Iss = _optionsMonitor.IDPUri;
                        response.Scope = scope;
                        response.Aud = aud;
                        response.Nbf = (int)jwtSecurityToken.IssuedAt.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogCritical("There is an exception that is thrown while validating the token {exception}", ex);
                    response.Active = false;
                }
            }

            return response;
        }

    }
}
