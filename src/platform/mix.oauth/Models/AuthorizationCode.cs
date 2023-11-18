/*
                        GNU GENERAL PUBLIC LICENSE
                          Version 3, 29 June 2007
 Copyright (C) 2022 Mohammed Ahmed Hussien babiker Free Software Foundation, Inc. <https://fsf.org/>
 Everyone is permitted to copy and distribute verbatim copies
 of this license document, but changing it is not allowed.
 */

using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Mix.OAuth.Models
{
    public class AuthorizationCode
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string RedirectUri { get; set; }

        public DateTime CreationTime { get; set; } = DateTime.UtcNow;


        public bool IsOpenId { get; set; }
        public IList<string> RequestedScopes { get; set; }

        public ClaimsPrincipal Subject { get; set; }
        public string Nonce { get; set; }
        public string CodeChallenge { get; set; }
        public string CodeChallengeMethod { get; set; }
    }
}
