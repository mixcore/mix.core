using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mix.Database.Entities.Account;
using Mix.Lib.Extensions;
using Mix.Lib.Models;

namespace Mix.Lib.Services
{
    public class TenantUserManager : UserManager<MixUser>
    {
        protected IHttpContextAccessor _httpContextAccessor;
        private MixTenantSystemModel _currentTenant;
        protected MixTenantSystemModel CurrentTenant
        {
            get
            {
                if (_currentTenant == null)
                {
                    _currentTenant = _httpContextAccessor.HttpContext.Session.Get<MixTenantSystemModel>(MixRequestQueryKeywords.Tenant);
                }
                return _currentTenant;
            }
        }
        public TenantUserManager(
            IHttpContextAccessor httpContextAccessor,
            IUserStore<MixUser> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<MixUser> passwordHasher,
            IEnumerable<IUserValidator<MixUser>> userValidators,
            IEnumerable<IPasswordValidator<MixUser>> passwordValidators,
            ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors,
            IServiceProvider services, ILogger<TenantUserManager> logger,
            MixCmsAccountContext context) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            _httpContextAccessor = httpContextAccessor;
            Context = context;
        }

        public MixCmsAccountContext Context { get; }

        public async Task<MixUser> FindByPhoneNumberAsync(string phoneNumber)
        {
            return await Context.MixUsers.SingleOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
        }

        public override async Task<IdentityResult> AddToRoleAsync(MixUser user, string roleName)
        {
            var result = new IdentityResult();
            var role = Context.MixRoles.SingleOrDefault(x => x.Name == roleName.ToString());
            if (!Context.AspNetUserRoles.Any(m => m.UserId == user.Id && m.RoleId == role.Id && m.MixTenantId == CurrentTenant.Id))
            {
                Context.AspNetUserRoles.Add(new AspNetUserRoles()
                {
                    UserId = user.Id,
                    RoleId = role.Id,
                    MixTenantId = CurrentTenant.Id
                });
                await Context.SaveChangesAsync();
            }
            return await base.UpdateAsync(user);
        }

        public async Task AddToRoleAsync(MixUser user, string roleName, int tenantId)
        {
            var role = Context.MixRoles.SingleOrDefault(x => x.Name == roleName.ToString());
            if (!Context.AspNetUserRoles.Any(m => m.UserId == user.Id && m.RoleId == role.Id && m.MixTenantId == tenantId))
            {
                Context.AspNetUserRoles.Add(new AspNetUserRoles()
                {
                    UserId = user.Id,
                    RoleId = role.Id,
                    MixTenantId = tenantId
                });
                await Context.SaveChangesAsync();
            }
        }

        public async Task RemoveFromRoleAsync(MixUser user, string roleName, int tenantId)
        {
            var role = Context.MixRoles.SingleOrDefault(x => x.Name == roleName.ToString());
            var userRole = Context.AspNetUserRoles.SingleOrDefault(m => m.UserId == user.Id && m.RoleId == role.Id && m.MixTenantId == tenantId);
            if (userRole != null)
            {
                Context.AspNetUserRoles.Remove(userRole);
                await Context.SaveChangesAsync();
            }
        }

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

        public override Task<bool> IsInRoleAsync(MixUser user, string roleName)
        {
            var role = Context.MixRoles.SingleOrDefault(x => x.Name == roleName);
            return Context.AspNetUserRoles.AnyAsync(m => m.UserId == user.Id && m.RoleId == role.Id && m.MixTenantId == CurrentTenant.Id);
        }

        public override async Task<IList<string>> GetRolesAsync(MixUser user)
        {
            var roles = from ur in Context.AspNetUserRoles
                        join r in Context.MixRoles
                        on ur.RoleId equals r.Id
                        where ur.UserId == user.Id && ur.MixTenantId == CurrentTenant.Id
                        select r.Name;
            return await roles.ToListAsync();
        }

        public async Task<IList<MixRole>> GetUserRolesAsync(MixUser user)
        {
            var roles = from ur in Context.AspNetUserRoles
                        join r in Context.MixRoles
                        on ur.RoleId equals r.Id
                        where ur.UserId == user.Id && ur.MixTenantId == CurrentTenant.Id
                        select r;
            return await roles.ToListAsync();
        }
    }
}
