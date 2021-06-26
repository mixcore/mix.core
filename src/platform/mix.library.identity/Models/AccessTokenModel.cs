using System;

namespace Mix.Identity.Models
{
    public class AccessTokenViewModel
    {
        public string AccessToken { get; set; }

        public string TokenType { get; set; }

        public string RefreshToken { get; set; }

        public int ExpiresIn { get; set; }

        public string Client_id { get; set; }

        public DateTime Issued { get; set; }

        public DateTime Expires { get; set; }

        public string DeviceId { get; set; }

        //public MixUserViewModel Info { get; set; }

        public DateTime? LastUpdateConfiguration { get; set; }
    }
}
