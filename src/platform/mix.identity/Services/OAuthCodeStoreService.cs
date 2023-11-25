using Microsoft.IdentityModel.Tokens;
using Mix.Auth.Models;
using Mix.Identity.Interfaces;
using Mix.OAuth.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Mix.Identity.Services
{
    public class OAuthCodeStoreService : IOAuthCodeStoreService
    {
        private readonly ConcurrentDictionary<string, OAuthCode> _codeIssued = new ConcurrentDictionary<string, OAuthCode>();
        private readonly OAuthClientService _clientService;

        public OAuthCodeStoreService(OAuthClientService clientStore)
        {
            _clientService = clientStore;
        }

        // Here I genrate the code for authorization, and I will store it 
        // in the Concurrent Dictionary

        public string GenerateOAuthCode(OAuthCode oauthCode)
        {
            var client = _clientService.Clients.Where(x => x.Id == oauthCode.ClientId).SingleOrDefault();

            if (client != null)
            {
                var rand = RandomNumberGenerator.Create();
                byte[] bytes = new byte[32];
                rand.GetBytes(bytes);
                var code = Base64UrlEncoder.Encode(bytes);

                _codeIssued[code] = oauthCode;

                return code;
            }
            return null;
        }

        public OAuthCode GetClientDataByCode(string key)
        {
            OAuthCode oauthCode;
            if (_codeIssued.TryGetValue(key, out oauthCode))
            {
                return oauthCode;
            }
            return null;
        }

        // TODO
        // Before updated the Concurrent Dictionary I have to Process User Sign In,
        // and check the user credienail first
        // But here I merge this process here inside update Concurrent Dictionary method
        public OAuthCode UpdateClientDataByCode(string key, ClaimsPrincipal claimsPrincipal, IList<string> requestdScopes)
        {
            var oldValue = GetClientDataByCode(key);

            if (oldValue != null)
            {
                // check the requested scopes with the one that are stored in the Client Store 
                var client = _clientService.Clients.Where(x => x.Id == oldValue.ClientId).FirstOrDefault();

                if (client != null)
                {
                    var clientScope = (from m in client.AllowedScopes
                                       where requestdScopes.Contains(m)
                                       select m).ToList();

                    if (!clientScope.Any())
                        return null;

                    OAuthCode newValue = new OAuthCode
                    {
                        ClientId = oldValue.ClientId,
                        CreationTime = oldValue.CreationTime,
                        IsOpenId = requestdScopes.Contains("openId") || requestdScopes.Contains("profile"),
                        RedirectUri = oldValue.RedirectUri,
                        RequestedScopes = requestdScopes,
                        Nonce = oldValue.Nonce,
                        CodeChallenge = oldValue.CodeChallenge,
                        CodeChallengeMethod = oldValue.CodeChallengeMethod,
                        Subject = claimsPrincipal,
                    };
                    var result = _codeIssued.TryUpdate(key, newValue, oldValue);

                    if (result)
                        return newValue;
                    return null;
                }
            }
            return null;
        }

        public OAuthCode RemoveClientDataByCode(string key)
        {
            OAuthCode oauthCode;
            var isRemoved = _codeIssued.TryRemove(key, out oauthCode);
            if (isRemoved)
                return oauthCode;
            return null;
        }

        // Here I genrate the code for authorization, and I will store it 
        // in the Concurrent Dictionary

        public string? GenerateAuthorizationCode(OAuthCode authorizationCode)
        {
            var client = _clientService.Clients.Where(x => x.Id == authorizationCode.ClientId).SingleOrDefault();

            if (client != null)
            {
                var rand = RandomNumberGenerator.Create();
                byte[] bytes = new byte[32];
                rand.GetBytes(bytes);
                var code = Base64UrlEncoder.Encode(bytes);

                _codeIssued[code] = authorizationCode;

                return code;
            }
            return null;
        }
    }
}
