using Microsoft.EntityFrameworkCore;
using Mix.Heart.Entities;
using Mix.Heart.Exceptions;
using Mix.Heart.Helpers;
using Mix.Heart.Repository;
using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;



namespace Mix.Xunittest.Domain.Base
{
    [TestCaseOrderer("Mix.XUnittest.Domain.Orderers.PriorityOrderer", "mix.xunittest")]
    public abstract class ViewModelTestBase<TFixture, TView, TDbContext, TEntity, TPrimaryKey>
         : TestBase<TFixture, TDbContext>
        where TFixture : SharedDatabaseFixture<TDbContext>
        where TView : ViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
        where TDbContext : DbContext
        where TPrimaryKey : IComparable
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected Repository<TDbContext, TEntity, TPrimaryKey, TView> Repository { get; set; }
        
        public ViewModelTestBase(TFixture fixture) : base(fixture)
        {
            Fixture = fixture;
            UowInfo = new UnitOfWorkInfo(fixture.Context);
            Repository = new(UowInfo);
        }

        #region Abstracts

        protected abstract TView CreateSampleValue();

        #endregion

        [Fact, TestPriority(1)]
        public async Task Step_1_Save()
        {
            TView valueToAdd = CreateSampleValue();
            var key = await valueToAdd.SaveAsync(UowInfo);
            await UowInfo.CompleteAsync();
            Assert.True(key != null, key.ToString());
        }

        [Fact, TestPriority(2)]
        public async Task Step_2_GetList()
        {
            var data = await Repository.GetListAsync(m => true);
            Assert.True(data.Count > 0);
        }

        [Fact, TestPriority(3)]
        public async Task Step_3_Delete()
        {
            try
            {
                var predicate = ReflectionHelper.GetExpression<TEntity>("Id", 1, ExpressionMethod.Eq);
                await Repository.DeleteAsync(predicate);
                await UowInfo.CompleteAsync();
                Assert.True(true);
            }
            catch (MixException mex)
            {
                Assert.True(false, mex.Message);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }
    }
}
