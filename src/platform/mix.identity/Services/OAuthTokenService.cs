using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Mix.Database.Entities.Account;
using Mix.Shared.Models.Configurations;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Mix.Auth.Models;
using Mix.Identity.Interfaces;
using Mix.Auth.Common;
using Mix.Auth.Enums;
using Mix.Shared.Services;

namespace Mix.Identity.Services
{
    // for encrypted key see: https://stackoverflow.com/questions/18223868/how-to-encrypt-jwt-security-token
    public sealed class OAuthTokenService : IOAuthTokenService
    {
        private readonly IOAuthClientService _clientService;
        private readonly IOAuthCodeStoreService _codeStoreService;
        private readonly OAuthServerOptions _options;
        private readonly AuthConfigService _authConfigService;
        private readonly MixAuthenticationConfigurations _authConfigs;

        public OAuthTokenService(IOAuthCodeStoreService codeStoreService,
            IOptions<OAuthServerOptions> options,
            IConfiguration configuration,
            IOAuthClientService clientService,
            AuthConfigService authConfigService)
        {
            _clientService = clientService;
            _codeStoreService = codeStoreService;
            _options = options.Value;
            _authConfigService = authConfigService;
            _authConfigs = _authConfigService.AppSettings;
        }
        public AuthorizeResponse AuthorizeRequest(IHttpContextAccessor httpContextAccessor, OAuthRequest authorizationRequest)
        {
            AuthorizeResponse response = new AuthorizeResponse();

            if (httpContextAccessor == null || httpContextAccessor.HttpContext == null)
            {
                response.Error = ErrorTypeEnum.ServerError.GetEnumDescription();
                return response;
            }

            var client = VerifyClientById(authorizationRequest.client_id);
            if (!client.IsSuccess)
            {
                response.Error = client.ErrorDescription;
                return response;
            }

            if (string.IsNullOrEmpty(authorizationRequest.response_type) || authorizationRequest.response_type != "code")
            {
                response.Error = ErrorTypeEnum.InvalidRequest.GetEnumDescription();
                response.ErrorDescription = "response_type is required or is not valid";
                return response;
            }

            if (!authorizationRequest.redirect_uri.IsRedirectUriStartWithHttps() && !httpContextAccessor.HttpContext.Request.IsHttps)
            {
                response.Error = ErrorTypeEnum.InvalidRequest.GetEnumDescription();
                response.ErrorDescription = "redirect_url is not secure, MUST be TLS";
                return response;
            }


            // check the return url is match the one that in the client store


            // check the scope in the client store with the
            // one that is comming from the request MUST be matched at leaset one

            var scopes = authorizationRequest.scope.Split(' ');

            var clientScopes = from m in client.Client.AllowedScopes
                               where scopes.Contains(m)
                               select m;

            if (!clientScopes.Any())
            {
                response.Error = ErrorTypeEnum.InValidScope.GetEnumDescription();
                response.ErrorDescription = "scopes are invalids";
                return response;
            }

            string nonce = authorizationRequest.nonce;

            // Verify that a scope parameter is present and contains the openid scope value.
            // (If no openid scope value is present,
            // the request may still be a valid OAuth 2.0 request, but is not an OpenID Connect request.)

            var authoCode = new OAuthCode
            {
                ClientId = authorizationRequest.client_id,
                RedirectUri = authorizationRequest.redirect_uri,
                RequestedScopes = clientScopes.ToList(),
                Nonce = nonce,
                CodeChallenge = authorizationRequest.code_challenge,
                CodeChallengeMethod = authorizationRequest.code_challenge_method,
                CreationTime = DateTime.UtcNow,
                Subject = httpContextAccessor.HttpContext.User //as ClaimsPrincipal

            };

            string code = _codeStoreService.GenerateAuthorizationCode(authoCode);
            if (code == null)
            {
                response.Error = ErrorTypeEnum.TemporarilyUnAvailable.GetEnumDescription();
                return response;
            }

            response.RedirectUri = client.Client.RedirectUris.FirstOrDefault() + "?response_type=code" + "&state=" + authorizationRequest.state;
            response.Code = code;
            response.State = authorizationRequest.state;
            response.RequestedScopes = clientScopes.ToList();

            return response;
        }

