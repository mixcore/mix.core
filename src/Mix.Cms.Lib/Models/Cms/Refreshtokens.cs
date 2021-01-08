using System;
using System.Collections.Generic;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Constants;
namespace Mix.Cms.Lib.Models.Cms
{
    public partial class Refreshtokens
    {
        public string Id { get; set; }
        public string ClientId { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public DateTime ExpiresUtc { get; set; }
        public DateTime IssuedUtc { get; set; }
    }
}
