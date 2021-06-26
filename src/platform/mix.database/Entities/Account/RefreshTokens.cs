using Mix.Heart.Entity;
using System;

namespace Mix.Database.Entities.Account
{
    public partial class RefreshTokens: IEntity<Guid>
    {
        public Guid Id { get; set; }
        public Guid ClientId { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public DateTime ExpiresUtc { get; set; }
        public DateTime IssuedUtc { get; set; }
    }
}