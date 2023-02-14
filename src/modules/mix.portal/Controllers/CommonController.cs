using Microsoft.AspNetCore.Mvc;
using Mix.Heart.Helpers;
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
        private readonly MixRepoDbRepository _repoDbRepository;
        private readonly MixCmsContext _context;
        public CommonController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixCmsContext context,
            MixService mixService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            IQueueService<MessageQueueModel> queueService,
            TenantUserManager userManager,
            MixRepoDbRepository repoDbRepository)
            : base(httpContextAccessor, configuration, mixService, translator, mixIdentityService, queueService)
        {
            _context = context;
            UserManager = userManager;
            _repoDbRepository = repoDbRepository;
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
        public async Task<ActionResult<JArray?>> PortalMenus()
        {
            var user = await UserManager.GetUserAsync(User);
            var roles = await UserManager.GetRolesAsync(user);
            var menus = await LoadUserPortalMenus(roles.ToArray());
            return Ok(menus);
        }



        private async Task<JArray?> LoadUserPortalMenus(string[] roles)
        {
            try
            {
                JArray arrMenus = new();
                if (!roles.Contains(MixRoles.SuperAdmin))
                {
                    _repoDbRepository.InitTableName(MixDatabaseNames.PORTAL_MENU);

                    var menus = await _repoDbRepository.GetListByAsync(
                            new List<SearchQueryField>()
                            {
                                new SearchQueryField("Role", roles, MixCompareOperator.InRange)
                            }
                        );                    
                    foreach (var item in menus)
                    {
                        var obj = ReflectionHelper.ParseObject(item);
                        if (!obj.ContainsKey("subMenus"))
                        {
                            await LoadNestedData(obj);
                        }
                        arrMenus.Add(obj);
                    }
                    return arrMenus;
                }
                return arrMenus;
            }
            catch (Exception ex)
            {
                return new();
            }
        }

        private async Task LoadNestedData(JObject data)
        {
            
            _repoDbRepository.InitTableName(nameof(MixDatabaseAssociation));
            List<QueryField> queries = GetAssociatoinQueries(MixDatabaseNames.PORTAL_MENU, MixDatabaseNames.PORTAL_MENU, data.Value<int>("id"));
            var associations = await _repoDbRepository.GetListByAsync(queries);
            if (associations.Count > 0)
            {
                _repoDbRepository.InitTableName(MixDatabaseNames.PORTAL_MENU);
                var nestedIds = JArray.FromObject(associations).Select(m => m.Value<int>("ChildId")).ToList();
                List<QueryField> query = new() { new("Id", Operation.In, nestedIds) };
                var nestedData = await _repoDbRepository.GetListByAsync(query);
                data.Add(new JProperty("subMenus", ReflectionHelper.ParseArray(nestedData)));
            }
        }

        private List<QueryField> GetAssociatoinQueries(string parentDatabaseName = null, string childDatabaseName = null, int? parentId = null, int? childId = null)
        {
            var queries = new List<QueryField>();
            if (!string.IsNullOrEmpty(parentDatabaseName))
            {
                queries.Add(new QueryField("ParentDatabaseName", parentDatabaseName));
            }
            if (!string.IsNullOrEmpty(childDatabaseName))
            {
                queries.Add(new QueryField("ChildDatabaseName", childDatabaseName));
            }
            if (parentId.HasValue)
            {
                queries.Add(new QueryField("ParentId", parentId));
            }
            if (childId.HasValue)
            {
                queries.Add(new QueryField("ChildId", childId));
            }
            return queries;
        }
    }
}
