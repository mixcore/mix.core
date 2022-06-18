namespace Mix.Database.Entities.Account
{
    public partial class AspNetUserTokens : IdentityUserToken<Guid>
    {
        public virtual MixUser MixUser { get; set; }
    }
}