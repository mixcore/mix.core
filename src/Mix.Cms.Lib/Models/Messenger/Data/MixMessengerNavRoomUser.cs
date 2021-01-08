﻿using System;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Constants;
namespace Mix.Cms.Messenger.Models.Data
{
    public partial class MixMessengerNavRoomUser
    {
        public Guid RoomId { get; set; }
        public string UserId { get; set; }
        public DateTime JoinedDate { get; set; }

        public MixMessengerHubRoom Room { get; set; }
        public MixMessengerUser User { get; set; }
    }
}