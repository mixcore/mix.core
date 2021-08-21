using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Mix.Database.Entities.Account
{
    public partial class MixRole : IdentityRole<Guid>
    {
        public MixRole() : base()
        {
        }

        public MixRole(string roleName) : base(roleName)
        {
        }

        public virtual ICollection<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual ICollection<AspNetUserRoles> AspNetUserRoles { get; set; }
    }
}