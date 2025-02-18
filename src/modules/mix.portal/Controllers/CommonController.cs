using Microsoft.AspNetCore.Mvc;
using Mix.Auth.Constants;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Heart.Helpers;
using Mix.Lib.Interfaces;
using Mix.Mixdb.Interfaces;
using Mix.Mixdb.Services;
using Mix.Mq.Lib.Models;
using Mix.RepoDb.Repositories;
using RepoDb;
using RepoDb.Enumerations;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/common")]
    [ApiController]
    public class CommonController : MixTenantApiControllerBase
    {
        protected readonly TenantUserManager UserManager;
        private readonly IMixDbDataService _mixDbDataSrv;
        private readonly MixCmsContext _context;
        public CommonController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            DatabaseService databaseService,
            MixCmsContext context,
            MixCacheService cacheService,
            MixIdentityService mixIdentityService,
            IMemoryQueueService<MessageQueueModel> queueService,
            TenantUserManager userManager,
            IMixTenantService mixTenantService,
            MixDbDataServiceFactory mixDbDataFactory)
            : base(httpContextAccessor, configuration,
                  cacheService, mixIdentityService, queueService, mixTenantService)
        {
            _context = context;
            UserManager = userManager;
            _mixDbDataSrv = mixDbDataFactory.GetDataService(databaseService.DatabaseProvider, databaseService.GetConnectionString(MixConstants.CONST_CMS_CONNECTION))!;
        }

        [HttpGet]
        [Route("{culture}/dashboard")]
        public ActionResult<DashboardModel> Dashboard(string culture)
        {
            var result = new DashboardModel(culture, _context);
            return Ok(result);
        }

        [MixAuthorize]
        [HttpGet("portal-menus")]
        public async Task<ActionResult<JArray>> PortalMenus()
        {
            var user = await UserManager.GetUserAsync(User);
            var roles = await UserManager.GetRolesAsync(user);
            var menus = await LoadUserPortalMenus(roles.ToArray());
            return Ok(menus);
        }



        private async Task<JArray> LoadUserPortalMenus(string[] roles)
        {
            try
            {
                JArray arrMenus = new();
                if (!roles.Contains(MixRoles.SuperAdmin))
                {
                    var menus = await _mixDbDataSrv.GetListByAsync(
                        new Shared.Models.SearchMixDbRequestModel()
                        {

                            TableName = MixDatabaseNames.PORTAL_MENU,
                            Queries = new List<MixQueryField>()
                            {
                                new MixQueryField("Role", roles, MixCompareOperator.InRange)
                            },
                        }
                    );

                    return arrMenus;
                }
                return arrMenus;
            }
            catch (Exception)
            {
                return new();
            }
        }


    }
}
