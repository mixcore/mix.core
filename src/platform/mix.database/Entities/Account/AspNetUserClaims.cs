namespace Mix.Database.Entities.Account
{
    public partial class AspNetUserClaims : IdentityUserClaim<Guid>
    {
        public virtual MixUser MixUser{ get; set; }
    }
}