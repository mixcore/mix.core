/*
                        GNU GENERAL PUBLIC LICENSE
                          Version 3, 29 June 2007
 Copyright (C) 2022 Mohammed Ahmed Hussien babiker Free Software Foundation, Inc. <https://fsf.org/>
 Everyone is permitted to copy and distribute verbatim copies
 of this license document, but changing it is not allowed.
 */

using System.ComponentModel;

namespace Mix.Auth.Enums
{
    public enum AuthorizationGrantTypesEnum : byte
    {
        [Description("code")] Code,

        [Description("client_credentials")] ClientCredentials,

        [Description("refresh_token")] RefreshToken,

        [Description("authorization_code")] AuthorizationCode
    }
}
