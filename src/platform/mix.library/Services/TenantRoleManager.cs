using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Mix.Database.Entities.Account;

namespace Mix.Lib.Services
{
    public class TenantRoleManager : RoleManager<MixRole>
    {
        public TenantRoleManager(
            MixCmsAccountContext context,
            IRoleStore<MixRole> store,
            IEnumerable<IRoleValidator<MixRole>> roleValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            ILogger<RoleManager<MixRole>> logger) : base(store, roleValidators, keyNormalizer, errors, logger)
        {
            Context = context;
        }

        public MixCmsAccountContext Context { get; }
    }
}
