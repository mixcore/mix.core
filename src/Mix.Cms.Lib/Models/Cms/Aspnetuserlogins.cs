using System;
using System.Collections.Generic;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Constants;
namespace Mix.Cms.Lib.Models.Cms
{
    public partial class Aspnetuserlogins
    {
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
        public string ApplicationUserId { get; set; }
        public string ProviderDisplayName { get; set; }
        public string UserId { get; set; }

        public virtual Aspnetusers ApplicationUser { get; set; }
        public virtual Aspnetusers User { get; set; }
    }
}
