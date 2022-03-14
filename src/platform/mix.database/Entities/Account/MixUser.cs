using Microsoft.AspNetCore.Identity;
using System;
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
        public DateTime JoinDate { get; set; }

        public bool IsActived { get; set; }
        public System.DateTime LastModified { get; set; }
        public string ModifiedBy { get; set; }

        public string RegisterType { get; set; }
        public string Avatar { get; set; }
        public string NickName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public int CountryId { get; set; }
        public string Culture { get; set; }
        public DateTime? DOB { get; set; }

        public virtual ICollection<AspNetUserClaims> Claims { get; set; }
        public virtual ICollection<AspNetUserClaims> AspNetUserClaimsUser { get; set; }
        public virtual ICollection<AspNetUserLogins> AspNetUserLoginsApplicationUser { get; set; }
        public virtual ICollection<AspNetUserLogins> AspNetUserLoginsUser { get; set; }
        public virtual ICollection<AspNetUserRoles> AspNetUserRolesApplicationUser { get; set; }
        public virtual ICollection<AspNetUserRoles> AspNetUserRolesUser { get; set; }
        public virtual ICollection<AspNetUserTokens> AspNetUserTokens { get; set; }
    }
}
