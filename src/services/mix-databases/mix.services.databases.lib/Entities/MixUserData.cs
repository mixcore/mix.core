using Mix.Constant.Enums;
using Mix.Heart.Entities;

namespace Mix.Services.Databases.Lib.Entities
{
    public sealed class MixUserData : EntityBase<int>
    {
        public Guid ParentId { get; set; }
        public MixDatabaseParentType ParentType { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Fullname { get; set; }
        public string? Avatar { get; set; }
        public string? Gender { get; set; }
        public int MixTenantId { get; set; }
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}
