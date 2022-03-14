namespace Mix.Database.Entities.Account
{
    public partial class AspNetUserLogins
    {
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
        public Guid MixUserId { get; set; }
        public string ProviderDisplayName { get; set; }
        public Guid UserId { get; set; }

        public virtual AspNetUsers MixUser { get; set; }
        public virtual AspNetUsers User { get; set; }
    }
}