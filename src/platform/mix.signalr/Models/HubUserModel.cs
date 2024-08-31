﻿namespace Mix.SignalR.Models
{
    public class HubUserModel
    {
        public int? TenantId { get; set; }
        public string ConnectionId { get; set; }
        public string Username { get; set; }
        public string Avatar { get; set; }
        public string Role { get; set; }

        public HubUserModel()
        {

        }

        public HubUserModel(string fromName)
        {
            Username = fromName;
        }
    }
}
