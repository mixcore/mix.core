using System;
using System.Collections.Generic;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Constants;
namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixMessengerMessage
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? RoomId { get; set; }
        public int? TeamId { get; set; }
        public string UserId { get; set; }

        public virtual MixMessengerHubRoom Room { get; set; }
        public virtual MixMessengerTeam Team { get; set; }
        public virtual MixMessengerUser User { get; set; }
    }
}
