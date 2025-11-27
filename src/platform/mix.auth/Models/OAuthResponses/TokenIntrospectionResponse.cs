/*
                        GNU GENERAL PUBLIC LICENSE
                          Version 3, 29 June 2007
 Copyright (C) 2022 Mohammed Ahmed Hussien babiker Free Software Foundation, Inc. <https://fsf.org/>
 Everyone is permitted to copy and distribute verbatim copies
 of this license document, but changing it is not allowed.
 */

namespace Mix.Auth.Models.OAuthResponses
{
    public class TokenIntrospectionResponse
    {
        /// <summary>
        /// Is Token Active / Required parameter
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Associated scope with this token,
        /// Return as space-sperated list
        /// </summary>
        public string? Scope { get; set; }

        /// <summary>
        /// Client Identifier for OAuth client that 
        /// requested this token
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Resource owner who authorized this token
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Access Token or Refresh Token
        /// </summary>
        public string TokenType { get; set; }

        /// <summary>
        /// Expiration Time
        /// </summary>
        public int Exp { get; set; }

        /// <summary>
        /// Issued At
        /// </summary>
        public int Iat { get; set; }

        /// <summary>
        /// Not Before
        /// </summary>
        public int Nbf { get; set; }

        /// <summary>
        /// Subject
        /// </summary>
        public string Sub { get; set; }

        /// <summary>
        /// Audience
        /// </summary>
        public string? Aud { get; set; }

        /// <summary>
        /// Issuer
        /// </summary>
        public string Iss { get; set; }
        /// <summary>
        /// JWT ID
        /// </summary>
        public string Jti { get; set; }
    }
}
