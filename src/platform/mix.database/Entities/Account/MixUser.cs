using System.Collections.Generic;

namespace Mix.Database.Entities.Account
{
    public class MixUser : IdentityUser<Guid>, IEntity<Guid>
    {
        public MixUser()
        {
            Claims = new HashSet<AspNetUserClaims>();
        }
        public DateTime CreatedDateTime { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime? LastModified { get; set; }
        public string ModifiedBy { get; set; }
        public string RegisterType { get; set; }
        public new DateTime? LockoutEnd { get; set; }

        public virtual ICollection<AspNetUserClaims> Claims { get; set; }
    }
}
