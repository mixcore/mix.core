using Mix.Lib.Helpers;
using Mix.Shared.Constants;
using Mix.Tenancy.Domain.Dtos;
using Mix.Tenancy.Domain.Services;


namespace Mix.Xunittest.Domain.Tests
{
    [Collection("Step 0 - Init")]
    public class c0_InitCmsTest
        : TestBase<SharedMixCmsDbFixture, MixCmsContext>, IClassFixture<SharedMixCmsDbFixture>
    {
        private readonly InitCmsService _initCmsService;
        public c0_InitCmsTest(
            SharedMixCmsDbFixture fixture, 
            InitCmsService initCmsService) : base(fixture)
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
            try
            {
                MixCmsHelper.CopyFolder("../../../../../shared/MixContent", MixFolders.ConfiguratoinFolder);
                DbFixture.Context = new(DbFixture.ConnectionString, DbFixture.DbProvider);
                DbFixture.Context.Database.EnsureDeleted();
                await _initCmsService.InitTenantAsync(model);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
