using System.Collections.Generic;

namespace Mix.Database.Entities.Account
{
    public class MixUser : IdentityUser<Guid>, IEntity<Guid>
    {
        public MixUser()
        {
            Claims = new HashSet<AspNetUserClaims>();
            AspNetUserClaimsUser = new HashSet<AspNetUserClaims>();
            AspNetUserLoginsUser = new HashSet<AspNetUserLogins>();
            AspNetUserRolesUser = new HashSet<AspNetUserRoles>();
            AspNetUserTokens = new HashSet<AspNetUserTokens>();
        }
        public string Avatar { get; set; }
        public string NickName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime? DOB { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public bool IsActived { get; set; }
        public DateTime? LastModified { get; set; }
        public string ModifiedBy { get; set; }
        public string RegisterType { get; set; }
        public new DateTime? LockoutEnd { get; set; }

        public virtual ICollection<AspNetUserClaims> Claims { get; set; }
        public virtual ICollection<AspNetUserClaims> AspNetUserClaimsUser { get; set; }
        public virtual ICollection<AspNetUserLogins> AspNetUserLoginsUser { get; set; }
        public virtual ICollection<AspNetUserRoles> AspNetUserRolesUser { get; set; }
        public virtual ICollection<AspNetUserTokens> AspNetUserTokens { get; set; }
    }
}
