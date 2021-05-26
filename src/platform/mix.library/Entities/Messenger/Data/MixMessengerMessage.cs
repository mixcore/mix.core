using System;

namespace Mix.Cms.Messenger.Models.Data
{
    public partial class MixMessengerMessage
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? RoomId { get; set; }
        public int? TeamId { get; set; }
        public string UserId { get; set; }

        public MixMessengerHubRoom Room { get; set; }
        public MixMessengerTeam Team { get; set; }
        public MixMessengerUser User { get; set; }
    }
}