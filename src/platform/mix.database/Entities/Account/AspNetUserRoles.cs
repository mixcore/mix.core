using System;

namespace Mix.Database.Entities.Account
{
    public partial class AspNetUserRoles
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        public Guid MixUserId { get; set; }
        public int MixTenantId { get; set; }

        public virtual AspNetUsers MixUser { get; set; }
        public virtual AspNetRoles Role { get; set; }
        public virtual AspNetUsers User { get; set; }
    }
}