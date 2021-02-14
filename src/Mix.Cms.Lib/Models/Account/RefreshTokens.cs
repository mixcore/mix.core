using System;

namespace Mix.Cms.Lib.Models.Account
{
    public partial class RefreshTokens
    {
        public string Id { get; set; }
        public string ClientId { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public DateTime ExpiresUtc { get; set; }
        public DateTime IssuedUtc { get; set; }
    }
}