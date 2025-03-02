namespace Mix.Database.Entities.Account
{
    public partial class RefreshTokens : Entity
    {
        public Guid ClientId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public DateTime ExpiresUtc { get; set; }
        public DateTime IssuedUtc { get; set; }
    }
}