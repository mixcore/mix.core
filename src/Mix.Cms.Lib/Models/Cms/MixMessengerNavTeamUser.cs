using System;

namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixMessengerNavTeamUser
    {
        public int TeamId { get; set; }
        public string UserId { get; set; }
        public DateTime JoinedDate { get; set; }
        public DateTime? LastModified { get; set; }
        public int Status { get; set; }

        public virtual MixMessengerTeam Team { get; set; }
        public virtual MixMessengerUser User { get; set; }
    }
}