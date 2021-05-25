using System;
using System.Collections.Generic;

namespace Mix.Cms.Messenger.Models.Data
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
        public string Status { get; set; }

        public ICollection<MixMessengerMessage> MixMessengerMessage { get; set; }
        public ICollection<MixMessengerNavRoomUser> MixMessengerNavRoomUser { get; set; }
        public ICollection<MixMessengerNavTeamUser> MixMessengerNavTeamUser { get; set; }
    }
}