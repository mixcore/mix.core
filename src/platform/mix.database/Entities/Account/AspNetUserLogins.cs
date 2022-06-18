namespace Mix.Database.Entities.Account
{
    public partial class AspNetUserLogins : IdentityUserLogin<Guid>
    {
        public virtual MixUser MixUser { get; set; }
    }
}