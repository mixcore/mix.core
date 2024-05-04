namespace Mix.Database.Entities.Account
{
    public class Permission : EntityBase<int>
    {
        public int MixTenantId { get; set; }
        public string DisplayName { get; set; }
        public string Group { get; set; }
        public string Key { get; set; }
    }
}
