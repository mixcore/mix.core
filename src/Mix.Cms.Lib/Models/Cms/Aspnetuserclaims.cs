using System;
using System.Collections.Generic;

namespace Mix.Cms.Lib.Models.Cms
{
    public partial class Aspnetuserclaims
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
        public string UserId { get; set; }

        public virtual Aspnetusers ApplicationUser { get; set; }
        public virtual Aspnetusers User { get; set; }
    }
}
