using Mix.Tenancy.Domain.Dtos;
using Mix.Tenancy.Domain.Services;


namespace Mix.Xunittest.Domain.Tests
{
    [Collection("Step 0 - Init")]
    public class Step_0_InitCmsTest 
        : TestBase<SharedMixCmsDbFixture, MixCmsContext>, IClassFixture<SharedMixCmsDbFixture>
    {
        private readonly InitCmsService _initCmsService;
        public Step_0_InitCmsTest(
            SharedMixCmsDbFixture fixture, 
            InitCmsService initCmsService) : base(fixture)
        {
            _initCmsService = initCmsService;
        }

        [Fact, TestPriority(1)]
        public async Task Step_1_Init_Site()
        {
            InitCmsDto model = new()
            {
                Culture = new()
                {
                    FullName = "United States - English",
                    Icon = "flag-icon-us",
                    Specificulture = "en-us"
                },
                DatabaseProvider = MixDatabaseProvider.SQLITE,
                SiteName = "Test",
                SqliteDbConnectionString = Fixture._connectionString
            };
            try
            {
                Fixture.Context.Database.EnsureDeleted();
                await _initCmsService.InitTenantAsync(model);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
