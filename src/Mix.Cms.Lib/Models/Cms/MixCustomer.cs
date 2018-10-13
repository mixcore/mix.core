using System;
using System.Collections.Generic;

namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixCustomer
    {
        public MixCustomer()
        {
            MixOrder = new HashSet<MixOrder>();
        }

        public int Id { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public DateTime? BirthDay { get; set; }
        public string Avatar { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string CreatedBy { get; set; }
        public int Priority { get; set; }
        public bool IsAgreeNotified { get; set; }

        public ICollection<MixOrder> MixOrder { get; set; }
    }
}
