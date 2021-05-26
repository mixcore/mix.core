using System;
using System.Collections.Generic;

namespace Mix.Cms.Messenger.Models.Data
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

        public ICollection<MixMessengerMessage> MixMessengerMessage { get; set; }
        public ICollection<MixMessengerNavRoomUser> MixMessengerNavRoomUser { get; set; }
    }
}