﻿using System;
using System.Collections.Generic;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Constants;
namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixMessengerHubRoom
    {
        public MixMessengerHubRoom()
        {
            MixMessengerMessage = new HashSet<MixMessengerMessage>();
            MixMessengerNavRoomUser = new HashSet<MixMessengerNavRoomUser>();
        }

        public Guid Id { get; set; }
        public string Avatar { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Description { get; set; }
        public string HostId { get; set; }
        public bool IsOpen { get; set; }
        public DateTime? LastModified { get; set; }
        public string Name { get; set; }
        public int? TeamId { get; set; }
        public string Title { get; set; }

        public virtual ICollection<MixMessengerMessage> MixMessengerMessage { get; set; }
        public virtual ICollection<MixMessengerNavRoomUser> MixMessengerNavRoomUser { get; set; }
    }
}
