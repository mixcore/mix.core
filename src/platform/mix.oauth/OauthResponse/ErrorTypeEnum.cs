/*
                        GNU GENERAL PUBLIC LICENSE
                          Version 3, 29 June 2007
 Copyright (C) 2022 Mohammed Ahmed Hussien babiker Free Software Foundation, Inc. <https://fsf.org/>
 Everyone is permitted to copy and distribute verbatim copies
 of this license document, but changing it is not allowed.
 */

using System.ComponentModel;

namespace Mix.OAuth.OauthResponse
{
    public enum ErrorTypeEnum : byte
    {
        [Description("invalid_request")]
        InvalidRequest,

        [Description("unauthorized_client")]
        UnAuthoriazedClient,

        [Description("access_denied")]
        AccessDenied,

        [Description("unsupported_response_type")]
        UnSupportedResponseType,

        [Description("invalid_scope")]
        InValidScope,

        [Description("server_error")]
        ServerError,

        [Description("temporarily_unavailable")]
        TemporarilyUnAvailable,

        [Description("invalid_grant")]
        InvalidGrant,

        [Description("invalid_client")]
        InvalidClient
    }
}
