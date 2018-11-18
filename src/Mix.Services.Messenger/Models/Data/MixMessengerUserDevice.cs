using System;
using System.Collections.Generic;

namespace Mix.Services.Messenger.Models.Data
{
    public partial class MixMessengerUserDevice
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string ConnectionId { get; set; }
        public string DeviceId { get; set; }
        public int Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
