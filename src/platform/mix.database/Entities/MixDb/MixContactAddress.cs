namespace Mix.Database.Entities.MixDb
{
    public class MixContactAddress : EntityBase<int>
    {
        public bool IsDefault { get; set; }
        public string Street { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Ward { get; set; }
        public int SysUserDataId { get; set; }
        public int MixTenantId { get; set; }
    }
}
