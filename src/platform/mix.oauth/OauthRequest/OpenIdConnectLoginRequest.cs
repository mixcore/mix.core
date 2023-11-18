/*
                        GNU GENERAL PUBLIC LICENSE
                          Version 3, 29 June 2007
 Copyright (C) 2022 Mohammed Ahmed Hussien babiker Free Software Foundation, Inc. <https://fsf.org/>
 Everyone is permitted to copy and distribute verbatim copies
 of this license document, but changing it is not allowed.
 */

using System.Collections.Generic;

namespace Mix.OAuth.OauthRequest
{
    public class OpenIdConnectLoginRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string RedirectUri { get; set; }
        public string Code { get; set; }
        public IList<string> RequestedScopes { get; set; }

        public bool IsValid()
        {
            return UserName != null && Password != null && Code != null && RedirectUri != null;
        }
    }
}
