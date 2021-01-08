using System;
using System.Collections.Generic;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Constants;
namespace Mix.Cms.Lib.Models.Cms
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
