using Mix.Cms.Lib.Enums;
using System;

namespace Mix.Cms.Messenger.Models.Data
{
    public partial class MixMessengerNavTeamUser
    {
        public int TeamId { get; set; }
        public string UserId { get; set; }
        public DateTime JoinedDate { get; set; }
        public DateTime? LastModified { get; set; }
        public MixContentStatus Status { get; set; }

        public MixMessengerTeam Team { get; set; }
        public MixMessengerUser User { get; set; }
    }
}