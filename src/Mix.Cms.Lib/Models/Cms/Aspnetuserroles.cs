using System;
using System.Collections.Generic;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Constants;
namespace Mix.Cms.Lib.Models.Cms
{
    public partial class Aspnetuserroles
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public string ApplicationUserId { get; set; }

        public virtual Aspnetusers ApplicationUser { get; set; }
        public virtual Aspnetroles Role { get; set; }
        public virtual Aspnetusers User { get; set; }
    }
}
