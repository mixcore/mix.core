using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Mix.Signalr.Hub.Hubs
{
    [AllowAnonymous]
    public class AuthHub : Microsoft.AspNetCore.SignalR.Hub
    {
        public async Task<string> Authorize()
        {
            return await Task.Run(() => { return TokenHelper.GenerateToken(); });
        }
    }
    public static class TokenHelper
    {
        public static string SECRET = "secret_signing_key";

        public static string GenerateToken()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(SECRET);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
