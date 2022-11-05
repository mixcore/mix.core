using System.ComponentModel.DataAnnotations.Schema;

namespace Mix.Database.Entities.Account
{
    public partial class AspNetUserClaims : IdentityUserClaim<Guid>
    {
        [NotMapped]
        public virtual MixUser MixUser { get; set; }
    }
}