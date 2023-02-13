
using Mix.Tenancy.Domain.Dtos;
using Mix.Tenancy.Domain.Services;

namespace Mix.Xunittest.Domain.Tests
{
    [Collection("Step 0 - Init")]
    public class c0_InitCmsTest
        : TestBase<SharedMixCmsDbFixture, MixCmsContext>
    {
        private readonly IInitCmsService _initCmsService;
        public c0_InitCmsTest(
            SharedMixCmsDbFixture fixture,
            IInitCmsService initCmsService) : base(fixture)
        {
            _initCmsService = initCmsService;
            model = new()
            {
                Culture = new()
                {
                    FullName = "United States - English",
                    Icon = "flag-icon-us",
                    Specificulture = "en-us"
                },
                SqliteDbConnectionString = DbFixture.ConnectionString,
                DatabaseProvider = DbFixture.DbProvider,
                SiteName = "Test"
            };
        }

        #region Properties

        private static InitCmsDto model;

        #endregion

        [Fact, TestPriority(1)]
        public async Task Step_1_Init_Site()
        {
            DbFixture.Context = new(DbFixture.ConnectionString, DbFixture.DbProvider);
            await DbFixture.Context.Database.EnsureDeletedAsync();
            model.PrimaryDomain = "localhost";
            await _initCmsService.InitDbContext(model);
            await _initCmsService.InitTenantAsync(model);
        }
    }
}
