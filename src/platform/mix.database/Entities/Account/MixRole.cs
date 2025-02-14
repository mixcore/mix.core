using System.Collections.Generic;

namespace Mix.Database.Entities.Account
{
    public partial class MixRole : IdentityRole<Guid>, IEntity<Guid>
    {
        public MixRole() : base()
        {
        }

        public MixRole(string roleName) : base(roleName)
        {
        }

    }
}