        private CheckOAuthClientResult VerifyClientById(Guid clientId, bool checkWithSecret = false, string clientSecret = null, string grantType = null)
        {
            CheckOAuthClientResult result = new CheckOAuthClientResult() { IsSuccess = false };

            if (clientId != default)
            {
                var client = _clientService.Clients.Where(x => x.Id.Equals(clientId)).FirstOrDefault();

                if (client != null)
                {
                    if (checkWithSecret && !string.IsNullOrEmpty(clientSecret))
                    {
                        bool hasSamesecretId = client.Secret.Equals(clientSecret, StringComparison.InvariantCulture);
                        if (!hasSamesecretId)
                        {
                            result.Error = ErrorTypeEnum.InvalidClient.GetEnumDescription();
                            return result;
                        }
                    }
                    // check if client is enabled or not

                    if (client.IsActive)
                    {
                        result.IsSuccess = true;
                        result.Client = client;

                        return result;
                    }
                    else
                    {
                        result.ErrorDescription = ErrorTypeEnum.UnAuthoriazedClient.GetEnumDescription();
                        return result;
                    }
                }
            }

            result.ErrorDescription = ErrorTypeEnum.AccessDenied.GetEnumDescription();
            return result;
        }


        public OAuthTokenResponse GenerateToken(OAuthTokenRequest tokenRequest)
        {

            var result = new OAuthTokenResponse();
            var serchBySecret = SearchForClientBySecret(tokenRequest.grant_type);

            var checkClientResult = VerifyClientById(tokenRequest.client_id, serchBySecret, tokenRequest.client_secret, tokenRequest.grant_type);
            if (!checkClientResult.IsSuccess)
            {
                return new OAuthTokenResponse { Error = checkClientResult.Error, ErrorDescription = checkClientResult.ErrorDescription };
            }

            // Check first if the authorization_grant is client_credentials...
            // then generate the jwt access token and store it to back store

            if (tokenRequest.grant_type == AuthorizationGrantTypesEnum.ClientCredentials.GetEnumDescription())
            {
                var clientHasClientCredentialsGrant = checkClientResult.Client.GrantTypes.Contains(tokenRequest.grant_type);
                if (!clientHasClientCredentialsGrant)
                {
                    result.Error = ErrorTypeEnum.InvalidGrant.GetEnumDescription();
                    return result;
                }
                IEnumerable<string> scopes = checkClientResult.Client.AllowedScopes.Intersect(tokenRequest.scope);

                var clientCredentialAccessTokenResult = GenerateJWTToken(scopes, OAuthConstants.TokenTypes.JWTAcceseccToken, checkClientResult.Client);
                result.access_token = clientCredentialAccessTokenResult.AccessToken;
                result.expired_at = clientCredentialAccessTokenResult.ExpirationDate;
                result.id_token = null; // I have to use data shaping here to remove this property or I can customize the return data in the json result, but for now null is ok.
                result.code = tokenRequest.code;
                return result;
            }


            // check code from the Concurrent Dictionary
            var clientCodeChecker = _codeStoreService.GetClientDataByCode(tokenRequest.code);
            if (clientCodeChecker == null)
                return new OAuthTokenResponse { Error = ErrorTypeEnum.InvalidGrant.GetEnumDescription() };


            // check if the current client who is one made this authentication request

            if (tokenRequest.client_id != clientCodeChecker.ClientId)
                return new OAuthTokenResponse { Error = ErrorTypeEnum.InvalidGrant.GetEnumDescription() };

            // TODO: 
            // also I have to check the rediret uri 


            if (checkClientResult.Client.UsePkce)
            {
                var pkceResult = CodeVerifierIsSendByTheClientThatReceivedTheCode(tokenRequest.code_verifier,
                    clientCodeChecker.CodeChallenge, clientCodeChecker.CodeChallengeMethod);

                if (!pkceResult)
                    return new OAuthTokenResponse { Error = ErrorTypeEnum.InvalidGrant.GetEnumDescription() };
            }


            string id_token = string.Empty;
            if (clientCodeChecker.IsOpenId)
            {
                if (!clientCodeChecker.Subject.Identity!.IsAuthenticated)
                    // I have to inform the caller to redirect the user to the login page
                    return new OAuthTokenResponse { Error = ErrorTypeEnum.InvalidGrant.GetEnumDescription() };

                var currentUserName = clientCodeChecker.Subject.Identity.Name;

                var userId = clientCodeChecker.Subject.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(currentUserName))
                    return new OAuthTokenResponse { Error = ErrorTypeEnum.InvalidGrant.GetEnumDescription() };

                // Generate Identity Token
                int iat = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
                string[] amrs = new string[] { "pwd" };

                var claims = new List<Claim>()
                    {
                        new Claim("sub", userId),
                        new Claim("given_name", currentUserName),
                        new Claim("iat", iat.ToString(), ClaimValueTypes.Integer), // time stamp
                        new Claim("nonce", clientCodeChecker.Nonce)
                    };
                foreach (var amr in amrs)
                    claims.Add(new Claim("amr", amr));// authentication

                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

                var token = new JwtSecurityToken(
                                    _options.IDPUri,
                                    checkClientResult.Client.Id.ToString(),
                                    claims,
                                    expires: DateTime.UtcNow.AddMinutes(int.Parse("50")),
                                    signingCredentials: new SigningCredentials(
                                                            new SymmetricSecurityKey(
                                                                Encoding.ASCII.GetBytes(_authConfigs.SecretKey)),
                                                                SecurityAlgorithms.HmacSha256));

                id_token = handler.WriteToken(token);
            }

