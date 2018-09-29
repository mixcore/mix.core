using System;
using System.Collections.Generic;

namespace Mix.Cms.Lib.Models.Account
{
    public partial class AspNetUserLogins
    {
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
        public string ApplicationUserId { get; set; }
        public string ProviderDisplayName { get; set; }
        public string UserId { get; set; }

        public AspNetUsers ApplicationUser { get; set; }
        public AspNetUsers User { get; set; }
    }
}
