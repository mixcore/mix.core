using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mix.Database.Entities.Account;

namespace Mix.Lib.Services
{
    public class TenantUserManager : UserManager<MixUser>
    {
        public TenantUserManager(
            IUserStore<MixUser> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<MixUser> passwordHasher,
            IEnumerable<IUserValidator<MixUser>> userValidators,
            IEnumerable<IPasswordValidator<MixUser>> passwordValidators,
            ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors,
            IServiceProvider services, ILogger<TenantUserManager> logger,
            MixCmsAccountContext context) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            Context = context;
        }

        public MixCmsAccountContext Context { get; }

        public async Task AddToTenant(MixUser user, int tenantId)
        {
            if (!Context.MixUserTenants.Any(m => m.TenantId == tenantId && m.MixUserId == user.Id))
            {
                Context.MixUserTenants.Add(new MixUserTenant()
                {
                    MixUserId = user.Id,
                    TenantId = tenantId
                });
                await Context.SaveChangesAsync();
            }
        }
    }
}
