using System;
using System.Collections.Generic;

namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixCmsUser
    {
        public string Id { get; set; }
        public string Address { get; set; }
        public string Avatar { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string PhoneNumber { get; set; }
        public int Priority { get; set; }
        public int Status { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }
}
