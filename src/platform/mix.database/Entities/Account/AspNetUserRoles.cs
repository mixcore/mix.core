using System;

namespace Mix.Database.Entities.Account
{
    public partial class AspNetUserRoles
    {
        public string UserId { get; set; }
        public Guid RoleId { get; set; }
        public string MixUserId { get; set; }

        public virtual AspNetUsers MixUser { get; set; }
        public virtual AspNetRoles Role { get; set; }
        public virtual AspNetUsers User { get; set; }
    }
}