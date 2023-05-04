namespace Mix.Database.Entities.MixDb
{
    public sealed class MixUserData : EntityBase<int>
    {
        public Guid ParentId { get; set; }
        public MixDatabaseParentType ParentType { get; set; }
        public string Avatar { get; set; }
        public string PhoneNumber { get; set; }
        public string Fullname { get; set; }
        public int MixTenantId { get; set; }
        public string Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}
