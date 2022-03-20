using System.Collections.Generic;

namespace Mix.Database.Entities.Account
{
    public class MixUser : IdentityUser<Guid>, IEntity<Guid>
    {
        public MixUser()
        {
            Claims = new HashSet<AspNetUserClaims>();
            AspNetUserClaimsUser = new HashSet<AspNetUserClaims>();
            AspNetUserLoginsApplicationUser = new HashSet<AspNetUserLogins>();
            AspNetUserLoginsUser = new HashSet<AspNetUserLogins>();
            AspNetUserRolesApplicationUser = new HashSet<AspNetUserRoles>();
            AspNetUserRolesUser = new HashSet<AspNetUserRoles>();
            AspNetUserTokens = new HashSet<AspNetUserTokens>();
        }
        public DateTime CreatedDateTime { get; set; }
        public bool IsActived { get; set; }
        public System.DateTime? LastModified { get; set; }
        public string ModifiedBy { get; set; }
        public string RegisterType { get; set; }
        public new DateTime? LockoutEnd { get; set; }

        public virtual ICollection<AspNetUserClaims> Claims { get; set; }
        public virtual ICollection<AspNetUserClaims> AspNetUserClaimsUser { get; set; }
        public virtual ICollection<AspNetUserLogins> AspNetUserLoginsApplicationUser { get; set; }
        public virtual ICollection<AspNetUserLogins> AspNetUserLoginsUser { get; set; }
        public virtual ICollection<AspNetUserRoles> AspNetUserRolesApplicationUser { get; set; }
        public virtual ICollection<AspNetUserRoles> AspNetUserRolesUser { get; set; }
        public virtual ICollection<AspNetUserTokens> AspNetUserTokens { get; set; }
    }
}
