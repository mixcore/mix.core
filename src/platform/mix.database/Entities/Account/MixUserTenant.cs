namespace Mix.Database.Entities.Account
{
    public partial class MixUserTenant
    {
        public int TenantId { get; set; }
        public Guid MixUserId { get; set; }
    }
}