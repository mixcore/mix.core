using Microsoft.AspNetCore.Http;
using Mix.Auth.Constants;
using System.Security.Claims;

namespace Mix.Lib.Services
{
    public sealed class MixUserConext(IHttpContextAccessor contextAccessor)
    {
        public ClaimsPrincipal? User
        {
            get
            {
                if (contextAccessor.HttpContext?.User is not null)
                {
                    return contextAccessor.HttpContext.User;
                }

                return null;
            }
        }

        public string? UserName
        {
            get
            {
                return GetClaimValue(MixClaims.Username);
            }

            private set { }
        }

        public string? Role
        {
            get
            {
                return GetClaimValue(MixClaims.Role);
            }

            private set { }
        }

        private List<Claim> GetClaimsByType(string type)
        {
            if (User is null)
            {
                return [];
            }

            return User.Claims.Where(c => c.Type == type).ToList();
        }

        private string? GetClaimValue(string type)
        {
            if (User is null)
            {
                return null;
            }

            return string.Join(',', GetClaimsByType(type).Select(m => m.Value));
        }
    }
}
