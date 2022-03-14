using System;

namespace Mix.Database.Entities.Account
{
    public partial class AspNetUserClaims
    {
        public int Id { get; set; }
        public Guid MixUserId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
        public Guid UserId { get; set; }

        public virtual AspNetUsers MixUser { get; set; }
        public virtual AspNetUsers User { get; set; }
    }
}