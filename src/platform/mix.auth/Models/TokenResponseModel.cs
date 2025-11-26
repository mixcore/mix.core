using Newtonsoft.Json.Linq;

namespace Mix.Auth.Models
{
    public sealed class TokenResponseModel
    {
        public string AccessToken { get; set; }

        public string TokenType { get; set; }

        public Guid RefreshToken { get; set; }

        public int ExpiresIn { get; set; }

        public string ClientId { get; set; }

        public DateTime Issued { get; set; }

        public DateTime Expires { get; set; }

        public bool IsActive { get; set; }
        public bool EmailConfirmed { get; set; }
        public string DeviceId { get; set; }

        public JObject Info { get; set; }

        public List<string> Roles { get; set; }

        public DateTime? LastUpdateConfiguration { get; set; }
    }
}
