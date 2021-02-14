using System;
using System.Collections.Generic;

namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixMessengerUser
    {
        public MixMessengerUser()
        {
            MixMessengerMessage = new HashSet<MixMessengerMessage>();
            MixMessengerNavRoomUser = new HashSet<MixMessengerNavRoomUser>();
            MixMessengerNavTeamUser = new HashSet<MixMessengerNavTeamUser>();
        }

        public string Id { get; set; }
        public string FacebookId { get; set; }
        public string Avatar { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModified { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }

        public virtual ICollection<MixMessengerMessage> MixMessengerMessage { get; set; }
        public virtual ICollection<MixMessengerNavRoomUser> MixMessengerNavRoomUser { get; set; }
        public virtual ICollection<MixMessengerNavTeamUser> MixMessengerNavTeamUser { get; set; }
    }
}