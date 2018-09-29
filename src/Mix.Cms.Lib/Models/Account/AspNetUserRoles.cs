using System;
using System.Collections.Generic;

namespace Mix.Cms.Lib.Models.Account
{
    public partial class AspNetUserRoles
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public string ApplicationUserId { get; set; }

        public AspNetUsers ApplicationUser { get; set; }
        public AspNetRoles Role { get; set; }
        public AspNetUsers User { get; set; }
    }
}
