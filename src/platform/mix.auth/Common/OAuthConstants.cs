/*
                        GNU GENERAL PUBLIC LICENSE
                          Version 3, 29 June 2007
 Copyright (C) 2022 Mohammed Ahmed Hussien babiker Free Software Foundation, Inc. <https://fsf.org/>
 Everyone is permitted to copy and distribute verbatim copies
 of this license document, but changing it is not allowed.
 */

namespace Mix.Auth.Common
{
    public static class OAuthConstants
    {
        public static string Plain = "plain";
        public static string SHA256 = "S256";

        // OpenID Connect Scopes
        public static string OpenId = "openid";
        public static string Profile = "profile";
        public static string Email = "email";
        public static string Address = "address";
        public static string Phone = "phone";

        // Token Types
        public static class TokenTypes
        {
            public const string JWTAcceseccToken = "Access_Token";
            public const string JWTIdentityToken = "Idp_Token";
            public const string AccessAndIdpToken = "Access_Idp_Token";
        }


        public static class TokenTypeHints
        {
            public const string AccessToken = "access_token";
            public const string RefreshToken = "refresh_token";
        }

        public static class Statuses
        {
            public const string InActive = "inactive";
            public const string Revoked = "revoked";
            public const string Valid = "valid";
        }

        public static class ContentTypeSupported
        {
            public const string XwwwFormUrlEncoded = "application/x-www-form-urlencoded";
            //application/x-www-form-urlencoded
        }


        public static class SupportedProvider
        {
            public const string InMemoey = "InMemoey";
            public const string BackStore = "BackStore";
        }
    }
}
