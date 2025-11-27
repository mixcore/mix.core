using System.Security.Claims;

namespace Mix.Auth.Constants
{
    public class MixClaims
    {
        public const string Id = ClaimTypes.NameIdentifier;
        public const string Role = ClaimTypes.Role;
        public const string UserName = ClaimTypes.Name;
        
        public const string TenantId = "TenantId";
        public const string Avatar = "Avatar";
        public const string RefreshToken = "RefreshToken";
        public const string AESKey = "AESKey";
        public const string RSAPublicKey = "RSAPublicKey";
        public const string ExpireAt = "ExpireAt";
        public const string Endpoints = "Endpoints";
    }
}