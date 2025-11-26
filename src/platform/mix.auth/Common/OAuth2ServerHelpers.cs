/*
                        GNU GENERAL PUBLIC LICENSE
                          Version 3, 29 June 2007
 Copyright (C) 2022 Mohammed Ahmed Hussien babiker Free Software Foundation, Inc. <https://fsf.org/>
 Everyone is permitted to copy and distribute verbatim copies
 of this license document, but changing it is not allowed.
 */

using System.Collections.Generic;

namespace Mix.Auth.Common
{
    public class OAuth2ServerHelpers
    {
        public static IList<string> CodeChallenegMethodsSupport = new List<string>()
        {
            OAuthConstants.Plain,
            OAuthConstants.SHA256
        };


        public static IList<string> OpenIdConnectScopes = new List<string>()
        {
            OAuthConstants.OpenId,
            OAuthConstants.Profile,
            OAuthConstants.Email,
            OAuthConstants.Address,
            OAuthConstants.Phone
        };
    }
}
