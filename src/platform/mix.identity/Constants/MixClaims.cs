namespace Mix.Identity.Constants
{
    public class MixClaims
    {
        public const string Id = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
        public const string Role = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
        public const string Username = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";
        public const string RefreshToken = "RefreshToken";
        public const string AESKey = "AESKey";
        public const string RSAPublicKey = "RSAPublicKey";
        public const string ExpireAt = "ExpireAt";
        public const string Endpoints = "Endpoints";
    }
}