            var scopesinJWtAccessToken = from m in clientCodeChecker.RequestedScopes.ToList()
                                         where !OAuth2ServerHelpers.OpenIdConnectScopes.Contains(m)
                                         select m;

            var accessTokenResult = GenerateJWTToken(scopesinJWtAccessToken, OAuthConstants.TokenTypes.JWTAcceseccToken, checkClientResult.Client);

            // here remove the code from the Concurrent Dictionary
            _codeStoreService.RemoveClientDataByCode(tokenRequest.code);

            result.access_token = accessTokenResult.AccessToken;
            result.id_token = id_token;
            result.code = tokenRequest.code;
            return result;
        }

        private bool CodeVerifierIsSendByTheClientThatReceivedTheCode(string codeVerifier, string codeChallenge, string codeChallengeMethod)
        {
            var odeVerifireAsByte = Encoding.ASCII.GetBytes(codeVerifier);

            if (codeChallengeMethod == OAuthConstants.Plain)
            {
                using var shaPalin = SHA256.Create();
                var computedHashPalin = shaPalin.ComputeHash(odeVerifireAsByte);
                var tranformedResultPalin = Base64UrlEncoder.Encode(computedHashPalin);
                return tranformedResultPalin.Equals(codeChallenge);
            }

            using var shaS256 = SHA256.Create();
            var computedHashS256 = shaS256.ComputeHash(odeVerifireAsByte);
            var tranformedResultS256 = Base64UrlEncoder.Encode(computedHashS256);

            return tranformedResultS256.Equals(codeChallenge);
        }


        private bool SearchForClientBySecret(string grantType)
        {
            if (grantType == AuthorizationGrantTypesEnum.ClientCredentials.GetEnumDescription() ||
                grantType == AuthorizationGrantTypesEnum.RefreshToken.GetEnumDescription() ||
                grantType == AuthorizationGrantTypesEnum.ClientCredentials.GetEnumDescription())
                return true;

            return false;
        }


        public TokenResult GenerateJWTToken(IEnumerable<string> scopes, string tokenType, OAuthClient client)
        {
            var result = new TokenResult();

            if (tokenType == OAuthConstants.TokenTypes.JWTAcceseccToken)
            {
                var claims_at = new List<Claim>
                {
                    new Claim("scope", string.Join(' ', scopes))
                };

                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                var token1 = new JwtSecurityToken(
                    _options.IDPUri,
                    client.ClientUri,
                    claims_at,
                    expires: DateTime.UtcNow.AddMinutes(_authConfigs.AccessTokenExpiration),
                    signingCredentials: new SigningCredentials(
                                            new SymmetricSecurityKey(
                                                    Encoding.ASCII.GetBytes(_authConfigs.SecretKey)),
                                                    SecurityAlgorithms.HmacSha256));

                string access_token = handler.WriteToken(token1);

                result.AccessToken = access_token;
                result.TokenType = tokenType;
                result.ExpirationDate = token1.ValidTo;
            }
            else
            {

            }

            return result;
        }
    }
}

