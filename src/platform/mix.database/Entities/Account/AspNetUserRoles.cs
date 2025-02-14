namespace Mix.Database.Entities.Account
{
    public partial class AspNetUserRoles : IdentityUserRole<Guid>
    {
        public new Guid UserId { get; set; }
        public new Guid RoleId { get; set; }
        public int TenantId { get; set; }
    }
}