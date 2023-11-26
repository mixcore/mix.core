/*
                        GNU GENERAL PUBLIC LICENSE
                          Version 3, 29 June 2007
 Copyright (C) 2022 Mohammed Ahmed Hussien babiker Free Software Foundation, Inc. <https://fsf.org/>
 Everyone is permitted to copy and distribute verbatim copies
 of this license document, but changing it is not allowed.
 */

using System.Collections.Generic;

namespace Mix.Auth.Models.OAuthResponses
{
    public class AuthorizeResponse
    {
        /// <summary>
        /// code or implicit grant or client creditional 
        /// </summary>
        public string ResponseType { get; set; }
        public string Code { get; set; }
        /// <summary>
        /// required if it was present in the client authorization request
        /// </summary>
        public string State { get; set; }

        public string RedirectUri { get; set; }
        public IList<string> RequestedScopes { get; set; }
        public string GrantType { get; set; }
        public string? Error { get; set; } = string.Empty;
        public string ErrorUri { get; set; }
        public string? ErrorDescription { get; set; }
        public bool HasError => !string.IsNullOrEmpty(Error);
    }
}
