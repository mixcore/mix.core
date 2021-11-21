using Microsoft.EntityFrameworkCore;
using Mix.Heart.UnitOfWork;

// Ref: https://docs.microsoft.com/en-us/dotnet/core/testing/order-unit-tests
// Need to turn off test parallelization so we can validate the run order

[assembly: CollectionBehavior(DisableTestParallelization = true)]
[assembly: TestCollectionOrderer("Mix.XUnittest.Domain.Orderers.DisplayNameOrderer", "mix.xunittest")]

namespace Mix.Xunittest.Domain.Base
{
    [TestCaseOrderer("Mix.XUnittest.Domain.Orderers.PriorityOrderer", "mix.xunittest")]
    public abstract class TestBase<TFixture, TDbContext>
         : IClassFixture<TFixture>
        where TFixture : SharedDatabaseFixture<TDbContext>
        where TDbContext : DbContext
    {
        public static TFixture DbFixture { get; set; }

        protected UnitOfWorkInfo UowInfo { get; set; }

        public TestBase(TFixture fixture)
        {
            DbFixture = fixture;
            
            //TODO: Update db connection string here to test other db provider
            //Fixture._connectionString = "";
            //Fixture._dbProvider = MixDatabaseProvider.SQLSERVER;
            
            UowInfo = new UnitOfWorkInfo(fixture.Context);
        }
    }
}
