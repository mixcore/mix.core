using System;

namespace Mix.Lib.Entities.Cms
{
    public partial class MixMessengerNavRoomUser
    {
        public Guid RoomId { get; set; }
        public string UserId { get; set; }
        public DateTime JoinedDate { get; set; }

        public virtual MixMessengerHubRoom Room { get; set; }
        public virtual MixMessengerUser User { get; set; }
    }
}