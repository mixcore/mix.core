namespace Mix.Database.Entities.Account
{
    public partial class OAuthClient
    {
        public string Id { get; set; }
        public bool Active { get; set; }
        public string AllowedOrigin { get; set; }
        public int ApplicationType { get; set; }
        public string Name { get; set; }
        public int RefreshTokenLifeTime { get; set; }
        public string Secret { get; set; }
    }
}