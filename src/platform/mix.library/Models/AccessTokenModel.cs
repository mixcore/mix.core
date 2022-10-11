namespace Mix.Lib.Models
{
    public sealed class AccessTokenViewModel
    {
        public string AccessToken { get; set; }

        public string TokenType { get; set; }

        public Guid RefreshToken { get; set; }

        public int ExpiresIn { get; set; }

        public string ClientId { get; set; }

        public DateTime Issued { get; set; }

        public DateTime Expires { get; set; }

        public string DeviceId { get; set; }

        public MixUserViewModel Info { get; set; }

        public List<string> Roles { get; set; }

        public DateTime? LastUpdateConfiguration { get; set; }
    }
}
